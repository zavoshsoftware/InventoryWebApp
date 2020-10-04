using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Models;
using ViewModels;

namespace Presentation.Controllers
{
    public class InputDetailsController : Infrastructure.BaseControllerWithUnitOfWork
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: InputDetails
        public ActionResult Index(Guid id, string error)
        {
            InputDetailViewModel viewModel = new InputDetailViewModel();
            viewModel.Input = UnitOfWork.InputRepository.GetById(id);
            viewModel.InputDetails = UnitOfWork.InputDetailsRepository.Get().Where(current => current.InputId == id).OrderByDescending(current => current.CreationDate).ToList();
            //viewModel.Detail = new InputDetail() { InputId = id };
            viewModel.EditMode = false;
            ViewBag.ProductId = new SelectList(UnitOfWork.ProductRepository.Get(), "Id", "Title");
            TempData["Error"] = "";
            if (!string.IsNullOrEmpty(error))
            {
                TempData["Error"] = @"<p class='alert alert-danger'>خطایی رخ داد! مجددا تلاش نمایید</p> ";
            }

            ViewBag.OrderId = new SelectList(UnitOfWork.OrderRepository.Get(c=>c.CustomerId==viewModel.Input.CustomerId&&c.IsActive), "Id", "Code");

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(InputDetailViewModel ViewModel)
        {
            try
            {
                if (!ViewModel.EditMode)
                {
                    ViewModel.Detail.IsDeleted = false;
                    ViewModel.Detail.CreationDate = DateTime.Now;
                    ViewModel.Detail.Id = Guid.NewGuid();
                    ViewModel.Detail.ProductId = ViewModel.ProductId;
                    ViewModel.Detail.InputId = ViewModel.Input.Id;
                    ViewModel.Detail.RemainDestinationWeight = ViewModel.Detail.DestinationWeight;
                    ViewModel.Detail.RemainQuantity = ViewModel.Detail.Quantity;
                    UnitOfWork.InputDetailsRepository.Insert(ViewModel.Detail);
                }
                else
                {
                    InputDetail inputDetail = UnitOfWork.InputDetailsRepository.GetById(ViewModel.Detail.Id);

                    inputDetail.IsDeleted = false;
                    inputDetail.ProductId = ViewModel.ProductId;
                    inputDetail.Code = ViewModel.Detail.Code;
                    inputDetail.Quantity = ViewModel.Detail.Quantity;
                    inputDetail.SourceWeight = ViewModel.Detail.SourceWeight;
                    inputDetail.DestinationWeight = ViewModel.Detail.DestinationWeight;
                    inputDetail.RemainDestinationWeight = ViewModel.Detail.DestinationWeight;
                    inputDetail.RemainQuantity= ViewModel.Detail.Quantity;

                    UnitOfWork.InputDetailsRepository.Update(inputDetail);
                }
                UnitOfWork.Save();
                ViewBag.OrderId = new SelectList(UnitOfWork.OrderRepository.Get(c => c.CustomerId == ViewModel.Input.CustomerId && c.IsActive), "Id", "Code");

                return RedirectToAction("Index", new { id = ViewModel.Input.Id });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { id = ViewModel.Input.Id, error = "error" });
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Edit(string id)
        {

            Guid detailId = new Guid(id);
            InputDetail inputDetail = UnitOfWork.InputDetailsRepository.GetById(detailId);
            return new JsonResult()
            {
                Data = new { id = inputDetail.Id, productId = inputDetail.ProductId, code = inputDetail.Code, quantity = inputDetail.Quantity, destinationWeight = inputDetail.DestinationWeight.ToString(), sourceWeight = inputDetail.SourceWeight.ToString() },
                JsonRequestBehavior = JsonRequestBehavior.DenyGet
            };

        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult DeleteConfirmed(string id)
        {
            Guid inputDetailId = new Guid(id);
            InputDetail inputDetail = db.InputDetails.Find(inputDetailId);
            try
            {
                UnitOfWork.InputDetailsRepository.DeleteById(inputDetailId);
                UnitOfWork.Save();
                return RedirectToAction("Index", new { id = inputDetail.InputId });
            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { id = inputDetail.InputId, error = "error" });
            }
        }


        [HttpPost]
        public ActionResult Details(string orderId,string productId)
        {
            Guid orderIdGuid=new Guid(orderId);
            Guid productIdGuid = new Guid(productId);

            List<InputDetail> inputDetails = UnitOfWork.InputDetailsRepository
                .Get(c => c.OrderId == orderIdGuid && c.ProductId == productIdGuid).ToList();

            List<InputDetailParentsViewModel> inputDetailParents=new List<InputDetailParentsViewModel>();

            foreach (InputDetail inputDetail in inputDetails)
            {
                inputDetailParents.Add(new InputDetailParentsViewModel()
                {
                    InputCode = inputDetail.Input.Code.ToString(),
                    DestinationWeight = inputDetail.DestinationWeight,
                    InputDate = inputDetail.Input.InputDate.ToShortDateString(),
                    Quantity = inputDetail.Quantity,
                    SourceWeight = inputDetail.SourceWeight
                });
            }

            return Json(inputDetailParents, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

       
    }
}
