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
    public class ProductsController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public ActionResult Index()
        {
            var products = UnitOfWork.ProductRepository.Get().OrderByDescending(p => p.CreationDate);
            return View(products.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.ProductCreatorId = new SelectList(UnitOfWork.ProductCreatorRepository.Get(), "Id", "Title");
            ViewBag.ProductFormId = new SelectList(UnitOfWork.ProductFormRepository.Get(), "Id", "Title");
            ViewBag.ProductGroupId = new SelectList(UnitOfWork.ProductGroupRepository.Get(), "Id", "Title");
            ViewBag.ProductStatusId = new SelectList(UnitOfWork.ProductStatusRepository.Get(), "Id", "Title");
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                UnitOfWork.ProductRepository.Insert(product);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }

            ViewBag.ProductCreatorId = new SelectList(UnitOfWork.ProductCreatorRepository.Get(), "Id", "Title", product.ProductCreatorId);
            ViewBag.ProductFormId = new SelectList(UnitOfWork.ProductFormRepository.Get(), "Id", "Title", product.ProductFormId);
            ViewBag.ProductGroupId = new SelectList(UnitOfWork.ProductGroupRepository.Get(), "Id", "Title", product.ProductGroupId);
            ViewBag.ProductStatusId = new SelectList(UnitOfWork.ProductStatusRepository.Get(), "Id", "Title", product.ProductStatusId);
            return View(product);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = UnitOfWork.ProductRepository.GetById(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductCreatorId = new SelectList(UnitOfWork.ProductCreatorRepository.Get(), "Id", "Title", product.ProductCreatorId);
            ViewBag.ProductFormId = new SelectList(UnitOfWork.ProductFormRepository.Get(), "Id", "Title", product.ProductFormId);
            ViewBag.ProductGroupId = new SelectList(UnitOfWork.ProductGroupRepository.Get(), "Id", "Title", product.ProductGroupId);
            ViewBag.ProductStatusId = new SelectList(UnitOfWork.ProductStatusRepository.Get(), "Id", "Title", product.ProductStatusId);
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                product.IsDeleted = false;
                UnitOfWork.ProductRepository.Update(product);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCreatorId = new SelectList(UnitOfWork.ProductCreatorRepository.Get(), "Id", "Title", product.ProductCreatorId);
            ViewBag.ProductFormId = new SelectList(UnitOfWork.ProductFormRepository.Get(), "Id", "Title", product.ProductFormId);
            ViewBag.ProductGroupId = new SelectList(UnitOfWork.ProductGroupRepository.Get(), "Id", "Title", product.ProductGroupId);
            ViewBag.ProductStatusId = new SelectList(UnitOfWork.ProductStatusRepository.Get(), "Id", "Title", product.ProductStatusId);
            return View(product);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = UnitOfWork.ProductRepository.GetById(id.Value);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Product product = UnitOfWork.ProductRepository.GetById(id);
            UnitOfWork.ProductRepository.Delete(product);
            UnitOfWork.Save();
            return RedirectToAction("Index");
        }
    }
}
