using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class InputDetailParentsViewModel
    {
        public string InputCode { get; set; }
        public string InputDate { get; set; }
        public int Quantity { get; set; }
        public decimal DestinationWeight { get; set; }
        public decimal SourceWeight { get; set; }
    }
}