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
        #region CRUD

        public ActionResult Index()
        {
            var orders = UnitOfWork.OrderRepository.Get().OrderByDescending(o => o.CreationDate);
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
                order.IsDeleted = false;
                UnitOfWork.OrderRepository.Update(order);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName", order.CustomerId);
            ViewBag.ParentId = new SelectList(UnitOfWork.OrderRepository.Get(), "Id", "Code", order.ParentId);
            return View(order);
        }

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

        #endregion

        public ActionResult AllocateOrder(string orderId, string inputId)
        {
            Guid orderIdGuid = new Guid(orderId);
            Guid inputIdGuid = new Guid(inputId);

            Input input = UnitOfWork.InputRepository.GetById(inputIdGuid);

            List<InputDetail> inputDetails =
                UnitOfWork.InputDetailsRepository.Get(c => c.InputId == inputIdGuid).ToList();

            Guid? id = GetOrderId(orderIdGuid, input.CustomerId);

            foreach (InputDetail inputDetail in inputDetails)
            {
                inputDetail.OrderId = id;

                UnitOfWork.InputDetailsRepository.Update(inputDetail);
            }

            UnitOfWork.Save();

            return Json("true", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Transfer(Guid id)
        {
            TransferViewModel transfer = new TransferViewModel()
            {
                InputDetails = GetInputDetailsGroupByProductId(id),

                Customer = UnitOfWork.CustomerRepository.GetById(id)
            };
            ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName");

            return View(transfer);
        }

        [HttpPost]
        public ActionResult ShowTransferData(string orderId, string productId)
        {
            Guid orderIdGuid = new Guid(orderId);
            Guid productIdGuid = new Guid(productId);

            Order order = UnitOfWork.OrderRepository.GetById(orderIdGuid);

            Product product = UnitOfWork.ProductRepository.GetById(productIdGuid);

           
            TransferDetailViewModel transfer=new TransferDetailViewModel()
            {
                RemainQuantity = GetRemainProduct(productIdGuid,orderIdGuid,order.CustomerId).RemainQuantity,
                RemainWight = GetRemainProduct(productIdGuid,orderIdGuid,order.CustomerId).RemainWeight,
                OrderCode = GenerateCode.GetChildOrderCode(order.Id),
                ProductTitle = product.Title,
                CustomerFullName = order.Customer.FullName,
            };

            return Json(transfer, JsonRequestBehavior.AllowGet);
        }
        #region HelperMethods

        public List<InputDetailTransferViewModel> GetInputDetailsGroupByProductId(Guid customerId)
        {
            List<InputDetail> inputDetails = UnitOfWork.InputDetailsRepository
                .Get(current => current.Order.CustomerId == customerId && current.OrderId != null).ToList();

            List<InputDetailTransferViewModel> transferInputDetail = new List<InputDetailTransferViewModel>();

            foreach (InputDetail inputDetail in inputDetails)
            {
                InputDetailTransferViewModel oInputDetailTransferViewModel =
                    transferInputDetail.FirstOrDefault(current =>
                        current.OrderId == inputDetail.OrderId && current.ProductId == inputDetail.ProductId);

                if (oInputDetailTransferViewModel != null)
                {
                    oInputDetailTransferViewModel.RemainQuantity =
                        oInputDetailTransferViewModel.RemainQuantity + inputDetail.RemainQuantity;

                    oInputDetailTransferViewModel.RemainWeight =
                        oInputDetailTransferViewModel.RemainWeight + inputDetail.RemainDestinationWeight;
                }
                else
                {
                    transferInputDetail.Add(new InputDetailTransferViewModel()
                    {
                        OrderCode = inputDetail.Order.Code,
                        OrderId = inputDetail.OrderId.Value,
                        RemainQuantity = inputDetail.RemainQuantity,
                        ProductId = inputDetail.ProductId,
                        RemainWeight = inputDetail.RemainDestinationWeight,
                        ProductTitle = inputDetail.Product.Title
                    });
                }
            }

            return transferInputDetail;
        }

        public ProductRemainsViewModel GetRemainProduct(Guid productId, Guid orderId,Guid customerId)
        {
            ProductRemainsViewModel remain = new ProductRemainsViewModel()
            {
                RemainQuantity = 0,
                RemainWeight = 0
            };

            List<InputDetail> inputDetails = UnitOfWork.InputDetailsRepository
                .Get(current => current.Order.CustomerId == customerId && current.OrderId == orderId &&
                                current.ProductId == productId && current.OrderId != null).ToList();

            foreach (var inputDetail in inputDetails)
            {
                remain.RemainQuantity = remain.RemainQuantity + inputDetail.RemainQuantity;
                remain.RemainWeight = remain.RemainWeight + inputDetail.RemainDestinationWeight;
            }
            return remain;
        }
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
                CustomerId = customerId,
                IsActive = true
            };

            UnitOfWork.OrderRepository.Insert(order);
            return order.Id;
        }


        #endregion


    }
}
