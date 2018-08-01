using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MOAS_LMS.Models;
using MOAS_LMS.Models.View;

namespace MOAS_LMS.Controllers
{
    [Authorize]
    public class ActivityController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Activity
        public ActionResult Index()
        {
            return View(db.Activities.ToList());
        }

        // GET: Activity/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityModel activityModel = db.Activities.Find(id);
            var user = db.Users.FirstOrDefault(u => u.Email == User.Identity.Name);
            if (activityModel == null || !activityModel.HasUserAccess(user))
            {
                return HttpNotFound();
            }

            return View(activityModel);
        }

        // GET: Activity/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create(int? id)
        {
            ViewBag.CourseId = db.Modules.FirstOrDefault(m => m.Id == id)?.Course.Id;
            ViewBag.ActivityTypes = db.ActivityTypes.ToList();
            return View();
        }

        // POST: Activity/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Id,ActivityTypeId,Name,StartDate,EndDate,Description")] CreateActivityViewModel activityModel, int? id)
        {
            if (ModelState.IsValid)
            {
                var activity = new ActivityModel
                {
                    Name = activityModel.Name,
                    Description = activityModel.Description,
                    Module = db.Modules.FirstOrDefault(m => m.Id == id),
                    ActivityType = db.ActivityTypes.FirstOrDefault(m => m.Id == activityModel.ActivityTypeId),
                    StartDate = activityModel.StartDate,
                    EndDate = activityModel.EndDate,
                };
                    
                db.Activities.Add(activity);
                db.SaveChanges();
                return RedirectToAction("Details", "Course", new { id = activity.Module.Course.Id });
            }
            ViewBag.CourseId = db.Modules.FirstOrDefault(m => m.Id == id)?.Course.Id;
            ViewBag.ActivityTypes = db.ActivityTypes.ToList();
            return View(activityModel); 
        }

        // GET: Activity/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityModel activityModel = db.Activities.Find(id);
            if (activityModel == null)
            {
                return HttpNotFound();
            }
            return View(activityModel);
        }

        // POST: Activity/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,ActivityType,Name,StartDate,EndDate,Description")] ActivityModel activityModel)
        {
            if (ModelState.IsValid)
            {
                var activity = db.Activities.SingleOrDefault(a => a.Id == activityModel.Id);
                activity.ActivityType = activityModel.ActivityType;
                activity.Name = activityModel.Name;
                activity.StartDate = activityModel.StartDate;
                activity.EndDate = activityModel.EndDate;
                activity.Description = activityModel.Description;
                db.SaveChanges();
                return RedirectToAction("Details", "Course", new { id = activity.Module.Course.Id });
            }
            return View(activityModel);
        }

        // GET: Activity/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ActivityModel activityModel = db.Activities.Find(id);
            if (activityModel == null)
            {
                return HttpNotFound();
            }
            return View(activityModel);
        }

        // POST: Activity/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            ActivityModel activityModel = db.Activities.Find(id);
            db.Activities.Remove(activityModel);
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
    }
}
