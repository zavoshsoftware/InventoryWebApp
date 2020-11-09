using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Reports.Models
{
    public class InvoiceReportViewModel
    {
        public string Date { get; set; }
        public string SellerName { get; set; }
        public string SellerEcoNumber { get; set; }
        public string SellerNationamNumber { get; set; }
        public string SellerAddress { get; set; }
        public string SellerPhone { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerNationamNumber { get; set; }
        public string CustomerEcoNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCarNumber { get; set; }
        public string OrderCode { get; set; }
        public string Weight { get; set; }
        public string InventoryAmount { get; set; }
        public string CutAmount { get; set; }
        public string LoadAmount { get; set; }
        public string WeightBridgeAmount { get; set; }
        public string OtherAmount { get; set; }
        public string SubTotal { get; set; }
        public string Vat { get; set; }
        public string Total { get; set; }
        public string PaymentAmount { get; set; }
        public string RemainAmount { get; set; }
        public IList<InvoiceProductViewModel> Products { get; set; }
    }

    public class InvoiceProductViewModel
    {
        public string Order { get; set; }
        public string ProductTitle { get; set; }
        public string Weight { get; set; }
        public string Unit { get; set; }
        public string Amount { get; set; }
    }
}