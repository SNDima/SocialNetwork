using System.Collections.Generic;
using System.Linq;
using SocialNetwork.Models;
using SocialNetwork.Repositories.Interfaces;

namespace SocialNetwork.Repositories.Implementations
{
    public class UserRepository
        : Repository<ApplicationUser, string>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public bool ContainsUser(
            IEnumerable<ApplicationUser> users, string userId)
        {
            if (users.ToList().Find(user => user.Id == userId) == null)
            {
                return false;
            }
            return true;
        }

        public IEnumerable<ApplicationUser> GetUsersExceptOneUser(string userId)
        {
            var excludedUsers = new List<ApplicationUser> {Get(userId)};
            return GetAll().Except(excludedUsers);
        }

        public bool IsPartisipant(int conversationId, string userId)
        {
            return Get(userId).Links.SingleOrDefault(
                link => link.ConversationId == conversationId) != null;
        }

        public IEnumerable<ApplicationUser> GetAllUsersExceptSomeUsers(
            IEnumerable<string> usersIds)
        {
            List<ApplicationUser> notIncludedUsers = 
                new List<ApplicationUser>();
            if (usersIds != null)
            {
                foreach (string userId in usersIds)
                {
                    notIncludedUsers.Add(Get(userId));
                }
            }
            return GetAll().Except(notIncludedUsers);
        }
    }
}