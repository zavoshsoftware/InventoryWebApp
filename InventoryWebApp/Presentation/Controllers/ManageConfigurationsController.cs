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
    public class ManageConfigurationsController : Infrastructure.BaseControllerWithUnitOfWork
    {

        // GET: ManageConfigurations
        public ActionResult Index()
        {
            return View(UnitOfWork.ManageConfigurationRepository.Get().OrderByDescending(a=>a.CreationDate).ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ManageConfiguration manageConfiguration)
        {
            if (ModelState.IsValid)
            {
				manageConfiguration.IsDeleted=false;
				manageConfiguration.CreationDate= DateTime.Now; 
                manageConfiguration.Id = Guid.NewGuid();
                UnitOfWork.ManageConfigurationRepository.Insert(manageConfiguration);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(manageConfiguration);
        }

        // GET: ManageConfigurations/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManageConfiguration manageConfiguration = UnitOfWork.ManageConfigurationRepository.GetById(id.Value);
            if (manageConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(manageConfiguration);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ManageConfiguration manageConfiguration)
        {
            if (ModelState.IsValid)
            {
				manageConfiguration.IsDeleted=false;
                UnitOfWork.ManageConfigurationRepository.Update(manageConfiguration);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(manageConfiguration);
        }
        
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ManageConfiguration manageConfiguration = UnitOfWork.ManageConfigurationRepository.GetById(id.Value);
            if (manageConfiguration == null)
            {
                return HttpNotFound();
            }
            return View(manageConfiguration);
        }

        // POST: ManageConfigurations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UnitOfWork.ManageConfigurationRepository.DeleteById(id);
            UnitOfWork.Save();
            return RedirectToAction("Index");
        }
        
    }
}
