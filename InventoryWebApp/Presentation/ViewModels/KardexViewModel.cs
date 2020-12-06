using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class KardexViewModel
    {
        public Guid OrderId { get; set; }
        public string ParentOrderCode { get; set; }
        public string ParentOrderCustomer { get; set; }
        public string OrderProductName { get; set; }
        public List<KardexChildOrderViewModel> ChildOrders { get; set; }
      

    }

    public class KardexChildOrderViewModel
    {
        public Guid OrderId { get; set; }
        public string OrderCode { get; set; }
        public string OrderCustomer { get; set; }
        public string InitialWeight { get; set; }
        public string InitialQuantity { get; set; }
        public string IssuedWeight { get; set; }
        public string IssuedQuantity { get; set; }
        public string InputDetailStatus { get; set; }
        public Guid InputDetailId { get; set; }
        public string CreationDate { get; set; }

    }
}