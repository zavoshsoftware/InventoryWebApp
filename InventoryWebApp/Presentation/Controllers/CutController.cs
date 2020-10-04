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
            CutOrder cutOrder = UnitOfWork.CutOrderRepository.GetById(id);
            CutOrderViewModel CutOrderViewModel = new CutOrderViewModel();
            //CutOrderViewModel.CutOrder = cutOrder;
            CutOrderViewModel.Density = cutOrder.InputDetail.Product.ProductGroup.Density;
            CutOrderViewModel.CutOrderId = cutOrder.Id;
            CutOrderViewModel.InputDetail = UnitOfWork.InputDetailsRepository.GetById(cutOrder.InputDetailId);
            CutOrderViewModel.CutOrderDetails = UnitOfWork.CutOrderDetailRepository.Get(current => current.CutOrderId == id).ToList();
            ViewBag.CutDetailTypeId = new SelectList(UnitOfWork.CutDetailTypeRepository.Get().ToList(), "Id", "Title");

            return View(CutOrderViewModel);
        }

        [HttpPost]
        public ActionResult Details(CutOrderViewModel cutOrderViewModel)
        {
            if (ModelState.IsValid)
            {
                int quantity = Convert.ToInt32(cutOrderViewModel.Weight / cutOrderViewModel.Density / cutOrderViewModel.Length);
                CutOrderDetail cutOrderDetail = new CutOrderDetail()
                {
                    Weight = cutOrderViewModel.Weight,
                    Length = cutOrderViewModel.Length,
                    Quantity = quantity,
                    CutOrderId = cutOrderViewModel.CutOrderId,
                    CutDetailTypeId = cutOrderViewModel.CutDetailTypeId,
                    CreationDate=DateTime.Now,
                    IsActive=true,
                    IsDeleted=false
                };
                UnitOfWork.CutOrderDetailRepository.Insert(cutOrderDetail);
                UnitOfWork.Save();
               
            }
            CutOrder cutOrder = UnitOfWork.CutOrderRepository.GetById(cutOrderViewModel.CutOrderId);
            cutOrderViewModel.InputDetail = cutOrder.InputDetail;
            cutOrderViewModel.CutOrderDetails = UnitOfWork.CutOrderDetailRepository.Get(current => current.CutOrderId == cutOrder.Id).ToList();
            ViewBag.CutDetailTypeId = new SelectList(UnitOfWork.CutDetailTypeRepository.Get().ToList(), "Id", "Title");
            return View(cutOrderViewModel);
        }
    }
}