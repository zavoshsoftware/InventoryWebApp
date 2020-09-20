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
    public class ProductGroupUnitsController : Infrastructure.BaseControllerWithUnitOfWork
    {

        public ActionResult Index()
        {
            return View(UnitOfWork.ProductGroupUnitRepository.Get().OrderByDescending(a => a.CreationDate).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductGroupUnit productGroupUnit)
        {
            if (ModelState.IsValid)
            {
                UnitOfWork.ProductGroupUnitRepository.Insert(productGroupUnit);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(productGroupUnit);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroupUnit productGroupUnit = UnitOfWork.ProductGroupUnitRepository.GetById(id.Value);
            if (productGroupUnit == null)
            {
                return HttpNotFound();
            }
            return View(productGroupUnit);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductGroupUnit productGroupUnit)
        {
            if (ModelState.IsValid)
            {
                productGroupUnit.IsDeleted = false;
                UnitOfWork.ProductGroupUnitRepository.Update(productGroupUnit);
                UnitOfWork.Save();

                return RedirectToAction("Index");
            }
            return View(productGroupUnit);
        }
        
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroupUnit productGroupUnit = UnitOfWork.ProductGroupUnitRepository.GetById(id.Value);

            if (productGroupUnit == null)
            {
                return HttpNotFound();
            }
            return View(productGroupUnit);
        }

        // POST: ProductGroupUnits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {

            ProductGroupUnit productGroupUnit = UnitOfWork.ProductGroupUnitRepository.GetById(id);
            UnitOfWork.ProductGroupUnitRepository.Delete(productGroupUnit);
            UnitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
