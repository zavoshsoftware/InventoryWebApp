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
    public class ProductCreatorsController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public ActionResult Index()
        {
            return View(UnitOfWork.ProductCreatorRepository.Get().OrderByDescending(a => a.CreationDate).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductCreator productCreator)
        {
            if (ModelState.IsValid)
            {
                UnitOfWork.ProductCreatorRepository.Insert(productCreator);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(productCreator);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCreator productCreator = UnitOfWork.ProductCreatorRepository.GetById(id.Value);
            if (productCreator == null)
            {
                return HttpNotFound();
            }
            return View(productCreator);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductCreator productCreator)
        {
            if (ModelState.IsValid)
            {
                productCreator.IsDeleted = false;
                UnitOfWork.ProductCreatorRepository.Update(productCreator);
                UnitOfWork.Save();

                return RedirectToAction("Index");
            }
            return View(productCreator);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCreator productCreator = UnitOfWork.ProductCreatorRepository.GetById(id.Value);
            if (productCreator == null)
            {
                return HttpNotFound();
            }
            return View(productCreator);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            ProductCreator productCreator = UnitOfWork.ProductCreatorRepository.GetById(id);
            UnitOfWork.ProductCreatorRepository.Delete(productCreator);
            UnitOfWork.Save();

            return RedirectToAction("Index");
        }

    }
}
