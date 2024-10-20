using forum.Models.Notification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using forum.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace forum.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ILogger<NotificationController> _logger;
        private readonly INotificationService _notificationService;

        public NotificationController(ILogger<NotificationController> logger, INotificationService notificationService)
        {
            _logger = logger;
            _notificationService = notificationService;
        }

        [HttpGet("discussion/{id}")]
        public async Task<IActionResult> GetDiscussionNotificationOfUser([FromRoute] int id)
        {
            try
            {
                var notifications = await _notificationService.GetNotificationsForUser(id);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch notifications from the database");
                return StatusCode(500, "An error occurred while fetching notifications");
            }
        }

        [HttpPut]
        public async Task<IActionResult> MarkAsReadFriendNotificationOfUser([FromBody] int id)
        {

            System.Console.WriteLine("Le id est "+id);

            try
            {
                await _notificationService.ChangeStatusNotification(id);
                return Ok("Notification properly marked as read");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to mark notification as read");
                return StatusCode(500, "An error occurred while marking the notification as read");
            }
        }

        [HttpPut("user/{userId}")]
        public async Task<IActionResult> MarkAsReadAllNotificationOfUser([FromRoute] int userId)
        {
            try
            {
                await _notificationService.MarkAllAsRead(userId);
                return Ok("All notifications properly marked as read");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to mark all notifications as read");
                return StatusCode(500, "An error occurred while marking notifications as read");
            }
        }
    }
}
