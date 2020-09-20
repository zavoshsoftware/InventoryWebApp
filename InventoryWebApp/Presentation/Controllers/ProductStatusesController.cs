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
    public class ProductStatusesController : Infrastructure.BaseControllerWithUnitOfWork
    { 

        // GET: ProductStatuses
        public ActionResult Index()
        {
            return View(UnitOfWork.ProductStatusRepository.Get().OrderByDescending(a=>a.CreationDate).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductStatus productStatus)
        {
            if (ModelState.IsValid)
            {
			UnitOfWork.ProductStatusRepository.Insert(productStatus);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(productStatus);
        }
         
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductStatus productStatus = UnitOfWork.ProductStatusRepository.GetById(id.Value);
            if (productStatus == null)
            {
                return HttpNotFound();
            }
            return View(productStatus);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductStatus productStatus)
        {
            if (ModelState.IsValid)
            {
				productStatus.IsDeleted=false;
                UnitOfWork.ProductStatusRepository.Update(productStatus);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(productStatus);
        }
         
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductStatus productStatus = UnitOfWork.ProductStatusRepository.GetById(id.Value);
            if (productStatus == null)
            {
                return HttpNotFound();
            }
            return View(productStatus);
        }
         
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProductStatus productStatus = UnitOfWork.ProductStatusRepository.GetById(id);
            UnitOfWork.ProductStatusRepository.Delete(productStatus);
            UnitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
