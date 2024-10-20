using forum.Data;
using forum.Models;
using forum.Models.Notification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace forum.Services
{
    public class FriendService(ForumDbContext dbContext) : IFriendService
    {
        private readonly ForumDbContext _dbContext = dbContext;

        public async Task<object> GetAllFriendshipsRelations()
        {
            return await _dbContext.Friendship.ToListAsync();
        }

        public async Task<object> GetFriendshipRelationOfUserById(int id)
        {
            return await _dbContext.Friendship
                .Where(f => f.ReceiverId == id || f.SenderId == id)
                .ToListAsync();
        }

        public async Task<string> CreateNewFriendRequest(Friendship friend)
        {

            friend.Status = FriendshipStatus.Pending;
            _dbContext.Friendship.Add(friend);

            await _dbContext.SaveChangesAsync();

            var sender = await _dbContext.User.Where(s => s.Id == friend.SenderId).Select(s => s.AccountId).FirstOrDefaultAsync();

            // Create a notification
            var notification = new UserNotification
            {
                ReferenceId = friend.SenderId,
                Date = friend.Date,
                ReceiverId = friend.ReceiverId,
                NotificationType = 2,
                FriendRequestId = friend.Id,
                IsRead = false,
                ReferenceString = sender
            };

            _dbContext.UserNotification.Add(notification);
            await _dbContext.SaveChangesAsync();

            return "Friend request sent successfully.";
        }

        public async Task<string> AcceptFriendRequest(FriendRequestAnswerDto data)
        {
            var friendship = await _dbContext.Friendship.FirstOrDefaultAsync(f => f.Id == data.Id);
            if (friendship == null)
            {
                return "Friend request not found.";
            }

            friendship.Status = FriendshipStatus.Accepted;
            await _dbContext.SaveChangesAsync();

            return "Friend request accepted.";
        }

        public async Task<string> DeclineFriendRequest(FriendRequestAnswerDto data)
        {
            var friendship = await _dbContext.Friendship.FirstOrDefaultAsync(f => f.Id == data.Id);
            if (friendship == null)
            {
                return "Friend request not found.";
            }

            friendship.Status = FriendshipStatus.Rejected;
            await _dbContext.SaveChangesAsync();

            return "Friend request declined.";
        }

        public async Task<string> GetFriendshipStatus(int user1Id, int user2Id)
        {
            var friendship = await _dbContext.Friendship
                .Where(fc => (fc.SenderId == user1Id && fc.ReceiverId == user2Id) ||
                             (fc.SenderId == user2Id && fc.ReceiverId == user1Id))
                .FirstOrDefaultAsync(); // Retrieve the first match

            // Check if a friendship exists
            if (friendship == null)
            {
                return "none";
            }

            // Return the friendship status as a string
            return friendship.Status.ToString();  // Status is of type FriendshipStatus
        }
    }
}
