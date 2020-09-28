using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class ExitInputDetailViewModel
    {
        public Guid ChildOrderId { get; set; }
        public string ChildOrderCode { get; set; }
        public string ParentOrderCode { get; set; }
        public string ParentCustomerName { get; set; }
        public string ChildCustomerName { get; set; }
 
        public List<InputDetailTransferViewModel> InputDetails { get; set; }
    }
}