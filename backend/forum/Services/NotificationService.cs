using forum.Data;
using forum.Models.Notification;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace forum.Services
{
    public class NotificationService : INotificationService
    {
        private readonly ForumDbContext _dbContext;

        public NotificationService(ForumDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<UserNotification>> GetNotificationsForUser(int userId)
        {
            return await _dbContext.UserNotification
                .Include(n => n.Receiver)
                .Where(n => n.ReceiverId == userId && !(n.IsRead && n.NotificationType == 2))
                .OrderByDescending(n => n.Date)
                .ToListAsync();
        }

        public async Task ChangeStatusNotification(int notificationId)
        {
            var notification = await _dbContext.UserNotification.FindAsync(notificationId);
            if (notification != null)
            {
                notification.IsRead =! notification.IsRead;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task MarkAllAsRead(int userId)
        {
            var notifications = await _dbContext.UserNotification
                .Where(n => n.ReceiverId == userId)
                .ToListAsync();

            foreach (var notification in notifications)
            {
                notification.IsRead = true;
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
