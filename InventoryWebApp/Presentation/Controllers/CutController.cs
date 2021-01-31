using Helper;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ViewModels;

namespace Presentation.Controllers
{
    public class CutController : Infrastructure.BaseControllerWithUnitOfWork
    {
        // GET: Cut
        public ActionResult Index()
        {
            var orders = UnitOfWork.OrderRepository.Get().OrderByDescending(o => o.CreationDate);
            return View(orders.ToList());
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
            ViewBag.CutTypeId = new SelectList(UnitOfWork.CutTypeRepository.Get(), "Id", "Title");
            return View(exit);
        }

        [HttpPost]
        public ActionResult PostCutOrder(string cutTypeId, string inputDetailId)
        {
            try
            {
                string message = "message-";
                Guid cutTypeIdGuid = new Guid(cutTypeId);
                Guid inputDetailIdGuid = new Guid(inputDetailId);
                CutOrder cutOrder = new CutOrder()
                {
                    CreationDate = DateTime.Now,
                    CutTypeId = cutTypeIdGuid,
                    InputDetailId = inputDetailIdGuid,
                    IsActive = true,
                    IsDeleted = false
                };
                UnitOfWork.CutOrderRepository.Insert(cutOrder);
                UnitOfWork.Save();
                //return RedirectToAction("Details", new { id = cutOrder.Id });
                return Json(cutOrder.Id, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Details(Guid id)
        {
            CutOrderViewModel CutOrderViewModel = new CutOrderViewModel();
            InputDetail inputDetail = UnitOfWork.InputDetailsRepository.GetById(id);
            CutOrderViewModel.InputDetail = inputDetail;
            CutOrderViewModel.Density = inputDetail.Product.ProductGroup.Density;
            CutOrderViewModel.Weight = GetOrderRemainWeight(inputDetail.OrderId.Value);
            CutOrder cutOrder = UnitOfWork.CutOrderRepository.Get(current => current.InputDetailId == inputDetail.Id).FirstOrDefault();
            if (cutOrder != null)
            {
                CutOrderViewModel.CutOrderId = cutOrder.Id;
                CutOrderViewModel.CutOrderDetails = UnitOfWork.CutOrderDetailRepository.Get(current => current.CutOrderId == cutOrder.Id).ToList();
            }
            ViewBag.CustomActionId = new SelectList(UnitOfWork.ProductGroupCustomActionRepository.Get(x => x.ProductGroupId == inputDetail.Product.ProductGroupId).Select(x => x.CustomAction), "Id", "Title");
            ViewBag.CustomerName = inputDetail.Order.Customer.FullName;
            CutOrderViewModel.RemainWeight = GetOrderRemainWeight(inputDetail.OrderId.Value);
            CutOrderViewModel.Math = inputDetail.Product.ProductGroup.Density * inputDetail.Product.Width * inputDetail.Product.Thickness;
            //CutOrder cutOrder = UnitOfWork.CutOrderRepository.GetById(id);
            //CutOrderViewModel CutOrderViewModel = new CutOrderViewModel();
            ////CutOrderViewModel.CutOrder = cutOrder;
            //CutOrderViewModel.Density = cutOrder.InputDetail.Product.ProductGroup.Density;
            //CutOrderViewModel.CutOrderId = cutOrder.Id;
            //CutOrderViewModel.InputDetail = UnitOfWork.InputDetailsRepository.GetById(cutOrder.InputDetailId);
            //CutOrderViewModel.CutOrderDetails = UnitOfWork.CutOrderDetailRepository.Get(current => current.CutOrderId == id).ToList();
            //ViewBag.CustomActionId = new SelectList(UnitOfWork.CutDetailTypeRepository.Get().ToList(), "Id", "Title");

            return View(CutOrderViewModel);
        }

        [HttpPost]
        public ActionResult Details(CutOrderViewModel cutOrderViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    cutOrderViewModel.InputDetail = UnitOfWork.InputDetailsRepository.GetById(cutOrderViewModel.InputDetail.Id);

                    Order inputDetailOrder = UnitOfWork.OrderRepository.Get(current => current.Id == cutOrderViewModel.InputDetail.OrderId).FirstOrDefault();
                    cutOrderViewModel.InputDetail.Order = inputDetailOrder;
                    ProductRemainsViewModel remain = GetRemainProduct(cutOrderViewModel.InputDetail.ProductId, cutOrderViewModel.InputDetail.OrderId.Value, cutOrderViewModel.InputDetail.Order.CustomerId);

                    string cutId = string.Empty;
                    if (cutOrderViewModel.CutOrderId != null)
                    {
                        cutId = cutOrderViewModel.CutOrderId.ToString();
                        Guid cutGuid = new Guid(cutId);
                        cutOrderViewModel.CutOrderDetails = UnitOfWork.CutOrderDetailRepository.Include(current => current.CutOrder.InputDetail.Product.ProductGroup).Where(current => current.CutOrderId == cutGuid).ToList();

                    }
                    if (remain.RemainWeight < cutOrderViewModel.Weight)
                    {
                        ModelState.AddModelError("Weight", "وزن وارد شده بیش از وزن باقی مانده می باشد");
                    }
                    else if (remain.RemainQuantity < cutOrderViewModel.Quantity)
                    {
                        ModelState.AddModelError("Weight", "تعداد برگ وارد شده بیش از تعداد باقی مانده می باشد");
                    }
                    else if (remain.RemainWeight == 0)
                    {
                        ModelState.AddModelError("Weight", "کالا موجود نمی باشد");
                    }
                    else if (remain.RemainWeight == cutOrderViewModel.Weight)
                    {

                        if (cutOrderViewModel.CutOrderId == null)
                        {
                            CutOrder cut = new CutOrder()
                            {
                                CreationDate = DateTime.Now,
                                InputDetailId = cutOrderViewModel.InputDetail.Id,
                                IsActive = true,
                                IsDeleted = false
                            };
                            UnitOfWork.CutOrderRepository.Insert(cut);
                            UnitOfWork.Save();
                            cutId = cut.Id.ToString();
                        }
                        Guid cutGuid = new Guid(cutId);
                        cutOrderViewModel.CutOrderId = cutGuid;
                        CutOrderDetail cutOrderDetail = new CutOrderDetail()
                        {
                            Weight = cutOrderViewModel.Weight,
                            Length = cutOrderViewModel.Length,
                            Quantity = Convert.ToInt32(cutOrderViewModel.Quantity),
                            CutOrderId = cutGuid,
                            CustomActionId = cutOrderViewModel.CustomActionId,
                            CreationDate = DateTime.Now,
                            IsActive = true,
                            IsDeleted = false
                        };
                        UnitOfWork.CutOrderDetailRepository.Insert(cutOrderDetail);
                        UnitOfWork.Save();

                        cutOrderViewModel.CutOrderDetails = UnitOfWork.CutOrderDetailRepository.Include(current => current.CutOrder.InputDetail.Product.ProductGroup).Where(current => current.CutOrderId == cutOrderViewModel.CutOrderId).ToList();

                        InputDetail inputDetailUpdate = UnitOfWork.InputDetailsRepository.GetById(cutOrderViewModel.InputDetail.Id);
                        inputDetailUpdate.RemainDestinationWeight = 0;
                        inputDetailUpdate.RemainQuantity = 0;
                        UnitOfWork.InputDetailsRepository.Update(inputDetailUpdate);
                        UnitOfWork.Save();
                        cutOrderViewModel.RemainWeight = inputDetailUpdate.RemainDestinationWeight;
                    }
                    else
                    {
                        if (cutOrderViewModel.CutOrderId != null)
                        {
                            Order order = new Order()
                            {
                                Customer = cutOrderViewModel.InputDetail.Order.Customer,
                                Parent = cutOrderViewModel.InputDetail.Order,
                                Code = GenerateCode.GetChildOrderCode(cutOrderViewModel.InputDetail.OrderId.Value),
                                IsActive = true,
                                IsDeleted = false,
                                IsLatest = true
                            };
                            UnitOfWork.OrderRepository.Insert(order);
                            UnitOfWork.Save();

                            CutOrderDetail cutOrderDetail = new CutOrderDetail()
                            {
                                Weight = cutOrderViewModel.Weight,
                                Length = cutOrderViewModel.Length,
                                Quantity = Convert.ToInt32(cutOrderViewModel.Quantity),
                                CutOrderId = cutOrderViewModel.CutOrderId.Value,
                                CustomActionId = cutOrderViewModel.CustomActionId,
                                CreationDate = DateTime.Now,
                                IsActive = true,
                                IsDeleted = false
                            };
                            UnitOfWork.CutOrderDetailRepository.Insert(cutOrderDetail);
                            UnitOfWork.Save();
                        }
                        else
                        {
                            Order order = new Order()
                            {
                                Customer = cutOrderViewModel.InputDetail.Order.Customer,
                                Parent = cutOrderViewModel.InputDetail.Order,
                                Code = GenerateCode.GetChildOrderCode(cutOrderViewModel.InputDetail.OrderId.Value),
                                IsActive = true,
                                IsDeleted = false,
                                IsLatest = true
                            };
                            UnitOfWork.OrderRepository.Insert(order);
                            UnitOfWork.Save();

                            CutOrder cut = new CutOrder()
                            {
                                CreationDate = DateTime.Now,
                                InputDetailId = cutOrderViewModel.InputDetail.Id,
                                IsActive = true,
                                IsDeleted = false
                            };
                            UnitOfWork.CutOrderRepository.Insert(cut);
                            UnitOfWork.Save();

                            cutOrderViewModel.CutOrderId = cut.Id;
                            CutOrderDetail cutOrderDetail = new CutOrderDetail()
                            {
                                Weight = cutOrderViewModel.Weight,
                                Length = cutOrderViewModel.Length,
                                Quantity = Convert.ToInt32(cutOrderViewModel.Quantity),
                                CutOrderId = cut.Id,
                                CustomActionId = cutOrderViewModel.CustomActionId,
                                CreationDate = DateTime.Now,
                                IsActive = true,
                                IsDeleted = false
                            };
                            UnitOfWork.CutOrderDetailRepository.Insert(cutOrderDetail);
                            UnitOfWork.Save();

                        }
                        cutOrderViewModel.CutOrderDetails = UnitOfWork.CutOrderDetailRepository.Include(current => current.CutOrder.InputDetail.Product.ProductGroup).Where(current => current.CutOrderId == cutOrderViewModel.CutOrderId).ToList();


                        decimal unit = remain.RemainWeight / remain.RemainQuantity;
                        InputDetail inputDetailUpdate = UnitOfWork.InputDetailsRepository.GetById(cutOrderViewModel.InputDetail.Id);
                        inputDetailUpdate.RemainDestinationWeight = remain.RemainWeight - cutOrderViewModel.Weight;
                        inputDetailUpdate.RemainQuantity = remain.RemainQuantity - (cutOrderViewModel.Weight / unit);
                        UnitOfWork.InputDetailsRepository.Update(inputDetailUpdate);
                        UnitOfWork.Save();
                        cutOrderViewModel.RemainWeight = inputDetailUpdate.RemainDestinationWeight;
                    }
                }

                ViewBag.CustomActionId = new SelectList(UnitOfWork.ProductGroupCustomActionRepository.Get(x => x.ProductGroupId == cutOrderViewModel.InputDetail.Product.ProductGroupId).Select(x => x.CustomAction), "Id", "Title");
                ViewBag.CustomerName = cutOrderViewModel.InputDetail.Order.Customer.FullName;
                return View(cutOrderViewModel);
            }
            catch (Exception exp)
            {

                return Json(exp.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetCutAmount(string id, string productGroupId)
        {
            try
            {
                if (id == null || productGroupId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Guid customActionId = new Guid(id);
                Guid productGroupGuid = new Guid(productGroupId);
                ProductGroupCustomAction productGroupCustomAction = UnitOfWork.ProductGroupCustomActionRepository.Get(
                    current => current.ProductGroupId == productGroupGuid && current.CustomActionId == customActionId).FirstOrDefault();
                if (productGroupCustomAction == null)
                {
                    return HttpNotFound();
                }
                return Json(productGroupCustomAction.Amount, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exp)
            {
                return Json(exp.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public decimal GetOrderRemainWeight(Guid orderId)
        {
            List<ProductInfoViewModel> result = new List<ProductInfoViewModel>();

            List<InputDetail> inputDetails = UnitOfWork.InputDetailsRepository
                .Get(c => c.OrderId == orderId).ToList();


            decimal remainWeight = 0;

            foreach (InputDetail inputDetail in inputDetails)
            {

                remainWeight += inputDetail.RemainDestinationWeight;

            }



            return remainWeight;
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


    }
}