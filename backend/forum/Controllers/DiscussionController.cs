using forum.Models;
using forum.Services; // Add this line to use the DiscussionService
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace forum.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class DiscussionController : ControllerBase
    {
        private readonly ILogger<DiscussionController> _logger;
        private readonly IDiscussionService _discussionService;

        public DiscussionController(ILogger<DiscussionController> logger, IDiscussionService discussionService)
        {
            _logger = logger;
            _discussionService = discussionService; // Use the service here
        }

        [HttpGet]
        public async Task<IActionResult> GetDiscussions()
        {
            try
            {
                var discussions = await _discussionService.GetAllDiscussions();
                return Ok(discussions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch discussions from the database");
                return StatusCode(500, "An error occurred while fetching discussions");
            }
        }

        [HttpGet("single/{discussionId}")]
        public async Task<IActionResult> GetDiscussionById([FromRoute]int discussionId)
        {
            try
            {
                var discussion = await _discussionService.GetDiscussionById(discussionId);
                return Ok(discussion);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch discussions from the database");
                return StatusCode(500, "An error occurred while fetching discussions");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateDiscussion([FromBody] Discussion discussion)
        {
            if (discussion == null || string.IsNullOrEmpty(discussion.Title))
            {
                return BadRequest("Discussion data is invalid.");
            }

            await _discussionService.CreateDiscussion(discussion); // Call the service method

            // Return the ID of the created discussion
            return Ok(new { discussionId = discussion.Id });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDiscussionCreatedByUser([FromRoute] int id)
        {
            try
            {
                var discussions = await _discussionService.GetDiscussionCreatedByUser(id);
                return Ok(discussions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch discussions from the database");
                return StatusCode(500, "An error occurred while fetching discussions");
            }
        }

        [HttpGet("message/{id}")]
        public async Task<IActionResult> GetDiscussionMessages([FromRoute] int id)
        {
            try
            {
                var messages = await _discussionService.GetDiscussionMessages(id);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch messages from the database");
                return StatusCode(500, "An error occurred while fetching messages");
            }
        }

        [HttpPost("message")]
        public async Task<IActionResult> SendNewMessageInDiscussion([FromBody] Message message)
        {
            var result = await _discussionService.SendNewMessageInDiscussion(message);
            if (!result)
            {
                return BadRequest("Message content cannot be empty or discussion not found.");
            }

            return Ok(new { Message = "Message created successfully", Id = message.Id });
        }

        [HttpPut("message/{id}")]
        public async Task<IActionResult> ModifyMessageContentInDiscussion([FromRoute] int id, [FromBody] string newContent)
        {
            try
            {
                var result = await _discussionService.ModifyMessageContentInDiscussion(id, newContent);
                if (!result)
                {
                    return NotFound("Message not found.");
                }

                return Ok("Message content updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update message content");
                return StatusCode(500, "An error occurred while updating message content.");
            }
        }

        [HttpPut("description/{discussionId}")]
        public async Task<IActionResult> ModifyDiscussionDescription([FromRoute] int discussionId, [FromBody] string newDescripton)
        {
            try
            {
                var result = await _discussionService.ModifyDiscussionDescription(discussionId, newDescripton);
                if (!result)
                {
                    return NotFound("Message not found.");
                }

                return Ok("Message content updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update message content");
                return StatusCode(500, "An error occurred while updating message content.");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetDiscussionWhereUserLastParticipated([FromRoute] int userId)
        {
            try
            {
                var discussions = await _discussionService.GetDiscussionWhereUserLastParticipated(userId);
                return Ok(discussions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch discussions from the database");
                return StatusCode(500, "An error occurred while fetching discussions");
            }
        }

        [HttpGet("friendChat/{user1Id}/{user2Id}")]
        public async Task<IActionResult> GetMessageBetweenFriends([FromRoute] int user1Id, [FromRoute] int user2Id)
        {
            try
            {
                var friendChats = await _discussionService.GetMessageBetweenFriends(user1Id, user2Id);
                return Ok(friendChats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch friend chat between users");
                return StatusCode(500, "An error occurred while fetching the chat.");
            }
        }

        [HttpPost("friendChat")]
        public async Task<IActionResult> SendNewFriendMessage([FromBody] FriendMessage message)
        {

            try
            {
                var result = await _discussionService.SendNewFriendMessage(message);
                if (!result)
                {
                    return BadRequest("Failed to send the friend message.");
                }

                return Ok(new { Message = "Message sent successfully", Id = message.Id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send a friend message");
                return StatusCode(500, "An error occurred while sending the friend message.");
            }
        }

        [HttpPut("friendChat/{id}")]
        public async Task<IActionResult> ModifyMessageContentInFriendDiscussion([FromRoute] int id, [FromBody] string newContent)
        {
            try
            {
                var result = await _discussionService.ModifyMessageContentInFriendDiscussion(id, newContent);
                if (!result)
                {
                    return NotFound("Message not found.");
                }

                return Ok("Message content updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update message content");
                return StatusCode(500, "An error occurred while updating message content.");
            }
        }
    }
}
