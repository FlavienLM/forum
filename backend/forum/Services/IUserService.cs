using forum.Models;

namespace forum.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<int> CreateUserAsync(User user);

        Task UpdateUser(User user);

        Task<User> GetUserById(int userId);

        // Get the friend with whom the user last interacted
        Task<IEnumerable<object>> GetFriendWithWhomUserLastInteracted(int userId);


    }
}