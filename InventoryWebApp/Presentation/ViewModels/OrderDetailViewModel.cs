using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class OrderDetailViewModel
    {
        public Guid OrderId { get; set; }
        public string ParentOrderCode { get; set; }
        public string ParentOrderCustomer { get; set; }
        public string ChildOrderCode { get; set; }
        public string ChildOrderCustomer { get; set; }
        public List<ProductInfoViewModel> Products { get; set; }

        public List<ChildOrderViewModel> ChildOrders { get; set; }
    }

    public class ProductInfoViewModel
    {
        public Guid InputDetailId { get; set; }
        public Guid ProductId { get; set; }
        public string ProductTitle { get; set; }

        public string InitialQty { get; set; }
     
        public string RemainQty { get; set; }

        public string InitialWeight { get; set; }
       
        public string RemainWeight { get; set; }
        public string InputDetailStatusTitle { get; set; }
    }

    public class ChildOrderViewModel
    {
        public Guid OrderId { get; set; }
        public string OrderCode { get; set; }
        public string OrderCustomer { get; set; }
        public string ProducTitle { get; set; }
        public string Weight { get; set; }
        public string Quantity { get; set; }
    }
}