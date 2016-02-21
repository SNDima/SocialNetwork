using System;
using Microsoft.AspNet.Identity;
using SocialNetwork.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SocialNetwork.Repositories;

namespace SocialNetwork.Controllers
{
    public class ConversationController : Controller
    {
        readonly UnitOfWork unitOfWork 
            = new UnitOfWork(new ApplicationDbContext());

        [HttpGet]
        public ActionResult Create(string recipientId)
        {
            var commonConversation = GetCommonConversation(recipientId);
            if (commonConversation != null)
            {
                return RedirectToAction("Conversation", "User",
                        new { conversationId = commonConversation.Id });
            }
            ViewBag.SenderName = GetName();
            ViewBag.RecipientName = AccountController
                .GetUsernameByUserId(recipientId);
            ViewBag.RecipientId = recipientId;
            return View();
        }

        [HttpPost]
        public ActionResult Create(string recipientsIds, Message newMessage)
        {
            IEnumerable<string> ids = recipientsIds.Split(',');
            var newConversation = InsertNewRecords(ids, newMessage);
            return RedirectToAction("Conversation", "User",
                new { conversationId = newConversation.Id });
        }

        public JsonResult GetNotIncludedFriends(
            IEnumerable<string> participantsIds)
        {
            var friends = unitOfWork.Users
                .GetAllUsersExceptSomeUsers(participantsIds)
                .Where(user => user.Id != GetId())
                .Select(user => new {
                    Id = user.Id,
                    Name = user.FirstName + " " + user.LastName
                });
            return Json(friends, JsonRequestBehavior.AllowGet);
        }

        private Conversation GetCommonConversation(string recipientId)
        {
            var conversationParticipants = new List<ApplicationUser>();
            conversationParticipants.Add(unitOfWork.Users.Get(GetId()));
            conversationParticipants.Add(unitOfWork.Users.Get(recipientId));
            return unitOfWork.Conversations
                .GetTwoUserConversation(conversationParticipants);
        }

        private Conversation InsertNewRecords(
            IEnumerable<string> recipientsIds, Message newMessage)
        {
            Conversation newConversation
                = unitOfWork.Conversations.AddConversation();
            newMessage = unitOfWork.Messages.AddMessage(
                GetId(), newMessage, newConversation.Id);
            unitOfWork.Links.AddLink(unitOfWork.Users.Get(
                GetId()), newConversation, newMessage);
            foreach (var id in recipientsIds)
            {
                unitOfWork.Links.AddLink(unitOfWork.Users.Get(
                id), newConversation, newMessage);
            }
            return newConversation;
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