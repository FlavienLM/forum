using forum.Data;
using forum.Models;
using Microsoft.EntityFrameworkCore;

namespace forum.Services
{
    public class UserService(ForumDbContext dbContext) : IUserService
    {
        private readonly ForumDbContext _dbContext = dbContext;

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _dbContext.User.ToListAsync();
        }

        public async Task<int> CreateUserAsync(User user)
        {
            _dbContext.User.Add(user);
            await _dbContext.SaveChangesAsync();
            return user.Id;
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _dbContext.User
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<IEnumerable<object>> GetFriendWithWhomUserLastInteracted(int userId)
        {
            var friendsWithLastChat = await _dbContext.Friendship
                .Where(f => f.SenderId == userId || f.ReceiverId == userId) // Get friends of the user
                .Select(f => new
                {
                    FriendId = f.SenderId == userId ? f.ReceiverId : f.SenderId, // Get the ID of the friend
                    Pseudo = f.SenderId == userId ? f.Receiver.Pseudo : f.Sender.Pseudo, // Get the ID of the friend
                    AccountId = f.SenderId == userId ? f.Receiver.AccountId : f.Sender.AccountId, // Get the ID of the friend
                    f.LastMessage,
                    Date = _dbContext.FriendMessage
                        .Where(fc => (fc.SenderId == userId && fc.ReceiverId == (f.SenderId == userId ? f.ReceiverId : f.SenderId)) ||
                                      (fc.ReceiverId == userId && fc.SenderId == (f.SenderId == userId ? f.ReceiverId : f.SenderId)))
                        .OrderByDescending(fc => fc.Date)
                        .Select(fc => fc.Date)
                        .FirstOrDefault() // Get the date of the last chat
                })
                .OrderByDescending(fc => fc.Date) // Sort by the date of the last chat
                .ToListAsync();

            return friendsWithLastChat;
        }

        public async Task UpdateUser(User userInfo)
        {
            var user = _dbContext.User.FirstOrDefault(u => u.Id == userInfo.Id);

            if (user != null)
            {
                user.Pseudo = userInfo.Pseudo;
                user.Profile = userInfo.Profile;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("User not found");
            }

            await _dbContext.SaveChangesAsync();

        }
    }
}
