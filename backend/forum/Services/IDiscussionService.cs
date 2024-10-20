using forum.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace forum.Services
{
    public interface IDiscussionService
    {
        // Get all discussions with pagination support
        Task<IEnumerable<DiscussionDto>> GetAllDiscussions();
        Task<Discussion> GetDiscussionById([FromRoute] int discussionId);

        // Get discussions created by a specific user with pagination
        Task<IEnumerable<Discussion>> GetDiscussionCreatedByUser(int userId);

        // Get all messages in a discussion with pagination support
        Task<IEnumerable<MessageDTO>> GetDiscussionMessages(int discussionId);

        // Send a new message in a discussion
        Task<bool> SendNewMessageInDiscussion(Message message);

        // Modify the content of a message in a discussion
        Task<bool> ModifyMessageContentInDiscussion(int messageId, string newContent);

        Task<bool> ModifyMessageContentInFriendDiscussion([FromRoute] int id, [FromBody] string newContent);

        // Get the last discussion where a user participated
        Task<IEnumerable<Discussion>> GetDiscussionWhereUserLastParticipated(int userId);


        // Get messages between two friends with pagination
        Task<IEnumerable<FriendMessage>> GetMessageBetweenFriends(int user1Idint,int user2Id);

        // Send a new message between friends
        Task<bool> SendNewFriendMessage(FriendMessage message);

        Task<bool> CreateDiscussion(Discussion discussion); // Add this method to create discussions
        Task<bool> ModifyDiscussionDescription(int DiscussionId, string newDescription);

    }
}
