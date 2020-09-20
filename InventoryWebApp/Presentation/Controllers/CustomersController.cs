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
    public class CustomersController : Infrastructure.BaseControllerWithUnitOfWork
    {

        public ActionResult Index()
        {
            return View(UnitOfWork.CustomerRepository.Get().OrderByDescending(a=>a.CreationDate).ToList());
        } 
     
        public ActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
				customer.IsDeleted=false;
				customer.CreationDate= DateTime.Now; 
                customer.Id = Guid.NewGuid();
                UnitOfWork.CustomerRepository.Insert(customer);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = UnitOfWork.CustomerRepository.GetById(id.Value);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            if (ModelState.IsValid)
            {
				customer.IsDeleted=false;

                UnitOfWork.CustomerRepository.Update(customer);
                UnitOfWork.Save();
                
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = UnitOfWork.CustomerRepository.GetById(id.Value);

            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UnitOfWork.CustomerRepository.DeleteById(id);
            UnitOfWork.Save();
           
            return RedirectToAction("Index");
        }
    }
}
