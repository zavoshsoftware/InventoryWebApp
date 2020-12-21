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
                if (error == "error")
                {
                    TempData["Error"] = @"<p class='alert alert-danger'>خطایی رخ داد! مجددا تلاش نمایید</p> ";
                }
                else
                {
                    TempData["Error"] = @"<p class='alert alert-danger'>" + error + "</p> ";
                }
            }

            ViewBag.OrderId = new SelectList(UnitOfWork.OrderRepository.Get(c => c.CustomerId == viewModel.Input.CustomerId && c.IsActive), "Id", "Code");

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
                    decimal weight = 0;
                    Input input = UnitOfWork.InputRepository.GetById(ViewModel.Input.Id);
                    List<InputDetail> inputDetails = UnitOfWork.InputDetailsRepository.Get(current => current.InputId == input.Id).ToList();
                    foreach (InputDetail inputDetail in inputDetails)
                    {
                        weight += inputDetail.DestinationWeight;
                    }
                    weight += ViewModel.Detail.DestinationWeight;

                    if (input.DestinationWeight < weight)
                    {
                        //TempData["Error"] = "<p class='alert alert-danger'>مجموع وزن وارد شده از وزن مقصد بیشتر است</p>";
                        return RedirectToAction("Index", new { id = ViewModel.Input.Id, error = "مجموع وزن وارد شده از وزن مقصد بیشتر است" });
                    }
                    else
                    {
                        ViewModel.Detail.IsDeleted = false;
                        ViewModel.Detail.CreationDate = DateTime.Now;
                        ViewModel.Detail.Id = Guid.NewGuid();
                        ViewModel.Detail.ProductId = ViewModel.ProductId;
                        ViewModel.Detail.OrderId = SetOrder(ViewModel.OrderId, ViewModel.Input.CustomerId, ViewModel.ProductId);
                        ViewModel.Detail.InputId = ViewModel.Input.Id;
                        ViewModel.Detail.RemainDestinationWeight = ViewModel.Detail.DestinationWeight;
                        ViewModel.Detail.RemainQuantity = ViewModel.Detail.Quantity;
                        ViewModel.Detail.InputDetailStatusId = UnitOfWork.InputDetailStatusRepository.Get(c => c.Code == 1)
                            .FirstOrDefault()?.Id;

                        UnitOfWork.InputDetailsRepository.Insert(ViewModel.Detail);
                    }
                }
                else
                {
                    InputDetail inputDetail = UnitOfWork.InputDetailsRepository.GetById(ViewModel.Detail.Id);

                    inputDetail.IsDeleted = false;
                    inputDetail.ProductId = ViewModel.ProductId;
                    ViewModel.Detail.OrderId = SetOrder(ViewModel.OrderId, ViewModel.Input.CustomerId, ViewModel.ProductId);
                    inputDetail.Quantity = ViewModel.Detail.Quantity;
                    inputDetail.SourceWeight = ViewModel.Detail.SourceWeight;
                    inputDetail.DestinationWeight = ViewModel.Detail.DestinationWeight;
                    inputDetail.RemainDestinationWeight = ViewModel.Detail.DestinationWeight;
                    inputDetail.RemainQuantity = ViewModel.Detail.Quantity;


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

        public Guid SetOrder(Guid orderId, Guid customerId, Guid productId)
        {
            Guid newOrderId = new Guid(System.Web.Configuration.WebConfigurationManager.AppSettings["newOrderId"]);

            if (orderId == newOrderId)
            {
                Order order = new Order()
                {
                    Id = Guid.NewGuid(),
                    CustomerId = customerId,
                    IsMultyProduct = false,
                    CreationDate = DateTime.Now,
                    Code = GenerateCode.GetOrderCode(),
                    ProductId = productId,
                    IsActive = true,
                    IsDeleted = false,
                    ParentId = null,
                };

                UnitOfWork.OrderRepository.Insert(order);

                return order.Id;
            }

            return orderId;
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Edit(string id)
        {

            Guid detailId = new Guid(id);
            InputDetail inputDetail = UnitOfWork.InputDetailsRepository.GetById(detailId);
            return new JsonResult()
            {
                Data = new { id = inputDetail.Id, productId = inputDetail.ProductId, quantity = inputDetail.Quantity, destinationWeight = inputDetail.DestinationWeight.ToString(), sourceWeight = inputDetail.SourceWeight.ToString() },
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
        public ActionResult Details(string orderId, string productId)
        {
            Guid orderIdGuid = new Guid(orderId);
            Guid productIdGuid = new Guid(productId);

            List<InputDetail> inputDetails = UnitOfWork.InputDetailsRepository
                .Get(c => c.OrderId == orderIdGuid && c.ProductId == productIdGuid).ToList();

            List<InputDetailParentsViewModel> inputDetailParents = new List<InputDetailParentsViewModel>();

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
