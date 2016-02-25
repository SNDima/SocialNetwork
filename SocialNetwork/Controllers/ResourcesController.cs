using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
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
            var filesNames = unitOfWork.Resources.Get(id)
                .Files.Select(file => file.Path.Split('\\').Last()).ToList();
            ViewBag.Names = String.Join("|", filesNames);
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
                CreateFiles(model.FilesNames, resource.Id);
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
            var filesNames = unitOfWork.Resources.Get(id)
                .Files.Select(file => file.Path.Split('\\').Last()).ToList();
            ViewBag.Names = String.Join("|", filesNames);
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
                CreateFiles(model.FilesNames, resource.Id);
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
            using (TransactionScope scope = new TransactionScope())
            {
                Resource resource = unitOfWork.Resources.Get(id);
                RemoveFiles(resource.Files);
                unitOfWork.Resources.Remove(resource);
                unitOfWork.Complete();
                scope.Complete();
            }
            return RedirectToAction("Index");
            
        }

        [HttpPost]
        public void AddFile(string fileName)
        {
            File file = new File {Path = Server.MapPath("~/Files/" + fileName)};
            unitOfWork.Files.Add(file);
            unitOfWork.Complete();
        }

        [HttpPost]
        public void DeleteFile(string fileName)
        {
            var filePath = Server.MapPath("~/Files/" + fileName);
            var file = unitOfWork.Files.SingleOrDefault(
                f => f.Path == filePath);
            if (file != null)
            {
                unitOfWork.Files.Remove(file);
                unitOfWork.Complete();
            }
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
                URLs = GetResourceURLs(resource),
                FilesNames = GetFilesNames(resource)
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

        private List<string> GetFilesNames(Resource resource)
        {
            List<string> names = new List<string>();
            foreach (File file in resource.Files)
            {
                names.Add(file.Path.Split('\\').Last());
            }
            return names;
        }

        private void CreateFiles(List<string> names, long resourceId)
        {
            if (names != null)
            {
                foreach (var name in names)
                {
                    var filePath = Server.MapPath("~/Files/" + name);
                    var file = unitOfWork.Files.SingleOrDefault(
                        f => f.Path == filePath);
                    if (file != null && file.ResourceId == null)
                    {
                        file.ResourceId = resourceId;
                        unitOfWork.Complete();
                    }
                }
            }
            else
            {
                DeleteFiles(resourceId);
            }
            ClearFiles();
        }

        private void ClearFiles()
        {
            var badFiles = unitOfWork.Files.GetAll()
                .Where(file => file.ResourceId == null).ToList();
            RemoveFiles(badFiles);
        }

        private void RemoveFiles(List<File> files)
        {
            if (files != null)
            {
                var removingFiles = new List<File>();
                foreach (var file in files)
                {
                    string fileName = file.Path.Split('\\').Last();
                    if (System.IO.File.Exists(file.Path))
                    {
                        System.IO.File.Delete(file.Path);
                        System.IO.File.Delete(Server.MapPath(
                            "~/Files/_thumbs/" + fileName + ".png"));
                    }
                    removingFiles.Add(file);
                }
                unitOfWork.Files.RemoveRange(removingFiles);
                unitOfWork.Complete();
            }
        }

        private void DeleteFiles(long resourceId)
        {
            var resource = unitOfWork.Resources.Get(resourceId);
            RemoveFiles(resource.Files);
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
