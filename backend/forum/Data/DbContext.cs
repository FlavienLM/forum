using Microsoft.EntityFrameworkCore;

namespace forum.Data
{
    public class ForumDbContext : DbContext
    {
        public ForumDbContext(DbContextOptions<ForumDbContext> options) : base(options)
        {

        }
        public DbSet<Models.User> User { get; set; }
        public DbSet<Models.Friendship> Friendship { get; set; }
        public DbSet<Models.Message> Message { get; set; }
        public DbSet<Models.FriendMessage> FriendMessage { get; set; }
        public DbSet<Models.Discussion> Discussion { get; set; }

        public DbSet<Models.Notification.UserNotification> UserNotification { get; set; }

        

    }

}
