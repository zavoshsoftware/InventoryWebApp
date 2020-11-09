using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class ExitAccountViewModel
    {
        public string InventoryAmount { get; set; }
        public string LoadAmount { get; set; }
        public string WeighbridgeAmount { get; set; }
        public string OtherAmount { get; set; }
        public string CutAmount { get; set; }
        public string Vat { get; set; }
        public string TotalAmount { get; set; }
    }
}