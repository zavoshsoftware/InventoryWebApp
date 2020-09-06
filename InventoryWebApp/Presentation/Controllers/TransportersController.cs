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
    public class TransportersController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public ActionResult Index()
        {
            return View(UnitOfWork.TransporterRepository.Get().OrderByDescending(a=>a.CreationDate).ToList());
        }
 

        public ActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Transporter transporter)
        {
            if (ModelState.IsValid)
            {
				transporter.IsDeleted=false;
				transporter.CreationDate= DateTime.Now; 
                transporter.Id = Guid.NewGuid();

                UnitOfWork.TransporterRepository.Insert(transporter);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(transporter);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transporter transporter = UnitOfWork.TransporterRepository.GetById(id.Value);
            if (transporter == null)
            {
                return HttpNotFound();
            }
            return View(transporter);
        }

 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Transporter transporter)
        {
            if (ModelState.IsValid)
            {
                transporter.IsDeleted = false;
                UnitOfWork.TransporterRepository.Update(transporter);
                UnitOfWork.Save();
        
                return RedirectToAction("Index");
            }
            return View(transporter);
        }
         
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transporter transporter = UnitOfWork.TransporterRepository.GetById(id.Value);
            if (transporter == null)
            {
                return HttpNotFound();
            }
            return View(transporter);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Transporter transporter = UnitOfWork.TransporterRepository.GetById(id);

            UnitOfWork.TransporterRepository.DeleteById(id);
           UnitOfWork.Save();
 
            return RedirectToAction("Index");
        }
    }
}
