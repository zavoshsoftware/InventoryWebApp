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

namespace Presentation.Controllers
{
    public class InputsController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public ActionResult Index()
        {
            var inputs = UnitOfWork.InputRepository.Get().OrderByDescending(i => i.CreationDate);

            return View(inputs.ToList());
        }

        public ActionResult Create()
        {
            ViewBag.CityId = new SelectList(UnitOfWork.CityRepository.Get(), "Id", "Title");
            ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName");
            ViewBag.TransporterId = new SelectList(UnitOfWork.TransporterRepository.Get(), "Id", "Title");
            ViewBag.OrderId = new SelectList(UnitOfWork.OrderRepository.Get(), "Id", "Code");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Input input)
        {
            if (ModelState.IsValid)
            {
                input.OrderId = GetOrderId(input.OrderId, input.CustomerId);
                input.IsActive = true;
                input.Code = GenerateCode.GetInputCode();

                UnitOfWork.InputRepository.Insert(input);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(UnitOfWork.CityRepository.Get(), "Id", "Title", input.CityId);
            ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName", input.CustomerId);
            ViewBag.TransporterId = new SelectList(UnitOfWork.TransporterRepository.Get(), "Id", "Title", input.TransporterId);
            ViewBag.OrderId = new SelectList(UnitOfWork.OrderRepository.Get(), "Id", "Code", input.OrderId);
            return View(input);
        }


        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Input input = UnitOfWork.InputRepository.GetById(id.Value);
            if (input == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityId = new SelectList(UnitOfWork.CityRepository.Get(), "Id", "Title", input.CityId);
            ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName", input.CustomerId);
            ViewBag.TransporterId = new SelectList(UnitOfWork.TransporterRepository.Get(), "Id", "Title", input.TransporterId);
            ViewBag.OrderId = new SelectList(UnitOfWork.OrderRepository.Get(c=>c.CustomerId==input.CustomerId), "Id", "Code", input.OrderId);
            return View(input);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Input input)
        {
            if (ModelState.IsValid)
            {
                input.IsDeleted = false;

                UnitOfWork.InputRepository.Update(input);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(UnitOfWork.CityRepository.Get(), "Id", "Title", input.CityId);
            ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName", input.CustomerId);
            ViewBag.TransporterId = new SelectList(UnitOfWork.TransporterRepository.Get(), "Id", "Title", input.TransporterId);
            ViewBag.OrderId = new SelectList(UnitOfWork.OrderRepository.Get(c => c.CustomerId == input.CustomerId), "Id", "Code", input.OrderId);
            return View(input);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Input input = UnitOfWork.InputRepository.GetById(id.Value);
            if (input == null)
            {
                return HttpNotFound();
            }
            return View(input);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            UnitOfWork.InputRepository.DeleteById(id);
            UnitOfWork.Save();


            return RedirectToAction("Index");
        }

        #region HelperMethods

        public Guid? GetOrderId(Guid? orderId, Guid customerId)
        {
           
            Guid newOrderId = new Guid(System.Web.Configuration.WebConfigurationManager.AppSettings["newOrderId"]);

            if (orderId == newOrderId)
                return CreateParentOrder(customerId);

            return orderId;
        }

        public Guid CreateParentOrder(Guid customerId)
        {
            Order order = new Order()
            {
                Code = GenerateCode.GetOrderCode(),
                ParentId = null,
                CustomerId = customerId
            };

            UnitOfWork.OrderRepository.Insert(order);
            return order.Id;
        }


        #endregion

    }
}
