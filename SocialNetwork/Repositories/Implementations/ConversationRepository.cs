using System.Collections.Generic;
using System.Linq;
using SocialNetwork.Models;
using SocialNetwork.Repositories.Interfaces;

namespace SocialNetwork.Repositories.Implementations
{
    public class ConversationRepository :
        Repository<Conversation, int>, IConversationRepository
    {
        // For finding a conversation with no more than 2 people
        private const int CommonConversationParticipants = 2;

        public ConversationRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public Conversation GetTwoUserConversation(
            IEnumerable<ApplicationUser> users)
        {
            var commonConversations = GetCommonConversations(users);
            foreach (var conversation in commonConversations)
            {
                if (conversation.Links.Count == CommonConversationParticipants)
                {
                    return conversation;
                }
            }
            return null;
        }

        public IEnumerable<Conversation> GetCommonConversations(
            IEnumerable<ApplicationUser> users)
        {
            var commonConversations = GetUserConversations(users.First());
            foreach (var user in users)
            {
                var userConversations = GetUserConversations(user);
                commonConversations =
                    commonConversations.Intersect(userConversations);
            }
            return commonConversations;
        }

        public IEnumerable<Conversation> GetUserConversations(
            ApplicationUser user)
        {
            return user.Links.Select(link => link.Conversation);
        }

        public IEnumerable<Conversation> GetConversations(
            IEnumerable<UserToConversationLink> links)
        {
            return links.Select(link => link.Conversation);
        }

        public Conversation AddConversation()
        {
            Conversation newConversation = new Conversation();
            Add(newConversation);
            Context.SaveChanges();
            return newConversation;
        }

        public long GetLastMessageId(int conversationId)
        {
            return Get(conversationId).Messages.LastOrDefault().Id;
        }

        public IEnumerable<ApplicationUser> GetConversationParticipants(
            int conversationId)
        {
            return Get(conversationId).Links.Select(c => c.User);
        }

        public UserToConversationLink Get(string userId, int conversationId)
        {
            return Get(conversationId).Links.SingleOrDefault(
                link => link.UserId == userId);
        }

        public long GetLastReadMessageId(
            int conversationId, string userId)
        {
            return Get(conversationId).Links
                .Where(link => link.UserId == userId)
                .Select(link => link.LastReadMessageId)
                .SingleOrDefault();
        }

        public IEnumerable<Message> GetUnreadMessages(
            int conversationId, long lastReadMessageId)
        {
            return Get(conversationId).Messages
                .Where(message => message.Id > lastReadMessageId);
        }

        public IEnumerable<Message> GetUnreadMessages(
            int conversationId, string userId)
        {
            long lastReadMessageId = GetLastReadMessageId(
                conversationId, userId);
            return GetUnreadMessages(conversationId, lastReadMessageId);
        }

        public IEnumerable<Message> GetReadMessages(
            int conversationId, long lastReadMessageId)
        {
            return Get(conversationId).Messages
                .Where(message => message.Id <= lastReadMessageId);
        }

        public IEnumerable<Message> GetReadMessages(
            int conversationId, string userId)
        {
            long lastReadMessageId = GetLastReadMessageId(
                conversationId, userId);
            return GetReadMessages(conversationId, lastReadMessageId);
        }

        public int GetUnreadMessagesNumber(
            int conversationId, long lastReadMessageId)
        {
            return GetUnreadMessages(conversationId, lastReadMessageId).Count();
        }

        public void ChangeConversationName(int conversationId, string name)
        {
            Get(conversationId).Name = name;
            Context.SaveChanges();
        }
    }
}