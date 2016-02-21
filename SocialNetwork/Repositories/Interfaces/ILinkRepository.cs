using SocialNetwork.Models;

namespace SocialNetwork.Repositories.Interfaces
{
    public interface ILinkRepository : IRepository<UserToConversationLink, int>
    {
        void AddLink(ApplicationUser user, Conversation newConversation,
            Message newMessage);

        void AddLink(string userId, int conversationId, long lastReadMessageId);

        UserToConversationLink Find(int conversationId, string userId);

        void RemoveLink(int conversationId, string userId);
    }
}
