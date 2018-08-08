using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MOAS_LMS.Models;
using MOAS_LMS.Models.View;

namespace MOAS_LMS.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CourseModels
        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId();
            var currentUser = db.Users.Where(u => u.Id == currentUserId).SingleOrDefault();

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else if (User.IsInRole("Admin"))
            {
                return View(db.Courses.ToList());
            }
            else if (currentUser.Course != null)
            {
                return RedirectToAction("Overview", new {id = currentUser.Course.Id});
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: CourseModels/Overview/5
        [Authorize]
        public ActionResult Overview(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseModel courseModel = db.Courses.Find(id);

            var user = db.Users.First(u => u.UserName == User.Identity.Name);
            if (courseModel == null || (!User.IsInRole("Admin") && !courseModel.HasUserAccess(user)))
            {
                return HttpNotFound();
            }

            CourseViewModel courseViewModel = new CourseViewModel {
                Id = courseModel.Id,
                Title = courseModel.Title,
                Description = courseModel.Description,
                StartDate = courseModel.StartDate.ToString("MMMM dd, yyyy"),
                EndDate = courseModel.EndDate.ToString("MMMM dd, yyyy"),
                Students = courseModel.Students.ToList(),
                Modules = courseModel.Modules.ToList(),
                Documents = courseModel.Documents.ToList(),
            };
            return View(courseViewModel);
        }

        // GET: CourseModels/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: CourseModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,StartDate,EndDate")]
            CourseModel courseModel)
        {
            if (ModelState.IsValid)
            {
                db.Courses.Add(courseModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(courseModel);
        }

        // GET: CourseModels/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseModel courseModel = db.Courses.Find(id);
            if (courseModel == null)
            {
                return HttpNotFound();
            }
            return View(courseModel);
        }

        // POST: CourseModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,StartDate,EndDate")]
            CourseModel courseModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Overview", new { id = courseModel.Id});
            }
            return View(courseModel);
        }

        // GET: CourseModels/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseModel courseModel = db.Courses.Find(id);
            if (courseModel == null)
            {
                return HttpNotFound();
            }
            return View(courseModel);
        }

        // POST: CourseModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            CourseModel courseModel = db.Courses.Find(id);

            DeleteCourse(db, courseModel);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        static void DeleteCourse(ApplicationDbContext db, CourseModel course) {
            var mods = course.Modules.ToList();
            foreach (var mod in mods) {
                ModuleController.DeleteModule(db, mod);
            }

            var docs = course.Documents.ToList();
            foreach (var doc in docs) {
                DocumentController.DeleteDocument(db, doc);
            }

            var students = course.Students.ToList();
            foreach (var stud in students) {
                db.Users.Remove(stud);
            }

            db.Courses.Remove(course);
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
