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
    public class ModuleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Module
        public ActionResult Index()
        {
            return View(db.Modules.ToList());
        }

        // GET: Module/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModuleModel moduleModel = db.Modules.Find(id);
            if (moduleModel == null)
            {
                return HttpNotFound();
            }
            return View(moduleModel);
        }

        // GET: Module/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Module/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description,StartDate,EndDate")] CreateModuleViewModel moduleModel, int? id)
        {
            if (ModelState.IsValid && id != null)
            {
                var course = db.Courses.FirstOrDefault(c => c.Id == id);
                if (course == null) {
                    return HttpNotFound();
                }

                var module = new ModuleModel {
                    Course = course,
                    Name = moduleModel.Name,
                    Description = moduleModel.Description,
                    StartDate = moduleModel.StartDate,
                    EndDate = moduleModel.EndDate,
                };

                db.Modules.Add(module);
                db.SaveChanges();
                return RedirectToAction("Details", "Course", new { id });
            }

            return View(moduleModel);
        }

        // GET: Module/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModuleModel moduleModel = db.Modules.Find(id);
            if (moduleModel == null)
            {
                return HttpNotFound();
            }
            return View(moduleModel);
        }

        // POST: Module/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate")] ModuleModel moduleModel)
        {
            if (ModelState.IsValid)
            {
                var module = db.Modules.SingleOrDefault(m => m.Id == moduleModel.Id);
                module.Name = moduleModel.Name;
                module.Description = moduleModel.Description;
                module.StartDate = moduleModel.StartDate;
                module.EndDate = moduleModel.EndDate;
                db.SaveChanges();

                return RedirectToAction("Details", "Course", new { id = module.Course.Id });
            }
            return View(moduleModel);
        }

        // GET: Module/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ModuleModel moduleModel = db.Modules.Find(id);
            if (moduleModel == null)
            {
                return HttpNotFound();
            }
            return View(moduleModel);
        }

        // POST: Module/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ModuleModel moduleModel = db.Modules.Find(id);
            db.Modules.Remove(moduleModel);
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
