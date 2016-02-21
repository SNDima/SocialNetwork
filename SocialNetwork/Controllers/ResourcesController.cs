using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using SocialNetwork.Models;
using SocialNetwork.Repositories;
using SocialNetwork.ViewModels.Resource;

namespace SocialNetwork.Controllers
{
    public class ResourcesController : Controller
    {
        readonly UnitOfWork unitOfWork
            = new UnitOfWork(new ApplicationDbContext());

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                List<IndexViewModel> models = new List<IndexViewModel>();
                var resources = unitOfWork.Resources.GetAll()
                    .OrderByDescending(resource => resource.PostingTime);
                foreach (var resource in resources)
                {
                    models.Add(CreateIndexViewModel(resource));
                }
                return View(models);
            }
            string returnUrl = Url.Action("Index");
            return RedirectToAction("Login", "Account",
                new { ReturnUrl = returnUrl });
        }

        public ActionResult Details(long id = 0)
        {
            Resource resource = unitOfWork.Resources.Get(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            unitOfWork.Resources.IncreaseViewsNumber(resource);
            var model = CreateIndexViewModel(resource);
            return View(model);
        }

        public ActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }
            string returnUrl = Url.Action("Create");
            return RedirectToAction("Login", "Account",
                new { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Resource resource = new Resource()
                {
                    Name = model.Name,
                    Description = model.Description,
                    OwnerId = GetId(),
                    PostingTime = DateTime.Now,
                    ViewsNumber = 0
                };
                unitOfWork.Resources.Add(resource);
                unitOfWork.Complete();
                unitOfWork.URLs.CreateURLs(model.URLs, resource.Id);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public ActionResult Edit(long id = 0)
        {
            Resource resource = unitOfWork.Resources.Get(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            EditViewModel model = CreateIndexViewModel(resource);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditViewModel model, long resourceId)
        {
            if (ModelState.IsValid)
            {
                var resource = unitOfWork.Resources.Get(resourceId);
                unitOfWork.URLs.RemoveRange(resource.URLs);
                resource.Name = model.Name;
                resource.Description = model.Description;
                resource.PostingTime = DateTime.Now;
                unitOfWork.Complete();
                unitOfWork.URLs.CreateURLs(model.URLs, resource.Id);
                return RedirectToAction("Index");
            }
            model.Id = resourceId;
            return View(model);
        }

        public ActionResult Delete(long id = 0)
        {
            Resource resource = unitOfWork.Resources.Get(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            var model = CreateIndexViewModel(resource);
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Resource resource = unitOfWork.Resources.Get(id);
            unitOfWork.Resources.Remove(resource);
            unitOfWork.Complete();
            return RedirectToAction("Index");
        }

        private IndexViewModel CreateIndexViewModel(Resource resource)
        {
            return new IndexViewModel()
            {
                Id = resource.Id,
                Name = resource.Name,
                UserName = AccountController.GetUsernameByUserId(
                    resource.OwnerId),
                PostingTime = resource.PostingTime.ToString(),
                ViewsNumber = resource.ViewsNumber,
                Description = resource.Description,
                URLs = GetResourceURLs(resource)
            };
        }

        private List<string> GetResourceURLs(Resource resource)
        {
            List<string> URLs = new List<string>();
            foreach (URL url in resource.URLs)
            {
                URLs.Add(url.Content);
            }
            return URLs;
        }

        private string GetId()
        {
            return User.Identity.GetUserId();
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
