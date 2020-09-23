using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Models;
using ViewModels;

namespace Presentation.Controllers
{
    public class GridController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public ActionResult Hierarchy()
        {
            //List<TestViewModel> tests=new List<TestViewModel>();

            //List<Order> orders = UnitOfWork.OrderRepository.Get().ToList();

            //foreach (Order order in orders)
            //{
            //    tests.Add(new TestViewModel()
            //    {
            //        Order = order,
            //        InputDetails = UnitOfWork.InputDetailsRepository.Get(c=>c.OrderId==order.Id).ToList()
            //    });
            //}

            return View(UnitOfWork.OrderRepository.Get().ToList());
        }

        public ActionResult HierarchyBinding_Orders([DataSourceRequest] DataSourceRequest request)
        {
            var orders = UnitOfWork.OrderRepository.Get().ToList();

            var result = new DataSourceResult()
            {
                Data = orders,
                Total = orders.Count()
            };


            return Json(result);

        }

        public ActionResult HierarchyBinding_InputDetail(Guid Id, [DataSourceRequest] DataSourceRequest request)
        {
            return Json(UnitOfWork.InputDetailsRepository.Get(c => c.OrderId == Id).ToList()
                .ToDataSourceResult(request));
        }
    }

   
}