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
    public class ExitsController : Infrastructure.BaseControllerWithUnitOfWork
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: Exits
        public ActionResult Index()
        {
            var exits = UnitOfWork.ExitRepository.Get(current => current.IsOpen == true).OrderByDescending(current => current.CreationDate);
            return View(exits.ToList());
        }

        // GET: Exits/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exit exit = UnitOfWork.ExitRepository.GetById(id.Value);
            if (exit == null)
            {
                return HttpNotFound();
            }
            return View(exit);
        }

        // GET: Exits/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName");
            return View();
        }

        // POST: Exits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Exit exit)
        {
            if (ModelState.IsValid)
            {
				exit.IsDeleted=false;
				exit.CreationDate= DateTime.Now; 
                exit.Id = Guid.NewGuid();
                UnitOfWork.ExitRepository.Insert(exit);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName", exit.CustomerId);
            return View(exit);
        }

        // GET: Exits/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exit exit = UnitOfWork.ExitRepository.GetById(id.Value);
            if (exit == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName", exit.CustomerId);
            return View(exit);
        }

        // POST: Exits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Exit exit)
        {
            if (ModelState.IsValid)
            {
				exit.IsDeleted=false;
                UnitOfWork.ExitRepository.Update(exit);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName", exit.CustomerId);
            return View(exit);
        }

        // GET: Exits/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exit exit = UnitOfWork.ExitRepository.GetById(id.Value);
            if (exit == null)
            {
                return HttpNotFound();
            }
            return View(exit);
        }

        // POST: Exits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UnitOfWork.ExitRepository.DeleteById(id);
            UnitOfWork.Save();
 
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
