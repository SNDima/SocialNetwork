using SocialNetwork.Models;

namespace SocialNetwork.Repositories.Interfaces
{
    public interface IMessageRepository : IRepository<Message, long>
    {
        Message AddMessage(string senderId, Message newMessage,
            int newConversationId);

        Message AddMessage(string senderId, string content,
            int newConversationId);
    }
}