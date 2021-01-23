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
using Stimulsoft.Base.Json;

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
                order.IsLatest = true;

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
            DateTime today = DateTime.Today;
            ViewBag.ExitId = new SelectList(UnitOfWork.ExitRepository.Get(c => c.ExitComplete == false && DbFunctions.TruncateTime(c.CreationDate) == today), "Id", "Code");

            return View(orderDetail);
        }

        public List<ChildOrderViewModel> GetChildOrders(Guid parentOrderId)
        {
            List<ChildOrderViewModel> result = new List<ChildOrderViewModel>();

            List<Order> orders = UnitOfWork.OrderRepository.Get(c => c.ParentId == parentOrderId).ToList();
            Order parent = UnitOfWork.OrderRepository.GetById(parentOrderId);

            foreach (Order order in orders)
            {
                string customerName = order.Customer.FullName;
                if (order.Code == parent.Code)
                {
                    customerName = parent.Customer.FullName + "-" + order.Customer.FullName; 
                }

                InputDetail inputDetail =
                    UnitOfWork.InputDetailsRepository.Get(c => c.OrderId == order.Id).FirstOrDefault();

                if (inputDetail != null)
                {
                    result.Add(new ChildOrderViewModel()
                    {
                        OrderId = order.Id,
                        OrderCustomer = customerName,
                        ProductId = inputDetail.ProductId,
                        ProducTitle = inputDetail.Product.Title,
                        Quantity = inputDetail.RemainQuantity.ToString("N0"),
                        OrderCode = order.Code,
                        Weight = inputDetail.RemainDestinationWeight.ToString("N0"),
                        InputDetailStatus = inputDetail.InputDetailStatus?.Title,
                        InitialWeight = inputDetail.DestinationWeight.ToString("N0"),
                        InitialQuantity = inputDetail.Quantity.ToString("N0"),
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
                    RemainWeight = remainWeight.ToString("N0"),
                    InitialQty = initialQty.ToString("N0"),
                    InitialWeight = initialWeight.ToString("N0"),
                    RemainQty = remainQty.ToString("N0"),
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
            string orderCode = string.Empty;
            List<InputDetail> inputDetails = UnitOfWork.InputDetailsRepository
              .Get(c => c.OrderId == order.Id).ToList();
            if (inputDetails.Count() == 1)
            {
                orderCode = order.Code;
            }
            else
            {
                orderCode = GenerateCode.GetChildOrderCode(order.Id);
            }

                TransferDetailViewModel transfer = new TransferDetailViewModel()
            {
                RemainQuantity = GetRemainProduct(productIdGuid, orderIdGuid, order.CustomerId).RemainQuantity,
                RemainWight = GetRemainProduct(productIdGuid, orderIdGuid, order.CustomerId).RemainWeight,
                OrderCode = orderCode,
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



                Guid childOrderId = new Guid();
                List<InputDetail> inputDetails = UnitOfWork.InputDetailsRepository
               .Get(c => c.OrderId == order.Id).ToList();
                if (inputDetails.Count() == 1)//حواله فقط یک کالا دارد
                {
                    decimal initialQty = 0;
                    decimal initialWeight = 0;
                    initialQty += inputDetails.FirstOrDefault().Quantity;
                    initialWeight += inputDetails.FirstOrDefault().DestinationWeight;
                    if (initialQty == qtyInt || initialWeight == weightDecimal)
                    {
                        childOrderId = CreateChildOrder(orderIdGuid, customerIdGuid, false);
                    }
                    else
                    {
                        childOrderId = CreateChildOrder(orderIdGuid, customerIdGuid, true);
                    }
                }




                //Guid childOrderId = CreateChildOrder(orderIdGuid, customerIdGuid);

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
                    Guid id = new Guid(exitId);
                    Exit exit = UnitOfWork.ExitRepository.Get(current => current.Id == id && current.ExitComplete == false).FirstOrDefault();

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

        [HttpPost]
        public ActionResult CreateDriver(string driverName)
        {
            try
            {
                ExitDriver exitDriver = new ExitDriver();
                exitDriver.FullName = driverName;
                exitDriver.IsDeleted = false;
                exitDriver.CreationDate = DateTime.Now;
                exitDriver.Id = Guid.NewGuid();
                UnitOfWork.ExitDriverRepository.Insert(exitDriver);
                UnitOfWork.Save();

                var response = new { Value = exitDriver.Id, Text = exitDriver.FullName };
                //var result = JsonConvert.SerializeObject(response);
                //return Json(result, JsonRequestBehavior.AllowGet);
                return new JsonResult()
                {
                    Data = response,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                };
            }
            catch (Exception)
            {

                throw;
            }

        }

        public ActionResult Search(Guid? id)
        {
            OrderDetailViewModel orderDetail = new OrderDetailViewModel();
            if (id == null)
            {
                List<SelectListItem> items = new SelectList(UnitOfWork.OrderRepository.Get(), "Id", "Code").ToList();
                items.Insert(0, (new SelectListItem { Text = "انتخاب کنید...", Value = "0" }));
                ViewBag.OrderId = items;
                orderDetail.Products = new List<ProductInfoViewModel>();
                orderDetail.ChildOrders = new List<ChildOrderViewModel>();
            }
            else
            {
                Order order = UnitOfWork.OrderRepository.GetById(id.Value);

                if (order == null)
                {
                    return HttpNotFound();
                }
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


                List<SelectListItem> items = new SelectList(UnitOfWork.OrderRepository.Get(current => current.IsLatest == true), "Id", "Code",order.Id).ToList();
                items.Insert(0, (new SelectListItem { Text = "انتخاب کنید...", Value = "0" }));
                ViewBag.OrderId = items;

            }
            List<SelectListItem> itemsCustomer = new SelectList(UnitOfWork.CustomerRepository.Get(), "Id", "FullName").ToList();
            itemsCustomer.Insert(0, (new SelectListItem { Text = "انتخاب کنید...", Value = "0" }));
            ViewBag.CustomerId = itemsCustomer;
            ViewBag.ExitDriverId = new SelectList(UnitOfWork.ExitDriverRepository.Get(), "Id", "FullName");
            DateTime today = DateTime.Today;
            ViewBag.ExitId = new SelectList(UnitOfWork.ExitRepository.Get(c => c.ExitComplete == false), "Id", "Code");


            return View(orderDetail);
        }
        [HttpPost]
        public ActionResult Search(string id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Guid orderId = new Guid(id);
                Order order = UnitOfWork.OrderRepository.GetById(orderId);

                if (order == null)
                {
                    return HttpNotFound();
                }

                OrderDetailViewModel orderDetail = new OrderDetailViewModel();

                orderDetail.ChildOrderCode = order.Code;
                orderDetail.ChildOrderCustomer = order.Customer.FullName;
                orderDetail.OrderId = orderId;

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

                orderDetail.Products = GetProductInfo(order.Id);

                orderDetail.ChildOrders = GetChildOrders(order.Id);
                return PartialView("_InputDatailPartial", orderDetail);
            }
            catch (Exception exp)
            {

                return Json(exp.Message, JsonRequestBehavior.AllowGet);
            }
            
        }

        public ActionResult Kardex(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InputDetail inputDetail = UnitOfWork.InputDetailsRepository.GetById(id.Value);

            if (inputDetail == null)
            {
                return HttpNotFound();
            }
            KardexViewModel model = new KardexViewModel();
            model.OrderId = inputDetail.OrderId.Value;
            model.ParentOrderCode = inputDetail.Order.Code;
            model.ParentOrderCustomer = inputDetail.Order.Customer.FullName;
            model.OrderProductName = inputDetail.Product.Title;
            model.ChildOrders = GetKardexChild(id.Value);
            return View(model);
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
                    .FirstOrDefault()?.Id,
                CreationDate=DateTime.Now

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

        public Guid CreateChildOrder(Guid parentOrderId, Guid customerId, bool createNew)
        {
            string code = string.Empty;
            if (createNew)
            {
                code = GenerateCode.GetChildOrderCode(parentOrderId);
            }
            else
            {
                code = UnitOfWork.OrderRepository.GetById(parentOrderId).Code;
                Order parent = UnitOfWork.OrderRepository.GetById(parentOrderId);
                parent.IsLatest = false;
            }
            Order order = new Order()
            {
                ParentId = parentOrderId,
                Code = code,
                CustomerId = customerId,
                IsActive = true,
                IsDeleted = false,
                IsLatest= true
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
                IsActive = true,
                IsLatest=true
            };

            UnitOfWork.OrderRepository.Insert(order);
            return order.Id;
        }

        public List<KardexChildOrderViewModel> GetKardexChild(Guid inputDetailId)
        {
            List<KardexChildOrderViewModel> result = new List<KardexChildOrderViewModel>();
            InputDetail parentInputDetail = UnitOfWork.InputDetailsRepository.GetById(inputDetailId);
            result.Add(new KardexChildOrderViewModel {
                OrderCode = parentInputDetail.Order.Code,
                OrderCustomer = parentInputDetail.Order.Customer.FullName,
                OrderId = parentInputDetail.OrderId.Value,
                InitialQuantity = parentInputDetail.RemainQuantity.ToString(),
                InitialWeight = parentInputDetail.RemainDestinationWeight.ToString(),
                InputDetailId = parentInputDetail.Id,
                InputDetailStatus = parentInputDetail.InputDetailStatus.Title,
                IssuedQuantity = "0",
                IssuedWeight = "0",
                CreationDate=parentInputDetail.CreationDate.ToString("yyyy/MM/dd")
            });

            List<InputDetail> childeren = UnitOfWork.InputDetailsRepository.Get(current => current.ParentId == parentInputDetail.Id).ToList();
            foreach (var item in childeren)
            {
                result.Add(new KardexChildOrderViewModel
                {
                    OrderCode = item.Order.Code,
                    OrderCustomer = item.Order.Customer.FullName,
                    OrderId = item.OrderId.Value,
                    InitialQuantity = "0",
                    InitialWeight = "0",
                    InputDetailId = item.Id,
                    InputDetailStatus = item.InputDetailStatus.Title,
                    IssuedQuantity = item.RemainQuantity.ToString(),
                    IssuedWeight = item.RemainDestinationWeight.ToString(),
                    CreationDate=item.CreationDate.ToString("yyyy/MM/dd")
                });
                List<InputDetail> childList = UnitOfWork.InputDetailsRepository.Get(current => current.ParentId == item.Id).ToList();
                foreach (var child in childList)
                {
                    result.Add(new KardexChildOrderViewModel
                    {
                        OrderCode = child.Order.Code,
                        OrderCustomer = child.Order.Customer.FullName,
                        OrderId = child.OrderId.Value,
                        InitialQuantity = "0",
                        InitialWeight = "0",
                        InputDetailId = child.Id,
                        InputDetailStatus = child.InputDetailStatus.Title,
                        IssuedQuantity = child.RemainQuantity.ToString(),
                        IssuedWeight = child.RemainDestinationWeight.ToString(),
                        CreationDate=child.CreationDate.ToString("yyyy/MM/dd")
                    });
                }
            }
            return result;
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
                    Description = desc,
                    WeighbridgeAmount = 0,
                    InventoryAmount = 0,
                    CutAmount = 0,
                    LoadAmount = 0,
                    RemainAmount = 0,
                    OtherAmount = 0,
                    SubTotalAmount = 0,
                    TotalAmount = 0,
                    PaymentAmount = 0
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
                Guid status = UnitOfWork.InputDetailStatusRepository.Get(c => c.Code == 2).FirstOrDefault().Id;
                if (weightMain < inputDetail.RemainDestinationWeight || quantity < inputDetail.RemainQuantity)
                {
                    status = UnitOfWork.InputDetailStatusRepository.Get(c => c.Code == 4).FirstOrDefault().Id;
                }
                inputDetail.RemainQuantity = inputDetail.RemainQuantity - quantity;
                inputDetail.RemainDestinationWeight = inputDetail.RemainDestinationWeight - weightMain;
                inputDetail.InputDetailStatusId = status;
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

