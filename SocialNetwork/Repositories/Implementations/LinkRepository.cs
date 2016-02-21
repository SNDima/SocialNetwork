using System.Linq;
using SocialNetwork.Models;
using SocialNetwork.Repositories.Interfaces;

namespace SocialNetwork.Repositories.Implementations
{
    public class LinkRepository
        : Repository<UserToConversationLink, int>, ILinkRepository
    {
        public LinkRepository(ApplicationDbContext context) : base(context)
        {
        }

        public void AddLink(ApplicationUser user, Conversation newConversation,
            Message newMessage)
        {
            AddLink(user.Id, newConversation.Id, newMessage.Id);
        }

        public void AddLink(string userId, int conversationId,
            long lastReadMessageId)
        {
            UserToConversationLink link = new UserToConversationLink
            {
                UserId = userId,
                ConversationId = conversationId,
                LastReadMessageId = lastReadMessageId
            };
            Add(link);
            Context.SaveChanges();
        }

        public UserToConversationLink Find(int conversationId, string userId)
        {
            return GetAll().SingleOrDefault(link => link.ConversationId
            == conversationId && link.UserId == userId);
        }

        public void RemoveLink(int conversationId, string userId)
        {
            Remove(Find(conversationId, userId));
            Context.SaveChanges();
        }
    }
}