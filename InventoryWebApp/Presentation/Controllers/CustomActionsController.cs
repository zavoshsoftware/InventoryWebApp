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
    public class CustomActionsController : Infrastructure.BaseControllerWithUnitOfWork
    {

        // GET: CustomActions
        public ActionResult Index()
        {
            return View(UnitOfWork.CustomActionRepository.Get().OrderByDescending(a=>a.CreationDate).ToList());
        }
#region Crud
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomAction customAction)
        {
            if (ModelState.IsValid)
            {
				customAction.IsDeleted=false;
				customAction.CreationDate= DateTime.Now; 
                customAction.Id = Guid.NewGuid();
                UnitOfWork.CustomActionRepository.Insert(customAction);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(customAction);
        }

        // GET: CustomActions/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomAction customAction = UnitOfWork.CustomActionRepository.GetById(id.Value);
            if (customAction == null)
            {
                return HttpNotFound();
            }
            return View(customAction);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomAction customAction)
        {
            if (ModelState.IsValid)
            {
				customAction.IsDeleted=false;
                UnitOfWork.CustomActionRepository.Update(customAction);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(customAction);
        }
        
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomAction customAction = UnitOfWork.CustomActionRepository.GetById(id.Value);
            if (customAction == null)
            {
                return HttpNotFound();
            }
            return View(customAction);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UnitOfWork.CustomActionRepository.DeleteById(id);
            return RedirectToAction("Index");
        }

#endregion
    }
}
