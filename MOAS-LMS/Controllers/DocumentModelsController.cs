using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MOAS_LMS.Models;

namespace MOAS_LMS.Controllers
{
    public class DocumentModelsController : Controller
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
        public ActionResult Create()
        {
            return View();
        }

        // POST: DocumentModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: DocumentModels/Edit/5
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

        // GET: DocumentModels/Delete/5
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
        public ActionResult DeleteConfirmed(int id)
        {
            DocumentModel documentModel = db.Documents.Find(id);
            db.Documents.Remove(documentModel);
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
