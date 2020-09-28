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

        public ActionResult InputDetails(Guid id)
        {
            List<InputDetail> inputDetails =
                UnitOfWork.InputDetailsRepository.Get(current => current.OrderId == id).ToList();

            Order order = UnitOfWork.OrderRepository.GetById(id);

            ExitInputDetailViewModel exit=new ExitInputDetailViewModel();

            if (order != null)
            {
                exit.ChildOrderId = id;
                exit.ChildOrderCode = order.Code;
                exit.ChildCustomerName = order.Customer.FullName;

                if (order.ParentId != null)
                {
                    exit.ParentOrderCode = order.Parent.Code;
                    exit.ParentCustomerName = order.Parent.Customer.FullName;
                }

                exit.InputDetails = GetInputDetailsGroupByOrderId(id);
            }

            return View(exit);
        }


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
                InputDetails = GetInputDetailsGroupByCustomerId(id),

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


            TransferDetailViewModel transfer = new TransferDetailViewModel()
            {
                RemainQuantity = GetRemainProduct(productIdGuid, orderIdGuid, order.CustomerId).RemainQuantity,
                RemainWight = GetRemainProduct(productIdGuid, orderIdGuid, order.CustomerId).RemainWeight,
                OrderCode = GenerateCode.GetChildOrderCode(order.Id),
                ProductTitle = product.Title,
                CustomerFullName = order.Customer.FullName,
                ParentOrderId = orderId,
                ProductId = productId
            };

            return Json(transfer, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult PostTransfer(string orderId, string productId, string weight, string qty, string customerId)
        {
            try
            {
                decimal weightDecimal = 0;
                int qtyInt = 0;
                string message = "message-";
                Guid orderIdGuid = new Guid(orderId);
                Guid productIdGuid = new Guid(productId);

                if (!string.IsNullOrEmpty(weight))
                    weightDecimal = Convert.ToDecimal(weight);

                if (!string.IsNullOrEmpty(qty))
                    qtyInt = Convert.ToInt32(qty);

                Guid customerIdGuid = new Guid(customerId);

                Order order = UnitOfWork.OrderRepository.GetById(orderIdGuid);

                Product product = UnitOfWork.ProductRepository.GetById(productIdGuid);

                ProductRemainsViewModel remain = GetRemainProduct(productIdGuid, orderIdGuid, order.CustomerId);

                if (!string.IsNullOrEmpty(qty) && remain.RemainQuantity < qtyInt)
                {
                    return Json(message + "تعداد وارد شده بیش از تعداد باقی مانده می باشد", JsonRequestBehavior.AllowGet);

                }

                if (!string.IsNullOrEmpty(weight) && remain.RemainWeight < weightDecimal)
                {
                    return Json(message + "وزن وارد شده بیش از وزن باقی مانده می باشد", JsonRequestBehavior.AllowGet);

                }

                if (customerIdGuid == order.CustomerId)
                    return Json(message + "امکان انتقال حواله به مالک قبلی آن وجود ندارد", JsonRequestBehavior.AllowGet);


                Guid childOrderId = CreateChildOrder(orderIdGuid, customerIdGuid);

                decimal unit = remain.RemainWeight / remain.RemainQuantity;

                SeprateInputDetail(productIdGuid, orderIdGuid, order.CustomerId, qtyInt, weightDecimal, childOrderId, unit);

                UnitOfWork.Save();

                return Json("true", JsonRequestBehavior.AllowGet);

            }
            catch (Exception e)
            {
                return Json("false", JsonRequestBehavior.AllowGet);

            }
        }


        #region HelperMethods

        public void SeprateInputDetail(Guid productId, Guid orderId, Guid customerId, decimal qty, decimal weight, Guid childOrderId, decimal unit)
        {
            List<InputDetail> inputDetails = UnitOfWork.InputDetailsRepository
                .Get(current => current.ProductId == productId && current.OrderId == orderId).ToList();

            List<InputDetail> sortedInputDetails = SortInputDetails(inputDetails, qty, weight);

            if (qty > 0)
            {
                List<InputDetail> enoughInputDetails = GetEnoughInputDetails(sortedInputDetails, qty, 0);
                  
                foreach (InputDetail inputDetail in enoughInputDetails)
                {

                    decimal remainQty = inputDetail.RemainQuantity - qty;

                    if (remainQty < 0)
                    {
                        decimal oldQty = inputDetail.RemainQuantity;

                        decimal weightByQty = unit * oldQty;

                        inputDetail.RemainQuantity = 0;

                        inputDetail.RemainDestinationWeight = 0;

                        UnitOfWork.InputDetailsRepository.Update(inputDetail);

                        CreateChildInputDetail(weightByQty, oldQty, inputDetail, childOrderId);

                        qty = qty - oldQty;
                    }
                    else
                    {
                        decimal weightByQty = unit * qty;

                        inputDetail.RemainQuantity = inputDetail.RemainQuantity - qty;

                        inputDetail.RemainDestinationWeight = inputDetail.RemainDestinationWeight - weightByQty;

                        UnitOfWork.InputDetailsRepository.Update(inputDetail);

                        CreateChildInputDetail(weightByQty, qty, inputDetail, childOrderId);
                    }
                 
                }
            }

            else if (weight > 0)
            {

                List<InputDetail> enoughInputDetails = GetEnoughInputDetails(sortedInputDetails, 0, weight);

               

                foreach (InputDetail inputDetail in enoughInputDetails)
                {

                    decimal remainWeight = inputDetail.RemainDestinationWeight-weight;

                    if (remainWeight < 0)
                    {
                        decimal oldWeight = inputDetail.RemainDestinationWeight;

                        decimal qtyByWeight = oldWeight / unit;

                         
                        inputDetail.RemainQuantity = 0;

                        inputDetail.RemainDestinationWeight =0;

                        UnitOfWork.InputDetailsRepository.Update(inputDetail);

                        CreateChildInputDetail(oldWeight, qtyByWeight, inputDetail, childOrderId);

                        weight = weight - oldWeight;
                    }
                    else
                    {
                        decimal qtyByWeight = weight / unit;

                        inputDetail.RemainQuantity = inputDetail.RemainQuantity - qtyByWeight;

                        inputDetail.RemainDestinationWeight =
                            inputDetail.RemainDestinationWeight - weight;

                        UnitOfWork.InputDetailsRepository.Update(inputDetail);

                        CreateChildInputDetail(weight, qtyByWeight, inputDetail, childOrderId);
                    }
                 
                }
            }

        }

        public List<InputDetail> GetEnoughInputDetails(List<InputDetail> sortedInputDetails, decimal targetQty, decimal targetWeight)
        {
            List<InputDetail> enoughInputDetail = new List<InputDetail>();

            if (targetQty > 0)
            {
                decimal totalQty = 0;


                foreach (InputDetail sortedInputDetail in sortedInputDetails)
                {
                    if (totalQty >= targetQty)
                        break;

                    totalQty = totalQty + sortedInputDetail.RemainQuantity;

                    enoughInputDetail.Add(sortedInputDetail);
                }
            }
            else if (targetWeight > 0)
            {
                decimal totalWeight = 0;


                foreach (InputDetail sortedInputDetail in sortedInputDetails)
                {
                    if (totalWeight >= targetWeight)
                        break;

                    totalWeight = totalWeight + sortedInputDetail.RemainDestinationWeight;

                    enoughInputDetail.Add(sortedInputDetail);
                }
            }


            return enoughInputDetail;
        }

        public void CreateChildInputDetail(decimal newWeight, decimal qty, InputDetail inputDetail, Guid childOrderId)
        {
            InputDetail oInputDetail = new InputDetail()
            {
                Code = GenerateCode.GetInputDetailCode(childOrderId),
                OrderId = childOrderId,
                Quantity = qty,
                DestinationWeight = newWeight,
                RemainQuantity = qty,
                RemainDestinationWeight = newWeight,
                InputId = inputDetail.InputId,
                IsActive = true,
                IsDeleted = false,
                ProductId = inputDetail.ProductId,
                ParentId = inputDetail.Id,
                SourceWeight = newWeight
            };

            UnitOfWork.InputDetailsRepository.Insert(oInputDetail);
        }


        public List<InputDetail> SortInputDetails(List<InputDetail> inputDetails, decimal qty, decimal weight)
        {
            if (qty > 0)
                return inputDetails.OrderBy(c => c.RemainQuantity).ToList();

            if (weight > 0)
                return inputDetails.OrderBy(c => c.RemainDestinationWeight).ToList();

            return new List<InputDetail>();
        }

        public Guid CreateChildOrder(Guid parentOrderId, Guid customerId)
        {
            Order order = new Order()
            {
                ParentId = parentOrderId,
                Code = GenerateCode.GetChildOrderCode(parentOrderId),
                CustomerId = customerId,
                IsActive = true,
                IsDeleted = false
            };

            UnitOfWork.OrderRepository.Insert(order);

            return order.Id;
        }

        public List<InputDetailTransferViewModel> GetInputDetailsGroupByCustomerId(Guid customerId)
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

        public List<InputDetailTransferViewModel> GetInputDetailsGroupByOrderId(Guid orderId)
        {
            List<InputDetail> inputDetails = UnitOfWork.InputDetailsRepository
                .Get(current => current.OrderId == orderId).ToList();

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

        public ProductRemainsViewModel GetRemainProduct(Guid productId, Guid orderId, Guid customerId)
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

