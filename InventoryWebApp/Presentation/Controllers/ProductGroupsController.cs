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
    public class ProductGroupsController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public ActionResult Index()
        {
            var productGroups = UnitOfWork.ProductGroupRepository.Get().OrderBy(p=>p.Code);
            return View(productGroups.ToList());
        }
         
        public ActionResult Create()
        {
            ViewBag.ProductGroupUnitId = new SelectList(UnitOfWork.ProductGroupUnitRepository.Get(), "Id", "Title");
            return View();
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductGroup productGroup)
        {
            if (ModelState.IsValid)
            {
				productGroup.IsDeleted=false;
				productGroup.CreationDate= DateTime.Now; 
                productGroup.Id = Guid.NewGuid();
                UnitOfWork.ProductGroupRepository.Insert(productGroup);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }

            ViewBag.ProductGroupUnitId = new SelectList(UnitOfWork.ProductGroupUnitRepository.Get(), "Id", "Title", productGroup.ProductGroupUnitId);
            return View(productGroup);
        }
         
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroup productGroup = UnitOfWork.ProductGroupRepository.GetById(id.Value);
            if (productGroup == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductGroupUnitId = new SelectList(UnitOfWork.ProductGroupUnitRepository.Get(), "Id", "Title", productGroup.ProductGroupUnitId);
            return View(productGroup);
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductGroup productGroup)
        {
            if (ModelState.IsValid)
            {
				productGroup.IsDeleted=false;

                UnitOfWork.ProductGroupRepository.Update(productGroup);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            ViewBag.ProductGroupUnitId = new SelectList(UnitOfWork.ProductGroupUnitRepository.Get(), "Id", "Title", productGroup.ProductGroupUnitId);
            return View(productGroup);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroup productGroup = UnitOfWork.ProductGroupRepository.GetById(id.Value);
            if (productGroup == null)
            {
                return HttpNotFound();
            }
            return View(productGroup);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UnitOfWork.ProductGroupRepository.DeleteById(id);
            UnitOfWork.Save();
            
            return RedirectToAction("Index");
        }

    }
}
