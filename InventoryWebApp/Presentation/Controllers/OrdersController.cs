using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Helper;
using Models;
using ViewModels;

namespace Presentation.Controllers
{
    public class OrdersController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public ActionResult Index()
        {
            var orders = UnitOfWork.OrderRepository.Get().OrderByDescending(o=>o.CreationDate);
            return View(orders.ToList());
        }

        public ActionResult List()
        {
            var orders = UnitOfWork.OrderRepository.Get().OrderByDescending(o=>o.CreationDate);
            return View(orders.ToList());
        }

        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = UnitOfWork.OrderRepository.GetById(id.Value);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName");
            ViewBag.ParentId = new SelectList(UnitOfWork.OrderRepository.Get(), "Id", "Code");

            return View();
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
              
                order.IsActive = true;
                order.Code = GenerateCode.GetOrderCode();

                UnitOfWork.OrderRepository.Insert(order);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName", order.CustomerId);
            ViewBag.ParentId = new SelectList(UnitOfWork.OrderRepository.Get(), "Id", "Code", order.ParentId);
            return View(order);
        }
         
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = UnitOfWork.OrderRepository.GetById(id.Value);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName", order.CustomerId);
            ViewBag.ParentId = new SelectList(UnitOfWork.OrderRepository.Get(), "Id", "Code", order.ParentId);
            return View(order);
        }

         [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
				order.IsDeleted=false;
                UnitOfWork.OrderRepository.Update(order);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName", order.CustomerId);
            ViewBag.ParentId = new SelectList(UnitOfWork.OrderRepository.Get(), "Id", "Code", order.ParentId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = UnitOfWork.OrderRepository.GetById(id.Value);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }
         
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Order order = UnitOfWork.OrderRepository.GetById(id);
            UnitOfWork.OrderRepository.Delete(order);
            UnitOfWork.Save();
            return RedirectToAction("Index");
        }

        public ActionResult GetCustomerOrders(string id)
        {
            Guid customerId = new Guid(id);
            //   ViewBag.cityId = ReturnCities(provinceId);
           List<Order> orders = UnitOfWork.OrderRepository.Get(c => c.CustomerId == customerId).OrderBy(current => current.Code).ToList();

            List<DropDownKeyValueViewModel> items = new List<DropDownKeyValueViewModel>();

            foreach (Order order in orders)
            {
                items.Add(new DropDownKeyValueViewModel()
                {
                    Text = order.Code,
                    Value = order.Id.ToString()
                });
            }
            return Json(items, JsonRequestBehavior.AllowGet);
        }


    }
}
