using forum.Models.Notification;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace forum.Services
{
    public interface INotificationService
    {
        Task<IEnumerable<UserNotification>> GetNotificationsForUser(int userId);
        Task ChangeStatusNotification(int notificationId);
        Task MarkAllAsRead(int userId);
    }
}
