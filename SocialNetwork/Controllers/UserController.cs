using Microsoft.AspNet.Identity;
using SocialNetwork.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using SocialNetwork.Repositories;

namespace SocialNetwork.Controllers
{
    public class UserController : Controller
    {
        readonly UnitOfWork unitOfWork 
            = new UnitOfWork(new ApplicationDbContext());

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated) {
                List<UserToConversationLink> userLinks
                    = unitOfWork.Users.Get(GetId()).Links;
                if(userLinks == null)
                {
                    return View(new List<KeyValuePair
                        <KeyValuePair<int, string>, int>>());
                }
                var preparedForViewList = prepareList(userLinks);
                return View(preparedForViewList);
            }
            string returnUrl = Url.Action("Index");
            return RedirectToAction("Login", "Account",
                new { ReturnUrl = returnUrl });
        }

        public ActionResult Conversation(int conversationId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!ContainsUser(conversationId))
                {
                    return RedirectToAction("Index", "User");
                }
                ViewBag.SenderName = GetName();
                ViewBag.RecipientsNames = CreateConversationName(
                    conversationId, GetId());
                ViewBag.ConversationId = conversationId;
                return View();
            }
            string returnUrl = Url.Action("Conversation");
            return RedirectToAction("Login", "Account",
                new { ReturnUrl = returnUrl });
        }

        public ActionResult Friends()
        {
            if (!User.Identity.IsAuthenticated)
            {
                string returnUrl = Url.Action("Friends");
                return RedirectToAction("Login", "Account",
                    new { ReturnUrl = returnUrl });
            }
            var allUsersExceptCurrentOne =
                unitOfWork.Users.GetUsersExceptOneUser(GetId());
            return View(allUsersExceptCurrentOne);
        }

        private List<KeyValuePair<KeyValuePair<int, string>, int>> prepareList(
            List<UserToConversationLink> links)
        {
            var userConversations 
                = unitOfWork.Conversations.GetConversations(links);
            // First int is ConversationId, string is ConversationName 
            // and second int is unreadMessagesNumber
            List<KeyValuePair<KeyValuePair<int, string>, int>> preparedList
                = new List<KeyValuePair<KeyValuePair<int, string>, int>>();
            foreach (Conversation conversation in userConversations)
            {
                 preparedList.Add(GetKeyValuePair(conversation));
            }
            return preparedList;
        }

        private KeyValuePair<KeyValuePair<int, string>, int> GetKeyValuePair(
            Conversation conversation)
        {
            var unreadMessagesNumber = FindUnreadMessagesNumber(conversation);
            if (conversation.Name != null)
            {
               return new KeyValuePair<KeyValuePair<int, string>, int>(
                    new KeyValuePair<int, string>(conversation.Id,
                    conversation.Name), unreadMessagesNumber);
            }
            string conversationName =
                CreateConversationName(conversation.Id, GetId());
            return new KeyValuePair<KeyValuePair<int, string>, int>(
                new KeyValuePair<int, string>(conversation.Id,
                conversationName), unreadMessagesNumber);
        }

        private int FindUnreadMessagesNumber(Conversation conversation)
        {
            var lastReadMessageId
                = unitOfWork.Conversations.GetLastReadMessageId(
                    conversation.Id, GetId());
            return unitOfWork.Conversations
                .GetUnreadMessagesNumber(conversation.Id, lastReadMessageId);
        }

        [HttpPost]
        public void ChangeConversationName(int conversationId, string name)
        {
            unitOfWork.Conversations.ChangeConversationName(conversationId, name);
        }

        public string CreateConversationName(int conversationId, string userId)
        {
            string conversationName = System.String.Empty;
            var conversationParticipants = unitOfWork.Conversations
                .GetConversationParticipants(conversationId);
            foreach (var conversationParticipant in conversationParticipants)
            {
                conversationName += GetConversationNamePart(
                    conversationName, conversationParticipant, userId);
            }
            return conversationName;
        }

        private string GetConversationNamePart(string conversationName,
            ApplicationUser conversationParticipant, string userId)
        {
            if (conversationParticipant.Id != userId)
            {
                if (conversationName.Equals(System.String.Empty))
                {
                    return AccountController
                        .GetUsernameByUserId(conversationParticipant.Id);
                }
                return ", " + AccountController
                    .GetUsernameByUserId(conversationParticipant.Id);
            }
            return System.String.Empty;
        }

        private bool ContainsUser(int conversationId)
        {
            var conversationParticipants = unitOfWork.Conversations
                .GetConversationParticipants(conversationId);
            if (unitOfWork.Users.ContainsUser(
                conversationParticipants, GetId()))
            {
                return true;
            }
            return false;
        }

        private string GetId()
        {
            return User.Identity.GetUserId();
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