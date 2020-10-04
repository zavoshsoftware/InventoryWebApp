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
    public class CutDetailTypesController : Infrastructure.BaseControllerWithUnitOfWork
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: CutDetailTypes
        public ActionResult Index()
        {
            return View(UnitOfWork.CutDetailTypeRepository.Get().OrderByDescending(a=>a.CreationDate).ToList());
        }

        // GET: CutDetailTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CutDetailType cutDetailType = UnitOfWork.CutDetailTypeRepository.GetById(id.Value);
            if (cutDetailType == null)
            {
                return HttpNotFound();
            }
            return View(cutDetailType);
        }

        // GET: CutDetailTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CutDetailTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CutDetailType cutDetailType)
        {
            if (ModelState.IsValid)
            {
				cutDetailType.IsDeleted=false;
				cutDetailType.CreationDate= DateTime.Now; 
                cutDetailType.Id = Guid.NewGuid();
                UnitOfWork.CutDetailTypeRepository.Insert(cutDetailType);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(cutDetailType);
        }

        // GET: CutDetailTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CutDetailType cutDetailType = UnitOfWork.CutDetailTypeRepository.GetById(id.Value);
            if (cutDetailType == null)
            {
                return HttpNotFound();
            }
            return View(cutDetailType);
        }

        // POST: CutDetailTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CutDetailType cutDetailType)
        {
            if (ModelState.IsValid)
            {
				cutDetailType.IsDeleted=false;
                UnitOfWork.CutDetailTypeRepository.Update(cutDetailType);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(cutDetailType);
        }

        // GET: CutDetailTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CutDetailType cutDetailType = UnitOfWork.CutDetailTypeRepository.GetById(id.Value);
            if (cutDetailType == null)
            {
                return HttpNotFound();
            }
            return View(cutDetailType);
        }

        // POST: CutDetailTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UnitOfWork.CutDetailTypeRepository.DeleteById(id);
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
