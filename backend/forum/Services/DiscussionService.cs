using forum.Data;
using forum.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace forum.Services
{
    public class DiscussionService(ForumDbContext dbContext) : IDiscussionService
    {
        private readonly ForumDbContext _dbContext = dbContext;

        public async Task<bool> CreateDiscussion(Discussion discussion)
        {
            _dbContext.Discussion.Add(discussion);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<DiscussionDto>> GetAllDiscussions()
        {
            var discussions = await _dbContext.Discussion
                .Include(d => d.Creator) // Include the entire Creator entity
                .ToListAsync();

            return discussions.Select(d => new DiscussionDto
            {
                Id = d.Id,
                Title = d.Title,
                Description = d.Description,
                Date = d.Date,
                LastMessage = d.LastMessage,
                CreatorId = d.Creator.Id,
                CreatorIdentifiant = d.Creator.AccountId,
                CommentCount = _dbContext.Message.Count(m => m.DiscussionId == d.Id)
            }).ToList();
        }

        public async Task<Discussion> GetDiscussionById([FromRoute] int discussionId)
        {
            return await _dbContext.Discussion.FirstOrDefaultAsync(d => d.Id == discussionId);
        }

        public async Task<IEnumerable<Discussion>> GetDiscussionCreatedByUser(int userId)
        {

            var discussions = await _dbContext.Discussion
                .Where(d => d.CreatorId == userId)
                .OrderBy(d => d.Date)
                .ToListAsync();
            return discussions;
        }

        public async Task<IEnumerable<MessageDTO>> GetDiscussionMessages(int discussionId)
        {
            var discussions = await _dbContext.Message
                .Where(d => d.DiscussionId == discussionId)
                .Select(d => new MessageDTO
                {
                    Id = d.Id,
                    Content = d.Content,
                    Date = d.Date,
                    Status = d.Status,
                    LastModified = d.LastModified,
                    AuthorId = d.AuthorId, 
                    Pseudo = d.Author.Pseudo,
                })
                .ToListAsync();

            return discussions;
        }

        public async Task<IEnumerable<Discussion>> GetDiscussionWhereUserLastParticipated(int userId)
        {
            var discussions = await _dbContext.Discussion
                   .Where(d => _dbContext.Message
                               .Any(dm => dm.DiscussionId == d.Id && dm.AuthorId == userId))
                   .OrderByDescending(d => _dbContext.Message
                                          .Where(dm => dm.DiscussionId == d.Id)
                                          .Max(dm => dm.Date)) // Sort by the latest message date
                   .ToListAsync();
            return discussions;
        }


        public async Task<IEnumerable<FriendMessage>> GetMessageBetweenFriends(int user1Id, int user2Id)
        {
            var friendMessage = await _dbContext.FriendMessage
                .Where(fc => (fc.SenderId == user1Id && fc.ReceiverId == user2Id) ||
                             (fc.SenderId == user2Id && fc.ReceiverId == user1Id))
                .OrderBy(fc => fc.Date) // Optional: Order by the message date
                .ToListAsync();


            return friendMessage;
        }

        public async Task<bool> ModifyDiscussionDescription(int discussionId, string newDescription)
        {
            var discussion = await _dbContext.Discussion.FirstOrDefaultAsync(d => d.Id == discussionId);

            discussion.Description = newDescription;

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ModifyMessageContentInDiscussion(int messageId, string newContent)
        {
            var message = await _dbContext.Message
            .FirstOrDefaultAsync(m => m.Id == messageId);

            message.Content = newContent;

            message.LastModified = DateTime.Now;

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ModifyMessageContentInFriendDiscussion([FromRoute] int messageId, [FromBody] string newContent)
        {
            var message = await _dbContext.FriendMessage
            .FirstOrDefaultAsync(m => m.Id == messageId);

            message.Content = newContent;

            message.LastModified = DateTime.Now;

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SendNewFriendMessage(FriendMessage message)
        {
            _dbContext.FriendMessage.Add(message);


            var friend = await _dbContext.Friendship.
              FirstOrDefaultAsync(f => f.ReceiverId == message.ReceiverId && f.SenderId == message.SenderId ||
              f.ReceiverId == message.SenderId && f.SenderId == message.ReceiverId);

            friend.LastMessage = message.Content;

            var notification = new Models.Notification.UserNotification
            {
                ReferenceId = message.SenderId,
                Date = message.Date,
                ReceiverId = message.ReceiverId,
                NotificationType = 0,
                ReferenceString = message.Sender.AccountId
            };

            // Add the notification to the database
            _dbContext.UserNotification.Add(notification);

            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SendNewMessageInDiscussion(Message message)
        {
            // Validate message content
            if (message == null || string.IsNullOrEmpty(message.Content))
            {
                return false;
            }

            // Add new message to the discussion
            _dbContext.Message.Add(message);

            // Fetch the related discussion and update the last message
            var discussion = await _dbContext.Discussion
                .FirstOrDefaultAsync(d => d.Id == message.DiscussionId);

            // If the discussion doesn't exist, return 404 NotFound
            if (discussion == null)
            {
                return false;
            }

            // Update the discussion with the latest message content
            discussion.LastMessage = message.Content;

            // Get all participants who have sent messages in the discussion except the current message author
            var discussionParticipants = await _dbContext.User
                .Where(u => _dbContext.Message
                    .Any(m => m.AuthorId == u.Id && m.DiscussionId == message.DiscussionId && m.AuthorId != message.AuthorId))
                .ToListAsync();

            // Create and add notifications for each participant
            foreach (var participant in discussionParticipants)
            {
                var notification = new Models.Notification.UserNotification
                {
                    ReferenceId = discussion.Id,
                    Date = message.Date,
                    ReceiverId = participant.Id,
                    NotificationType = 1, // 1: Represents new message notification
                    ReferenceString = discussion.Title
                };

                _dbContext.UserNotification.Add(notification);
            }

            // Save changes (new message and notifications)
            await _dbContext.SaveChangesAsync();

            // Return a success response (no object returned, just 200 OK)
            return true;
        }

    }
}
