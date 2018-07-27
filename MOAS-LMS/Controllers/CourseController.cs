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
                return RedirectToAction("Details", new {id = currentUser.Course.Id});
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: CourseModels/Details/5
        [Authorize]
        public ActionResult Details(int? id)
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

            CourseViewModel courseViewModel = new CourseViewModel
            {
                Id = courseModel.Id,
                Title = courseModel.Title,
                Description = courseModel.Description,
                StartDate = courseModel.StartDate.ToString("MMMM dd, yyyy"),
                EndDate = courseModel.EndDate.ToString("MMMM dd, yyyy"),
                Students = courseModel.Students.ToList(),
                Modules = courseModel.Modules.ToList()
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
                return RedirectToAction("Index");
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
            db.Courses.Remove(courseModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //[Authorize(Roles = "Admin")]
        //public ActionResult Teacher()
        //{
        //    List<TeacherModel> teacherModels = new List<TeacherModel>();

        //    foreach (var temp in db.Roles.FirstOrDefault(x => x.Name == "Admin").Users.ToList())
        //    {
        //        var temp2 = db.Users.FirstOrDefault(x => x.Id == temp.UserId);
        //        if (temp2 != null)
        //            teacherModels.Add(new TeacherModel()
        //            {
        //                Username = temp2.UserName
        //            });
        //    }
        //    return View(teacherModels); //db.Users.ToList());
        //}
    }
}
