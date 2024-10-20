using forum.Models;
using forum.Models.Notification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using forum.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace forum.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly ILogger<FriendController> _logger;
        private readonly IFriendService _friendService;

        public FriendController(ILogger<FriendController> logger, IFriendService friendService)
        {
            _logger = logger;
            _friendService = friendService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFriendRelations()
        {
            try
            {
                var friendships = await _friendService.GetAllFriendshipsRelations();
                return Ok(friendships);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch friend relations from the database");
                return StatusCode(500, "An error occurred while fetching friend relations");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFriendsOfUser([FromRoute] int id)
        {
            try
            {
                var friends = await _friendService.GetFriendshipRelationOfUserById(id);
                return Ok(friends);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch friend relations from the database");
                return StatusCode(500, "An error occurred while fetching friend relations");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFriendRequest([FromBody] Friendship friend)
        {
            if (friend == null)
            {
                return BadRequest("Friend data is invalid.");
            }

            try
            {
                var response = await _friendService.CreateNewFriendRequest(friend);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create friend request");
                return StatusCode(500, "An error occurred while creating friend request");
            }
        }

        [HttpPost("accept")]
        public async Task<IActionResult> AcceptFriendRequest([FromBody] FriendRequestAnswerDto data)
        {
            if (data == null)
            {
                return BadRequest("Friend data is invalid.");
            }

            try
            {
                var response = await _friendService.AcceptFriendRequest(data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to accept friend request");
                return StatusCode(500, "An error occurred while accepting friend request");
            }
        }

        [HttpPost("decline")]
        public async Task<IActionResult> DeclineFriendRequest([FromBody] FriendRequestAnswerDto data)
        {
            if (data == null)
            {
                return BadRequest("Friend data is invalid.");
            }

            try
            {
                var response = await _friendService.DeclineFriendRequest(data);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to accept friend request");
                return StatusCode(500, "An error occurred while accepting friend request");
            }
        }

        [HttpGet("{user1Id}/{user2Id}")]
        public async Task<IActionResult> GetFriendshipStatus([FromRoute] int user1Id, [FromRoute] int user2Id)
        {
            try
            {
                var status = await _friendService.GetFriendshipStatus(user1Id, user2Id);
                return Ok(status);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch friend chat between users");
                return StatusCode(500, "An error occurred while fetching the chat.");
            }
        }
    }

}
