using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ViewModels
{
    public class CutOrderViewModel
    {
        public CutOrderViewModel()
        {
            CutOrderDetails = new List<CutOrderDetail>();
        }
        //public CutOrder CutOrder { get; set; }
        public InputDetail InputDetail { get; set; }
        public List<CutOrderDetail> CutOrderDetails { get; set; }
        public  CutOrderDetail CutOrderDetail { get; set; }
        [Display(Name ="وزن")]
        public decimal Weight { get; set; }
        [Display(Name = "وزن مانده")]
        public decimal RemainWeight { get; set; }
        [Display(Name = "متراژ")]
        public decimal Length { get; set; }
        [Display(Name = "چگالی")]
        public decimal Density { get; set; }
        [Display(Name = "برگ")]
        public decimal Quantity { get; set; }
        [Display(Name = "نوع برش")]
        public Guid CustomActionId { get; set; }
        public Guid? CutOrderId { get; set; }
        public decimal Math { get; set; }
    }
}