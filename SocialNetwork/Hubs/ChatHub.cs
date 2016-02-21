using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using SocialNetwork.Controllers;
using SocialNetwork.Models;
using SocialNetwork.Repositories;

namespace SocialNetwork.Hubs
{
    public class ChatHub : Hub
    {
        readonly UnitOfWork unitOfWork 
            = new UnitOfWork(new ApplicationDbContext());

        public void Send(string message, int conversationId)
        {
            Message newMessage = AddMessage(message, conversationId);
            UpdateLastReadMessageId(newMessage.ConversationId, newMessage.Id);
            SendMessageToCurrentUser(newMessage);
            SendMessageToOtherUsers(newMessage);
        }

        public void Connect(int conversationId)
        {
            DisplayAlreadyReadMessages(conversationId);
            DisplayUnreadMessages(conversationId);
            DisplayUsers(conversationId);
        }

        public void ReadMessages(int conversationId)
        {
            UserToConversationLink currentUserLink = unitOfWork.Conversations
                .Get(GetId(), conversationId);
            long lastMessageId = unitOfWork.Conversations
                .GetLastMessageId(conversationId);
            currentUserLink.LastReadMessageId = lastMessageId;
            unitOfWork.Complete();
        }

        public void ChangeConversationParticipant(int conversationId,
            string userId)
        {
            if (unitOfWork.Users.IsPartisipant(conversationId, userId))
            {
                RemoveUser(conversationId, userId);
            }
            else
            {
                AddUser(conversationId, userId);
            }
        }

        private void RemoveUser(int conversationId, string userId)
        {
            unitOfWork.Links.RemoveLink(conversationId, userId);
            Clients.User(userId).disconnect();
            var convName = new UserController()
                .CreateConversationName(conversationId, GetId());
            Clients.All.updateUser(conversationId, false, userId, convName);
        }

        private void AddUser(int conversationId, string userId)
        {
            unitOfWork.Links.AddLink(userId, conversationId, 0);
            var convName = new UserController()
                .CreateConversationName(conversationId, GetId());
            Clients.All.updateUser(conversationId, true, userId, convName);
        }

        private Message AddMessage(string message, int conversationId)
        {
            return unitOfWork.Messages.AddMessage(GetId(),
                message, conversationId);
        }

        private void UpdateLastReadMessageId(
            int conversationId, long newMessageId)
        {
            var senderLink = unitOfWork.Conversations
                .Get(GetId(), conversationId);
            senderLink.LastReadMessageId = newMessageId;
            unitOfWork.Complete();
        }

        private void SendMessageToCurrentUser(Message newMessage)
        {
            Clients.Caller.addMessage(newMessage.ConversationId,
                true, false, GetName(), newMessage.Content,
                newMessage.DepartureTime.ToString());
        }

        private void SendMessageToOtherUsers(Message newMessage)
        {
            var conversationParticipants = unitOfWork.Conversations
                .GetConversationParticipants(newMessage.Conversation.Id);
            foreach (var conversationParticipant in conversationParticipants)
            {
                if (conversationParticipant.Id != GetId())
                {
                    Clients.User(conversationParticipant.Id).addMessage(
                        newMessage.ConversationId, false, true, GetName(),
                        newMessage.Content, newMessage.DepartureTime.ToString());
                }
            }
        }

        private void DisplayAlreadyReadMessages(int conversationId)
        {
            var readMessages = unitOfWork.Conversations
                .GetReadMessages(conversationId, GetId());
            foreach (Message message in readMessages)
            {
                Clients.Caller.addMessage(message.ConversationId,
                    true, false, message.SenderName, message.Content,
                    message.DepartureTime.ToString());
            }
        }

        private void DisplayUnreadMessages(int conversationId)
        {
            var unreadMessages = unitOfWork.Conversations
                .GetUnreadMessages(conversationId, GetId());
            foreach (Message message in unreadMessages)
            {
                Clients.Caller.addMessage(message.ConversationId, 
                    false, false, message.SenderName, message.Content,
                    message.DepartureTime.ToString());
            }
        }

        private void DisplayUsers(int conversationId)
        {
            var participants = DisplayParticipants(conversationId);
            DisplayOtherUsers(conversationId, participants);
        }

        private IEnumerable<ApplicationUser> DisplayParticipants(
            int conversationId)
        {
            DisplayCurrentUser();
            return DisplayOtherParticipants(conversationId);
        }

        private void DisplayCurrentUser()
        {
            Clients.Caller.addUser(GetId(), GetName(), true, true);
        }

        private IEnumerable<ApplicationUser> DisplayOtherParticipants(
            int conversationId)
        {
            var conversationParticipants = unitOfWork.Conversations
                .GetConversationParticipants(conversationId).ToList();
            foreach (var participant in conversationParticipants)
            {
                if (participant.Id != GetId())
                {
                    Clients.Caller.addUser(participant.Id,
                    AccountController.GetUsernameByUserId(participant.Id),
                    true, false);
                }
            }
            return conversationParticipants;
        }

        private void DisplayOtherUsers(int conversationId,
            IEnumerable<ApplicationUser> participants)
        {
            var otherUsers = unitOfWork.Users.GetAll().Except(participants);
            foreach (var user in otherUsers)
            {
                Clients.Caller.addUser(user.Id, AccountController
                    .GetUsernameByUserId(user.Id), false, false);
            }
        }

        private string GetId()
        {
            return Context.User.Identity.GetUserId();
        }

        private string GetName()
        {
            return AccountController.GetUsernameByUserId(GetId());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}