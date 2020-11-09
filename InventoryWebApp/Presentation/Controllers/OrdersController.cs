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
        public ActionResult List()
        {
            var orders = UnitOfWork.OrderRepository.Get().OrderByDescending(o => o.CreationDate);
            return View(orders.ToList());
        }



        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName");
            ViewBag.ProductId = new SelectList(UnitOfWork.ProductRepository.Get(), "Id", "Title");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                if (!order.IsMultyProduct && order.ProductId == null)
                {
                    ModelState.AddModelError("productRequired", "محصول را انتخاب کنید.");

                    ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName", order.CustomerId);
                    ViewBag.ProductId = new SelectList(UnitOfWork.ProductRepository.Get(), "Id", "Title", order.ProductId);

                    return View(order);
                }


                order.IsActive = true;
                order.Code = GenerateCode.GetOrderCode();
                order.ParentId = null;

                UnitOfWork.OrderRepository.Insert(order);
                UnitOfWork.Save();
                return RedirectToAction("Index");

            }

            ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName", order.CustomerId);
            ViewBag.ProductId = new SelectList(UnitOfWork.ProductRepository.Get(), "Id", "Title", order.ProductId);

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
            ViewBag.ProductId = new SelectList(UnitOfWork.ProductRepository.Get(), "Id", "Title", order.ProductId);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order order)
        {
            if (ModelState.IsValid)
            {
                if (!order.IsMultyProduct && order.ProductId == null)
                {
                    ModelState.AddModelError("productRequired", "محصول را انتخاب کنید.");

                    ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName", order.CustomerId);
                    ViewBag.ProductId = new SelectList(UnitOfWork.ProductRepository.Get(), "Id", "Title", order.ProductId);

                    return View(order);
                }


                order.IsDeleted = false;
                UnitOfWork.OrderRepository.Update(order);
                UnitOfWork.Save();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName", order.CustomerId);
            ViewBag.ProductId = new SelectList(UnitOfWork.ProductRepository.Get(), "Id", "Title", order.ProductId);
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

            OrderDetailViewModel orderDetail = new OrderDetailViewModel();

            orderDetail.ChildOrderCode = order.Code;
            orderDetail.ChildOrderCustomer = order.Customer.FullName;
            orderDetail.OrderId = id.Value;

            if (order.ParentId == null)
            {
                orderDetail.ParentOrderCode = order.Code;
                orderDetail.ParentOrderCustomer = order.Customer.FullName;

            }
            else
            {
                orderDetail.ParentOrderCode = order.Parent.Code;
                orderDetail.ParentOrderCustomer = order.Parent.Customer.FullName;
            }

            List<Product> products = GetProductsByOrder(order);

            ViewBag.ProductId = new SelectList(GetProductsByOrder(order), "Id", "Title", products.FirstOrDefault()?.Id);

            orderDetail.Products = GetProductInfo(order.Id);

            orderDetail.ChildOrders = GetChildOrders(order.Id);


            ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName");
            ViewBag.ExitDriverId = new SelectList(UnitOfWork.ExitDriverRepository.Get(), "Id", "FullName");
            ViewBag.ExitId = new SelectList(UnitOfWork.ExitRepository.Get(c => c.ExitComplete == false), "Id", "Order");

            return View(orderDetail);
        }

        public List<ChildOrderViewModel> GetChildOrders(Guid parentOrderId)
        {
            List<ChildOrderViewModel> result = new List<ChildOrderViewModel>();

            List<Order> orders = UnitOfWork.OrderRepository.Get(c => c.ParentId == parentOrderId).ToList();

            foreach (Order order in orders)
            {
                InputDetail inputDetail =
                    UnitOfWork.InputDetailsRepository.Get(c => c.OrderId == order.Id).FirstOrDefault();

                if (inputDetail != null)
                {
                    result.Add(new ChildOrderViewModel()
                    {
                        OrderId = order.Id,
                        OrderCustomer = order.Customer.FullName,
                        ProductId = inputDetail.ProductId,
                        ProducTitle = inputDetail.Product.Title,
                        Quantity = inputDetail.RemainQuantity.ToString(),
                        OrderCode = order.Code,
                        Weight = inputDetail.RemainDestinationWeight.ToString(),
                        InputDetailStatus = inputDetail.InputDetailStatus?.Title,
                        InitialWeight = inputDetail.DestinationWeight.ToString(),
                        InitialQuantity = inputDetail.Quantity.ToString(),
                        InputDetailId = inputDetail.Id
                    });
                }


            }

            return result;
        }
        public List<ProductInfoViewModel> GetProductInfo(Guid orderId)
        {
            List<ProductInfoViewModel> result = new List<ProductInfoViewModel>();

            List<InputDetail> inputDetails = UnitOfWork.InputDetailsRepository
                .Get(c => c.OrderId == orderId).ToList();


            decimal initialQty = 0;
            decimal remainQty = 0;
            decimal remainWeight = 0;
            decimal initialWeight = 0;




            foreach (InputDetail inputDetail in inputDetails)
            {
                initialQty += inputDetail.Quantity;
                initialWeight += inputDetail.DestinationWeight;

                remainQty += inputDetail.RemainQuantity;
                remainWeight += inputDetail.RemainDestinationWeight;

                result.Add(new ProductInfoViewModel()
                {
                    ProductId = inputDetail.ProductId,
                    RemainWeight = remainWeight.ToString(),
                    InitialQty = initialQty.ToString(),
                    InitialWeight = initialWeight.ToString(),
                    RemainQty = remainQty.ToString(),
                    ProductTitle = inputDetail.Product.Title,
                    InputDetailId = inputDetail.Id,
                    InputDetailStatusTitle = inputDetail.InputDetailStatus.Title
                });
            }



            return result;
        }

        public List<Product> GetProductsByOrder(Order order)
        {
            List<Product> products = new List<Product>();

            if (!order.IsMultyProduct)
                products.Add(order.Product);

            else
            {
                List<InputDetail> inputDetails =
                    UnitOfWork.InputDetailsRepository.Get(c => c.OrderId == order.Id).ToList();

                foreach (InputDetail inputDetail in inputDetails)
                {
                    products.Add(inputDetail.Product);
                }
            }

            return products;
        }

        public ActionResult InputDetails(Guid id)
        {
            List<InputDetail> inputDetails =
                UnitOfWork.InputDetailsRepository.Get(current => current.OrderId == id).ToList();

            Order order = UnitOfWork.OrderRepository.GetById(id);

            ExitInputDetailViewModel exit = new ExitInputDetailViewModel();

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

                exit.InputDetails = inputDetails;
            }
            //ViewBag.CustomerId = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName");
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
        public ActionResult ShowLoadingData(string inputDetailId)
        {
            Guid inputDetailIdGuid = new Guid(inputDetailId);

            InputDetail inputDetail = UnitOfWork.InputDetailsRepository.GetById(inputDetailIdGuid);

            Order order = UnitOfWork.OrderRepository.GetById(inputDetail.OrderId.Value);

            Product product = UnitOfWork.ProductRepository.GetById(inputDetail.ProductId);


            TransferDetailViewModel transfer = new TransferDetailViewModel()
            {
                RemainQuantity = inputDetail.RemainQuantity,
                RemainWight = inputDetail.RemainDestinationWeight,
                OrderCode = order.Code,
                ProductTitle = product.Title,
                CustomerFullName = order.Customer.FullName,
                ParentOrderId = order.Id.ToString(),
                ProductId = product.Id.ToString()
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

                //  Product product = UnitOfWork.ProductRepository.GetById(productIdGuid);

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


        [HttpPost]
        public ActionResult PostLoading(string orderId, string productId, string weight, string qty, string inputDetailId, string driverId
            , string carNumber, string phone, string desc, string exitId)
        {
            try
            {
                decimal weightDecimal = 0;
                int qtyInt = 0;
                string message = "message-";
                Guid orderIdGuid = new Guid(orderId);
                Guid productIdGuid = new Guid(productId);
                Guid inputDetailIdGuid = new Guid(inputDetailId);

                if (!string.IsNullOrEmpty(weight))
                    weightDecimal = Convert.ToDecimal(weight);

                if (!string.IsNullOrEmpty(qty))
                    qtyInt = Convert.ToInt32(qty);

                //Guid customerIdGuid = new Guid(customerId);

                Order order = UnitOfWork.OrderRepository.GetById(orderIdGuid);

                //  Product product = UnitOfWork.ProductRepository.GetById(productIdGuid);

                //ProductRemainsViewModel remain = GetRemainProduct(productIdGuid, orderIdGuid, order.CustomerId);
                InputDetail inputDetail = UnitOfWork.InputDetailsRepository.GetById(inputDetailIdGuid);
                decimal unit = inputDetail.RemainDestinationWeight / inputDetail.RemainQuantity;

                if (!string.IsNullOrEmpty(qty) && inputDetail.RemainQuantity < qtyInt)
                {
                    return Json(message + "تعداد وارد شده بیش از تعداد باقی مانده می باشد", JsonRequestBehavior.AllowGet);

                }

                if (!string.IsNullOrEmpty(weight) && inputDetail.RemainDestinationWeight < weightDecimal)
                {
                    return Json(message + "وزن وارد شده بیش از وزن باقی مانده می باشد", JsonRequestBehavior.AllowGet);

                }


                if (string.IsNullOrEmpty(exitId))
                {
                    Exit exit = CreateExit(order.CustomerId, driverId, carNumber, phone, desc);
                    CreateExitDetail(exit.Id, inputDetailIdGuid, qtyInt, weightDecimal, unit);
                    UpdateInputDetail(inputDetailIdGuid, qtyInt, weightDecimal, unit);
                }
                else
                {
                    int exitOrder = Convert.ToInt32(exitId);
                    Exit exit = UnitOfWork.ExitRepository.Get(current => current.Order == exitOrder && current.ExitComplete == false).FirstOrDefault();

                    if (exit == null)
                        return Json(message + "ردیف بارگیری اشتباه است", JsonRequestBehavior.AllowGet);

                    CreateExitDetail(exit.Id, inputDetailIdGuid, qtyInt, weightDecimal, unit);
                    UpdateInputDetail(inputDetailIdGuid, qtyInt, weightDecimal, unit);
                }

                //if (customerIdGuid == order.CustomerId)
                //    return Json(message + "امکان انتقال حواله به مالک قبلی آن وجود ندارد", JsonRequestBehavior.AllowGet);


                //Guid childOrderId = CreateChildOrder(orderIdGuid, customerIdGuid);

                //decimal unit = remain.RemainWeight / remain.RemainQuantity;

                //SeprateInputDetail(productIdGuid, orderIdGuid, order.CustomerId, qtyInt, weightDecimal, childOrderId, unit);

                //UnitOfWork.Save();

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

                    if (remainQty <= 0)
                    {
                        decimal oldQty = inputDetail.RemainQuantity;

                        decimal weightByQty = unit * oldQty;

                        inputDetail.RemainQuantity = 0;

                        inputDetail.RemainDestinationWeight = 0;

                        inputDetail.InputDetailStatusId = UnitOfWork.InputDetailStatusRepository.Get(c => c.Code == 3)
                            .FirstOrDefault()?.Id;

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

                    decimal remainWeight = inputDetail.RemainDestinationWeight - weight;

                    if (remainWeight <= 0)
                    {
                        decimal oldWeight = inputDetail.RemainDestinationWeight;

                        decimal qtyByWeight = oldWeight / unit;


                        inputDetail.RemainQuantity = 0;

                        inputDetail.RemainDestinationWeight = 0;

                        inputDetail.InputDetailStatusId = UnitOfWork.InputDetailStatusRepository.Get(c => c.Code == 3)
                            .FirstOrDefault()?.Id;

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
                //Code = GenerateCode.GetInputDetailCode(childOrderId),
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
                SourceWeight = newWeight,
                InputDetailStatusId = UnitOfWork.InputDetailStatusRepository.Get(c => c.Code == 1)
                    .FirstOrDefault()?.Id

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
                        ProductTitle = inputDetail.Product.Title,
                        InputDetailId = inputDetail.Id
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

        #region ExitMethods

        public Exit CreateExit(Guid customerId, string driverId
        , string carNumber, string phone, string desc)
        {
            try
            {
                Guid exitDriverId = new Guid(driverId);
                Exit exit = new Exit()
                {
                    CreationDate = DateTime.Now,
                    CustomerId = customerId,
                    ExitDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    ExitComplete = false,
                    Order = GenerateCode.GetExitOrder(),
                    Code = GenerateCode.GetExitCode(),
                    ExitDriverId = exitDriverId,
                    CarNumber = carNumber,
                    DriverPhone = phone,
                    Description = desc
                };
                UnitOfWork.ExitRepository.Insert(exit);
                UnitOfWork.Save();
                return exit;
            }
            catch (Exception e)
            {

                return null;
            }
        }
        public ActionResult CreateExitDetail(Guid exitId, Guid inputDetailId, decimal quantity, decimal weight, decimal unit)
        {
            try
            {
                decimal weightMain = quantity * unit;
                if (quantity == 0 && weight > 0)
                {
                    weightMain = weight;
                    quantity = weight / unit;

                }
                ExitDetail exitDetail = new ExitDetail()
                {
                    CreationDate = DateTime.Now,
                    ExitId = exitId,
                    InputDetailId = inputDetailId,
                    IsActive = true,
                    IsDeleted = false,
                   InitialQuantity = quantity,
                    InitialWeight = weightMain,
                    FullWeight = 0,
                    EmptyWeight = 0,
                    PureWeight = weightMain
                };
                UnitOfWork.ExitDetailRepository.Insert(exitDetail);
                UnitOfWork.Save();

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateInputDetail(Guid inputDetailId, decimal quantity, decimal weight, decimal unit)
        {
            try
            {
                decimal weightMain = quantity * unit;
                if (quantity == 0 && weight > 0)
                {
                    weightMain = weight;
                    quantity = weight / unit;

                }

                InputDetail inputDetail = UnitOfWork.InputDetailsRepository.GetById(inputDetailId);
                inputDetail.RemainQuantity = inputDetail.RemainQuantity - quantity;
                inputDetail.RemainDestinationWeight = inputDetail.RemainDestinationWeight - weightMain;
                inputDetail.InputDetailStatusId =
                    UnitOfWork.InputDetailStatusRepository.Get(c => c.Code == 2).FirstOrDefault()?.Id;
                UnitOfWork.InputDetailsRepository.Update(inputDetail);
                UnitOfWork.Save();
                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {

                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        [HttpPost]
        public ActionResult GetOrderByProduct(string productId, string inputId)
        {
            Guid productIdGuid = new Guid(productId);
            Guid inputIdGuid = new Guid(inputId);

            Input input = UnitOfWork.InputRepository.GetById(inputIdGuid);

            List<Order> orders = UnitOfWork.OrderRepository.Get(c =>
                c.CustomerId == input.CustomerId && c.ParentId == null &&
                (c.IsMultyProduct || c.ProductId == productIdGuid)).ToList();

            List<DropDownKeyValueViewModel> dropDownList = new List<DropDownKeyValueViewModel>();

            foreach (Order order in orders)
            {
                dropDownList.Add(new DropDownKeyValueViewModel()
                {
                    Text = order.Code,
                    Value = order.Id.ToString()
                });
            }

            return Json(dropDownList, JsonRequestBehavior.AllowGet);
        }


    }
}

