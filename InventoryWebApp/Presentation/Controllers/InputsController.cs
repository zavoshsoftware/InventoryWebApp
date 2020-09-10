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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Input input)
        {
            if (ModelState.IsValid)
            {
				input.IsActive=true;
				input.IsDeleted=false;
				input.CreationDate= DateTime.Now; 
                input.Id = Guid.NewGuid();
                input.Code = ReturnInputCode();

                UnitOfWork.InputRepository.Insert(input);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(UnitOfWork.CityRepository.Get(), "Id", "Title", input.CityId);
            ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName", input.CustomerId);
            ViewBag.TransporterId = new SelectList(UnitOfWork.TransporterRepository.Get(), "Id", "Title", input.TransporterId);
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
            return View(input);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Input input)
        {
            if (ModelState.IsValid)
            {
				input.IsDeleted=false;

                UnitOfWork.InputRepository.Update(input);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            ViewBag.CityId = new SelectList(UnitOfWork.CityRepository.Get(), "Id", "Title", input.CityId);
            ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName", input.CustomerId);
            ViewBag.TransporterId = new SelectList(UnitOfWork.TransporterRepository.Get(), "Id", "Title", input.TransporterId);
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

        public string ReturnInputCode()
        {
            string result = "1000";
            Input input = UnitOfWork.InputRepository.Get().OrderByDescending(current => current.Code).FirstOrDefault();
            if (!string.IsNullOrEmpty(input.Code))
                result = (Convert.ToInt32(input.Code) + 1).ToString();
            return result;
        }
    }
}
