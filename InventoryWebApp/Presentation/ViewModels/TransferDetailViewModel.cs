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
        public int RemainQuantity { get; set; }

        public decimal UnitWeight
        {
            get { return RemainWight / RemainQuantity; }
        }

    }

   
}