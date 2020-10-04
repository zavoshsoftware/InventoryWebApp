using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;

namespace Presentation.Controllers
{
    public class CutTypesController : Infrastructure.BaseControllerWithUnitOfWork
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: CutTypes
        public ActionResult Index()
        {
            return View(UnitOfWork.CutTypeRepository.Get().OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: CutTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CutType cutType = UnitOfWork.CutTypeRepository.GetById(id.Value);
            if (cutType == null)
            {
                return HttpNotFound();
            }
            return View(cutType);
        }

        // GET: CutTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CutTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CutType cutType)
        {
            if (ModelState.IsValid)
            {
				cutType.IsDeleted=false;
				cutType.CreationDate= DateTime.Now; 
                cutType.Id = Guid.NewGuid();
                UnitOfWork.CutTypeRepository.Insert(cutType);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(cutType);
        }

        // GET: CutTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CutType cutType = UnitOfWork.CutTypeRepository.GetById(id.Value);
            if (cutType == null)
            {
                return HttpNotFound();
            }
            return View(cutType);
        }

        // POST: CutTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CutType cutType)
        {
            if (ModelState.IsValid)
            {
				cutType.IsDeleted=false;
                UnitOfWork.CutTypeRepository.Update(cutType);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(cutType);
        }

        // GET: CutTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CutType cutType = UnitOfWork.CutTypeRepository.GetById(id.Value);
            if (cutType == null)
            {
                return HttpNotFound();
            }
            return View(cutType);
        }

        // POST: CutTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UnitOfWork.CutTypeRepository.DeleteById(id);
            UnitOfWork.Save();
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
