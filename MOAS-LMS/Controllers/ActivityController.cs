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
    [Authorize(Roles = "Admin")]
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
            if (activityModel == null)
            {
                return HttpNotFound();
            }
            return View(activityModel);
        }

        // GET: Activity/Create
        public ActionResult Create()
        {
            ViewBag.ActivityTypes = db.ActivityTypes.ToList();
            return View();
        }

        // POST: Activity/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ActivityType,Name,StartDate,EndDate,Description")] CreateActivityViewModel activityModel)
        {
            if (ModelState.IsValid)
            {
                var activity = new ActivityModel
                {
                    Name = activityModel.Name,
                    Description = activityModel.Description,
                    Id = activityModel.ModuleId,
                    StartDate = activityModel.StartDate,
                    EndDate = activityModel.EndDate,
                };
                    
                db.Activities.Add(activity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(activityModel);
        }

        // GET: Activity/Edit/5
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
        public ActionResult Edit([Bind(Include = "Id,ActivityType,Name,StartDate,EndDate,Description")] ActivityModel activityModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activityModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(activityModel);
        }

        // GET: Activity/Delete/5
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
