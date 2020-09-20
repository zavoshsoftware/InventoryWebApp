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
    public class ProductFormsController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public ActionResult Index()
        {
            return View(UnitOfWork.ProductFormRepository.Get().OrderByDescending(a => a.CreationDate).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductForm productForm)
        {
            if (ModelState.IsValid)
            {
                UnitOfWork.ProductFormRepository.Insert(productForm);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(productForm);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductForm productForm = UnitOfWork.ProductFormRepository.GetById(id.Value);
            if (productForm == null)
            {
                return HttpNotFound();
            }
            return View(productForm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductForm productForm)
        {
            if (ModelState.IsValid)
            {
                productForm.IsDeleted = false;
                UnitOfWork.ProductFormRepository.Update(productForm);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(productForm);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductForm productForm = UnitOfWork.ProductFormRepository.GetById(id.Value);
            if (productForm == null)
            {
                return HttpNotFound();
            }
            return View(productForm);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProductForm productForm = UnitOfWork.ProductFormRepository.GetById(id);
          UnitOfWork.ProductFormRepository.Delete(productForm);
            UnitOfWork.Save();
            return RedirectToAction("Index");
        }
         
    }
}
