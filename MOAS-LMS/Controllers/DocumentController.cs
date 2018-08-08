using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MOAS_LMS.Models;

namespace MOAS_LMS.Controllers
{
    [Authorize]
    public class DocumentController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DocumentModels
        public ActionResult Index()
        {
            return View(db.Documents.ToList());
        }

        // GET: DocumentModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentModel documentModel = db.Documents.Find(id);
            if (documentModel == null)
            {
                return HttpNotFound();
            }
            return View(documentModel);
        }

        // GET: DocumentModels/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: DocumentModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,FileName,Path,TimeStamp,Feedback,IsHandIn")] DocumentModel documentModel)
        {
            if (ModelState.IsValid)
            {
                db.Documents.Add(documentModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(documentModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Upload(HttpPostedFileBase document) {
            if (ModelState.IsValid && document != null) {
                var filename = Path.GetFileName(document.FileName);
                var path = Path.Combine(Server.MapPath($"~/App_Data/Uploads/test/"), filename);
                var doc = new DocumentModel {
                    FileName = filename,
                    Path = path,
                    TimeStamp = DateTime.Now,
                    Feedback = null,
                    IsHandIn = false,
                };
                db.Documents.Add(doc);
                db.SaveChanges();
                document.SaveAs(path);
            }
            return RedirectToAction("Index", "Course", null);
        }

        [HttpPost]
        public ActionResult UploadHandIn(IEnumerable<HttpPostedFileBase> file, int? id) {
            if (!ModelState.IsValid || file == null || id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            var activity = db.Activities.FirstOrDefault(a => a.Id == id);
            if (activity == null || !activity.ActivityType.AllowUploads) {
                return HttpNotFound("Could not find assignment");
            }
            var uploader = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            if (!activity.HasUserAccess(uploader)) {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            foreach (var document in file) {
                var filename = Path.GetFileName(document.FileName);
                var guid = Guid.NewGuid().ToString();
                var folder = Server.MapPath($"~/App_Data/Uploads/HandIns/{uploader.Id}/{activity.Id}/");
                var path = Path.Combine(folder, guid);
                Directory.CreateDirectory(folder);
                document.SaveAs(path);
                var doc = new DocumentModel {
                    FileName = filename,
                    Path = path,
                    Uploader = uploader,
                    TimeStamp = DateTime.Now,
                    Activity = activity,
                    Feedback = null,
                    IsHandIn = true,
                };
                db.Documents.Add(doc);
            }
            db.SaveChanges();
            return RedirectToAction("Overview", "Course", new { id = uploader.Course.Id });
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult UploadToCourse(IEnumerable<HttpPostedFileBase> file, int? id) {
            if (file == null || id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var course = db.Courses.FirstOrDefault(a => a.Id == id);
            if (course == null) {
                return HttpNotFound("Could not find course");
            }
            var uploader = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            if (!User.IsInRole("Admin")) {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            foreach (var document in file) {
                var filename = Path.GetFileName(document.FileName);
                var guid = Guid.NewGuid().ToString();
                var folder = Server.MapPath($"~/App_Data/Uploads/Course/{course.Id}/");
                var path = Path.Combine(folder, guid);
                Directory.CreateDirectory(folder);
                document.SaveAs(path);
                var doc = new DocumentModel {
                    FileName = filename,
                    Path = path,
                    Uploader = uploader,
                    TimeStamp = DateTime.Now,
                    Course = course,
                    Feedback = null,
                    IsHandIn = false,
                };
                db.Documents.Add(doc);
            }
            db.SaveChanges();
            return RedirectToAction("Overview", "Course", new { id });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult UploadToModule(IEnumerable<HttpPostedFileBase> file, int? id) {
            if (file == null || id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var module = db.Modules.FirstOrDefault(a => a.Id == id);
            if (module == null) {
                return HttpNotFound("Could not find module");
            }
            var uploader = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            if (!User.IsInRole("Admin")) {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            foreach (var document in file) {
                var filename = Path.GetFileName(document.FileName);
                var guid = Guid.NewGuid().ToString();
                var folder = Server.MapPath($"~/App_Data/Uploads/Module/{module.Id}/");
                var path = Path.Combine(folder, guid);
                Directory.CreateDirectory(folder);
                document.SaveAs(path);
                var doc = new DocumentModel {
                    FileName = filename,
                    Path = path,
                    Uploader = uploader,
                    TimeStamp = DateTime.Now,
                    Module = module,
                    Feedback = null,
                    IsHandIn = false,
                };
                db.Documents.Add(doc);
            }
            db.SaveChanges();
            return RedirectToAction("Overview", "Course", new { id = module?.Course?.Id });
        }

        [Authorize(Roles = "Admin")]
        public ActionResult UploadToActivity(IEnumerable<HttpPostedFileBase> file, int? id) {
            if (file == null || id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var activity = db.Activities.FirstOrDefault(a => a.Id == id);
            if (activity == null) {
                return HttpNotFound("Could not find activity");
            }
            var uploader = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            if (!User.IsInRole("Admin")) {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }
            foreach (var document in file) {
                var filename = Path.GetFileName(document.FileName);
                var guid = Guid.NewGuid().ToString();
                var folder = Server.MapPath($"~/App_Data/Uploads/Activity/{activity.Id}/");
                var path = Path.Combine(folder, guid);
                Directory.CreateDirectory(folder);
                document.SaveAs(path);
                var doc = new DocumentModel {
                    FileName = filename,
                    Path = path,
                    Uploader = uploader,
                    TimeStamp = DateTime.Now,
                    Activity = activity,
                    Feedback = null,
                    IsHandIn = false,
                };
                db.Documents.Add(doc);
            }
            db.SaveChanges();
            return RedirectToAction("Overview", "Course", new { id = activity?.Module?.Course?.Id });
        }

        public ActionResult Get(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var doc = db.Documents.FirstOrDefault(d => d.Id == id);
            if (doc == null) {
                return HttpNotFound();
            }

            if (!User.IsInRole("Admin")) {
                var downloader = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
                if (!doc.HasUserAccess(downloader)) {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }
            }

            try {
                var file = System.IO.File.Open(doc.Path, FileMode.Open);
                return File(file, MimeMapping.GetMimeMapping(doc.FileName), doc.FileName);
            }
            catch (Exception e) {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Could not fetch file: " + e.Message);
            }
            
        }

        // GET: DocumentModels/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentModel documentModel = db.Documents.Find(id);
            if (documentModel == null)
            {
                return HttpNotFound();
            }
            return View(documentModel);
        }

        // POST: DocumentModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,FileName,Path,TimeStamp,Feedback,IsHandIn")] DocumentModel documentModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(documentModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(documentModel);
        }

        // GET: DocumentModels/Edit/5
        [Authorize]
        public ActionResult Feedback(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentModel documentModel = db.Documents.Find(id);
            if (documentModel == null) {
                return HttpNotFound();
            }
            return View(documentModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Feedback([Bind(Include = "Id,Feedback")] DocumentModel documentModel) {
            if (ModelState.IsValid) {
                var doc = db.Documents.FirstOrDefault(d => d.Id == documentModel.Id);
                doc.Feedback = documentModel.Feedback;
                db.SaveChanges();
                return RedirectToAction("Overview", "Course", new { id = doc.Activity?.Module?.Course?.Id });
            }
            return View(documentModel);
        }

        // GET: DocumentModels/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DocumentModel documentModel = db.Documents.Find(id);
            if (documentModel == null)
            {
                return HttpNotFound();
            }
            return View(documentModel);
        }

        // POST: DocumentModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            DocumentModel documentModel = db.Documents.Find(id);
            var path = documentModel.Path;
            var act = documentModel.Activity;
            var mod = documentModel.Module;
            var cou = documentModel.Course;

            DeleteDocument(db, documentModel);

            db.SaveChanges();

            if (cou != null) {
                return RedirectToAction("Overview", "Course", new { id = cou.Id });
            }
            else if (mod?.Course != null) {
                return RedirectToAction("Overview", "Course", new { id = mod.Course.Id });
            }
            else if (act?.Module?.Course != null) {
                return RedirectToAction("Overview", "Course", new { id = act.Module.Course.Id });
            }
            return RedirectToAction("Index");
        }

        public static void DeleteDocument(ApplicationDbContext db, DocumentModel doc) {
            try {
                System.IO.File.Delete(doc.Path);

            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
            db.Documents.Remove(doc);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
