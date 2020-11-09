using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Models;

namespace Presentation.Controllers
{
    public class ExitDriversController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public ActionResult Index()
        {
            return View(UnitOfWork.ExitDriverRepository.Get(a=>a.IsDeleted==false).OrderByDescending(a=>a.CreationDate).ToList());
        }
         
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ExitDriver exitDriver)
        {
            if (ModelState.IsValid)
            {
				exitDriver.IsDeleted=false;
				exitDriver.CreationDate= DateTime.Now; 
                exitDriver.Id = Guid.NewGuid();
                UnitOfWork.ExitDriverRepository.Insert(exitDriver);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(exitDriver);
        }
         
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExitDriver exitDriver = UnitOfWork.ExitDriverRepository.GetById(id.Value);
            if (exitDriver == null)
            {
                return HttpNotFound();
            }
            return View(exitDriver);
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ExitDriver exitDriver)
        {
            if (ModelState.IsValid)
            {
				exitDriver.IsDeleted=false;
                UnitOfWork.ExitDriverRepository.Update(exitDriver);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(exitDriver);
        }
         
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExitDriver exitDriver = UnitOfWork.ExitDriverRepository.GetById(id.Value);
            if (exitDriver == null)
            {
                return HttpNotFound();
            }
            return View(exitDriver);
        }
         
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ExitDriver exitDriver = UnitOfWork.ExitDriverRepository.GetById(id);
            UnitOfWork.ExitDriverRepository.Delete(exitDriver);
            UnitOfWork.Save();
            return RedirectToAction("Index");
        } 
    }
}

