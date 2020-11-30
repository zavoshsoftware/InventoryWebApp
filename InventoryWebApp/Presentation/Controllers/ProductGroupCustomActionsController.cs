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
    public class ProductGroupCustomActionsController : Infrastructure.BaseControllerWithUnitOfWork
    {

        // GET: ProductGroupCustomActions
        public ActionResult Index(Guid id)
        {
            var productGroupCustomActions = UnitOfWork.ProductGroupCustomActionRepository.Get(p=>p.ProductGroupId==id)
                .Include(p => p.CustomAction).Where(p=>p.IsDeleted==false).OrderByDescending(p=>p.CreationDate).Include(p => p.ProductGroup).Where(p=>p.IsDeleted==false).OrderByDescending(p=>p.CreationDate);
            return View(productGroupCustomActions.ToList());
        }

        
#region Crud
        public ActionResult Create(Guid id)
        {
            ViewBag.CustomActionId = new SelectList(UnitOfWork.CustomActionRepository.Get(), "Id", "Title");
            ViewBag.Id = id;
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductGroupCustomAction productGroupCustomAction,Guid id)
        {
            if (ModelState.IsValid)
            {
                productGroupCustomAction.ProductGroupId = id;
				productGroupCustomAction.IsDeleted=false;
				productGroupCustomAction.CreationDate= DateTime.Now; 
                productGroupCustomAction.Id = Guid.NewGuid();
                UnitOfWork.ProductGroupCustomActionRepository.Insert(productGroupCustomAction);
                UnitOfWork.Save();
                return RedirectToAction("Index",new { id=id});
            }

            ViewBag.CustomActionId = new SelectList(UnitOfWork.CustomActionRepository.Get(), "Id", "Title", productGroupCustomAction.CustomActionId);
            ViewBag.Id = id;
            return View(productGroupCustomAction);
        }

        // GET: ProductGroupCustomActions/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroupCustomAction productGroupCustomAction = UnitOfWork.ProductGroupCustomActionRepository.GetById(id.Value);
            if (productGroupCustomAction == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomActionId = new SelectList(UnitOfWork.CustomActionRepository.Get(), "Id", "Title", productGroupCustomAction.CustomActionId);
            ViewBag.Id = productGroupCustomAction.ProductGroupId;
            return View(productGroupCustomAction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductGroupCustomAction productGroupCustomAction)
        {
            if (ModelState.IsValid)
            {
				productGroupCustomAction.IsDeleted=false;
                UnitOfWork.ProductGroupCustomActionRepository.Update(productGroupCustomAction);
                UnitOfWork.Save();
                return RedirectToAction("Index",new { id=productGroupCustomAction.ProductGroupId});
            }
            ViewBag.CustomActionId = new SelectList(UnitOfWork.CustomActionRepository.Get(), "Id", "Title", productGroupCustomAction.CustomActionId);
            ViewBag.Id = productGroupCustomAction.ProductGroupId;
            return View(productGroupCustomAction);
        }

        // GET: ProductGroupCustomActions/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductGroupCustomAction productGroupCustomAction = UnitOfWork.ProductGroupCustomActionRepository.GetById(id.Value);
            if (productGroupCustomAction == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = productGroupCustomAction.ProductGroupId;
            return View(productGroupCustomAction);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProductGroupCustomAction productGroupCustomAction = UnitOfWork.ProductGroupCustomActionRepository.GetById(id);

            UnitOfWork.ProductGroupCustomActionRepository.DeleteById(id);
            UnitOfWork.Save();
            return RedirectToAction("Index",new {id= productGroupCustomAction.ProductGroupId });
        }

#endregion
    }
}
