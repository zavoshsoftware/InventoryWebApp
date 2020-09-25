using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace ViewModels
{
    public class TransferDetailViewModel
    {
        public string ProductTitle { get; set; }
        public string OrderCode { get; set; }
        public string CustomerFullName { get; set; }
        public decimal RemainWight { get; set; }
        public decimal RemainQuantity { get; set; }

        public string ParentOrderId { get; set; }
        public string ProductId { get; set; }

        public decimal UnitWeight
        {
            get { return RemainWight / RemainQuantity; }
        }

    }

   
}