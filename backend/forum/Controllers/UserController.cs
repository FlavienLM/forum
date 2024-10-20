using forum.Models;
using forum.Services; // Import the service namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace forum.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class UserController(IUserService userService, ILogger<UserController> logger) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly ILogger<UserController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch users");
                return StatusCode(500, "An error occurred while fetching users");
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser([FromRoute]int userId)
        {
            try
            {
                var users = await _userService.GetUserById(userId);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch users");
                return StatusCode(500, "An error occurred while fetching users");
            }
        }

        [HttpGet("friend/{userId}")]
        public async Task<IActionResult> GetFriendWithWhomUserLastInteracted([FromRoute] int userId)
        {
            try
            {
                var friendsWithLastChat = await _userService.GetFriendWithWhomUserLastInteracted(userId);
                return Ok(friendsWithLastChat);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch friend interactions");
                return StatusCode(500, "An error occurred while fetching friends with whom user last interacted.");
            }
        }
        

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {            
            if (user == null)
            {
                return BadRequest("User data is invalid.");
            }

            var createdUserId = await _userService.CreateUserAsync(user);
            return Ok(new { userId = createdUserId });
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {            
            if (user == null)
            {
                return BadRequest("User data is invalid.");
            }

            await _userService.UpdateUser(user);
            return Ok("User successfuly modified");
        }
    }
}
