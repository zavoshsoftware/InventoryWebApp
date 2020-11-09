using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reports.Models
{
    public class ExitReportViewModel
    {
        public string ExitCode { get; set; }
        public string ExitDate { get; set; }
        public string CustomerName { get; set; }
        public string DriverName { get; set; }
        public string CarNumber { get; set; }
        public string CutAmount { get; set; }
        public string LoadAmount { get; set; }
        public string WeightBridgeAmount { get; set; }
        public string InventoryAmount { get; set; }
        public string OtherAmount { get; set; }
        public string SubTotal { get; set; }
        public string Vat { get; set; }
        public string Total { get; set; }
        public string PaymentAmount { get; set; }
        public string RemainAmount { get; set; }
        public IList<ExitDetailReportViewModel> ExitDetails { get; set; }
    }

    public class ExitDetailReportViewModel
    {
        public string OrderCode { get; set; }
        public string ProductTitle { get; set; }
        public string ParentCustomerName { get; set; }
        public string Quantity { get; set; }
        public string FullWeight { get; set; }
        public string EmptyWeight { get; set; }
        public string PureWeight { get; set; }

    }
}