using System;
using SocialNetwork.Controllers;
using SocialNetwork.Models;
using SocialNetwork.Repositories.Interfaces;

namespace SocialNetwork.Repositories.Implementations
{
    public class MessageRepository
        : Repository<Message, long>, IMessageRepository
    {
        public MessageRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Message AddMessage(string senderId, Message newMessage,
            int newConversationId)
        {
            string senderName = AccountController.GetUsernameByUserId(senderId);
            newMessage.DepartureTime = DateTime.Now;
            newMessage.SenderName = senderName;
            newMessage.ConversationId = newConversationId;
            Add(newMessage);
            Context.SaveChanges();
            return newMessage;
        }

        public Message AddMessage(string senderId, string content,
            int newConversationId)
        {
            Message newMessage = new Message
            {
                Content = content
            };
            return AddMessage(senderId, newMessage, newConversationId);
        }
    }
}