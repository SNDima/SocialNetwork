using System.Collections.Generic;
using SocialNetwork.Models;

namespace SocialNetwork.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<ApplicationUser, string>
    {
        bool ContainsUser(IEnumerable<ApplicationUser> users, string userId);

        IEnumerable<ApplicationUser> GetUsersExceptOneUser(string userId);

        bool IsPartisipant(int conversationId, string userId);

        IEnumerable<ApplicationUser> GetAllUsersExceptSomeUsers(
            IEnumerable<string> usersIds);
    }
}