using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using System.ComponentModel.DataAnnotations;

namespace ViewModels
{
    public class TransferViewModel
    {
        public Customer Customer { get; set; }
        public List<InputDetailTransferViewModel> InputDetails { get; set; }
    
    }

    public class InputDetailTransferViewModel
    {
        public Guid ProductId { get; set; }
        public Guid OrderId { get; set; }
        [Display(Name = "Code", ResourceType = typeof(Resources.Models.Order))]
        public string OrderCode { get; set; }
        [Display(Name = "RemainQuantity", ResourceType = typeof(Resources.Models.InputDetail))]
        public int RemainQuantity { get; set; }
        [Display(Name = "RemainDestinationWeight", ResourceType = typeof(Resources.Models.InputDetail))]
        public decimal RemainWeight { get; set; }
        [Display(Name = "Title", ResourceType = typeof(Resources.Models.Product))]
        public string ProductTitle { get; set; }
    }
}