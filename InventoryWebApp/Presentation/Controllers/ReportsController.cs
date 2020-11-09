using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Reports.Models;
using Stimulsoft.Report;
using Stimulsoft.Report.Mvc;
using System.Data.Entity;

namespace Presentation.Controllers
{
    public class ReportsController : Infrastructure.BaseControllerWithUnitOfWork
    {
        public ActionResult Invoice(Guid id)
        {
            TempData["id"] = id;
            return View();
        }

        #region InvoiceReport

        public ActionResult LoadInvoiceReportSnapshot()
        {
            Guid exitId = new Guid(TempData["id"].ToString());

            var report = new StiReport();
            report.Load(Server.MapPath("~/Reports/MRT/InvoiceReport.mrt"));
            report.RegBusinessObject("InvoiceReport", GetInvoice(exitId));
            //  report.Dictionary.Variables.Add("today", DateTime.Today());
            return StiMvcViewer.GetReportResult(report);
        }

        public virtual ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }
        public virtual ActionResult PrintReport()
        {
            return StiMvcViewer.PrintReportResult();
        }

        public virtual ActionResult ExportReport()
        {
            return StiMvcViewer.ExportReportResult();
        }

        public InvoiceReportViewModel GetInvoice(Guid exitId)
        {
            Exit exit = UnitOfWork.ExitRepository.GetById(exitId);


            if (exit != null)
            {
                string paymentAmount = "0";
                if (exit.PaymentAmount != null)
                    paymentAmount = exit.PaymentAmount.Value.ToString("N0");


                string remainAmount = "0";
                if (exit.RemainAmount != null)
                    remainAmount = exit.RemainAmount.Value.ToString("N0");


                InvoiceReportViewModel invoice = new InvoiceReportViewModel()
                {
                    Date = DateTime.Now.ToShortDateString(),
                    SellerName = "انبار داریوش",
                    SellerEcoNumber = "",
                    SellerNationamNumber = "1189084910",
                    SellerAddress = "تهران، ابتدای جاده قدیم قم، بعد از سه راهی ترانسفو",
                    SellerPhone = "02155208921-6",
                    CustomerName = exit.Customer.FullName,
                    CustomerCarNumber = exit.CarNumber,
                    Products = GetProductList(exit),
                    SubTotal = exit.SubTotalAmount.Value.ToString("N0"),
                    Total = exit.TotalAmount.Value.ToString("N0"),
                    Vat = exit.Vat.Value.ToString("N0"),
                    PaymentAmount = paymentAmount,
                    RemainAmount = remainAmount,
                };

                return invoice;
            }

            return new InvoiceReportViewModel();
        }

        public List<InvoiceProductViewModel> GetProductList(Exit exit)
        {
            ExitDetail exitDetail = UnitOfWork.ExitDetailRepository.Get(c => c.ExitId == exit.Id).FirstOrDefault();

            List<InvoiceProductViewModel> result = new List<InvoiceProductViewModel>();

            result.Add(new InvoiceProductViewModel()
            {
                Order = "1",
                ProductTitle = "انبار داری حواله " + exitDetail.InputDetail.Order.Code,
                Weight = exitDetail.PureWeight.ToString().Split('/')[0],
                Unit = exitDetail.InputDetail.Product.ProductGroup.ProductGroupUnit.Title,
                Amount = exitDetail.Exit.InventoryAmount.Value.ToString("N0")
            });


            result.Add(new InvoiceProductViewModel()
            {
                Order = "2",
                ProductTitle = "برش حواله ",
                Weight = "0",
                Unit = exitDetail.InputDetail.Product.ProductGroup.ProductGroupUnit.Title,
                Amount = "0"
            });



            result.Add(new InvoiceProductViewModel()
            {
                Order = "3",
                ProductTitle = "بارگیری حواله " + exitDetail.InputDetail.Order.Code,
                Weight = exitDetail.PureWeight.ToString().Split('/')[0],
                Unit = exitDetail.InputDetail.Product.ProductGroup.ProductGroupUnit.Title,
                Amount = exitDetail.Exit.LoadAmount.Value.ToString("N0")
            });


            result.Add(new InvoiceProductViewModel()
            {
                Order = "4",
                ProductTitle = "باسکول حواله " + exitDetail.InputDetail.Order.Code,
                Weight = exitDetail.PureWeight.ToString().Split('/')[0],
                Unit = exitDetail.InputDetail.Product.ProductGroup.ProductGroupUnit.Title,
                Amount = exitDetail.Exit.WeighbridgeAmount.Value.ToString("N0")
            });


            result.Add(new InvoiceProductViewModel()
            {
                Order = "5",
                ProductTitle = "متفرقه ",
                Weight = "",
                Unit = "",
                Amount = "0"
            });




            return result;
        }

