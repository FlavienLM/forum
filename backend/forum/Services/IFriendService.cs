using forum.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace forum.Services
{
    public interface IFriendService
    {
        Task<object> GetAllFriendshipsRelations(); // Change the return type as per your requirement
        Task<object> GetFriendshipRelationOfUserById(int id); // Change the return type as per your requirement
        Task<string> CreateNewFriendRequest(Friendship friend);
        Task<string> AcceptFriendRequest(FriendRequestAnswerDto data);
        Task<string> DeclineFriendRequest(FriendRequestAnswerDto data);
        Task<string> GetFriendshipStatus(int user1Idint,int user2Id);

    }
}
