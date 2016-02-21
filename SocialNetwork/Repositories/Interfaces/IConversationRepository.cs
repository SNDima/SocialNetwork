using System.Collections.Generic;
using SocialNetwork.Models;

namespace SocialNetwork.Repositories.Interfaces
{
    public interface IConversationRepository : IRepository<Conversation, int>
    {
        Conversation GetTwoUserConversation(IEnumerable<ApplicationUser> users);

        IEnumerable<Conversation> GetCommonConversations(
            IEnumerable<ApplicationUser> users);

        IEnumerable<Conversation> GetUserConversations(ApplicationUser user);

        IEnumerable<Conversation> GetConversations(
            IEnumerable<UserToConversationLink> links);

        Conversation AddConversation();

        long GetLastMessageId(int conversationId);

        IEnumerable<ApplicationUser> GetConversationParticipants(
            int conversationId);

        UserToConversationLink Get(string userId, int conversationId);

        long GetLastReadMessageId(int conversationId,
            string userId);

        IEnumerable<Message> GetUnreadMessages(int conversationId,
            long lastReadMessageId);

        IEnumerable<Message> GetUnreadMessages(int conversationId,
            string userId);

        IEnumerable<Message> GetReadMessages(int conversationId,
            long lastReadMessageId);

        IEnumerable<Message> GetReadMessages(int conversationId, string userId);

        int GetUnreadMessagesNumber(int conversationId, long lastReadMessageId);

        void ChangeConversationName(int conversationId, string name);
    }
}