        #endregion


        public ActionResult Exit(Guid id)
        {
            TempData["exitId"] = id;
            return View();
        }

        #region InvoiceReport

        public ActionResult LoadExitReportSnapshot()
        {
            Guid exitId = new Guid(TempData["exitId"].ToString());

            var report = new StiReport();
            report.Load(Server.MapPath("~/Reports/MRT/ExitReport.mrt"));
            report.RegBusinessObject("ExitReport", GetExit(exitId));
            //  report.Dictionary.Variables.Add("today", DateTime.Today());
            return StiMvcViewer.GetReportResult(report);
        }



        public ExitReportViewModel GetExit(Guid exitId)
        {
            Exit exit = UnitOfWork.ExitRepository.GetById(exitId);


            if (exit != null)
            {
                string paymentAmount = "0";
                if (exit.PaymentAmount != null)
                    paymentAmount = exit.PaymentAmount.Value.ToString("N0");


                string remainAmount = "0";
                if (exit.RemainAmount != null)
                    remainAmount = exit.RemainAmount.Value.ToString("N0");


                ExitReportViewModel result = new ExitReportViewModel()
                {
                    CustomerName = exit.Customer.FullName,

                    SubTotal = exit.SubTotalAmount.Value.ToString("N0"),
                    Total = exit.TotalAmount.Value.ToString("N0"),
                    Vat = exit.Vat.Value.ToString("N0"),
                    PaymentAmount = paymentAmount,
                    RemainAmount = remainAmount,
                    CarNumber = exit.CarNumber,
                    InventoryAmount = exit.InventoryAmount.Value.ToString("N0"),
                    LoadAmount = exit.LoadAmount.Value.ToString("N0"),
                    CutAmount = exit.CutAmount.Value.ToString("N0"),
                    OtherAmount = exit.OtherAmount.Value.ToString("N0"),
                    DriverName = exit.ExitDriver.FullName,
                    ExitCode = exit.Code.ToString(),
                    ExitDate = exit.CreationDateStr,
                    WeightBridgeAmount = exit.WeighbridgeAmount.Value.ToString("N0"),
                    ExitDetails = GetExitDetail(exit),

                };

                return result;
            }

            return new ExitReportViewModel();
        }

        public List<ExitDetailReportViewModel> GetExitDetail(Exit exit)
        {
            List<ExitDetail> exitDetails = UnitOfWork.ExitDetailRepository.Get(c => c.ExitId == exit.Id).ToList();


            List<ExitDetailReportViewModel> result = new List<ExitDetailReportViewModel>();


            foreach (ExitDetail exitDetail in exitDetails)
            {
                InputDetail inputDetail = UnitOfWork.InputDetailsRepository.GetById(exitDetail.InputDetailId);

                Order order = UnitOfWork.OrderRepository.GetById(inputDetail.OrderId.Value);

                string parentCustomerName = "";
                if (order.ParentId == null)
                    parentCustomerName = order.Customer.FullName;
                else
                    parentCustomerName = order.Parent.Customer.FullName;

                result.Add(new ExitDetailReportViewModel()
                {
                    OrderCode = inputDetail.Order.Code,
                    ProductTitle = inputDetail.Product.Title,
                    Quantity = exitDetail.InitialQuantity.ToString().Split('/')[0],
                    PureWeight = exitDetail.PureWeight.ToString().Split('/')[0],
                    FullWeight = exitDetail.FullWeight.ToString().Split('/')[0],
                    EmptyWeight = exitDetail.EmptyWeight.ToString().Split('/')[0],
                    ParentCustomerName = parentCustomerName
                });

              
            }



            return result;
        }

        #endregion
    }
}