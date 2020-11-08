using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;
using System.ComponentModel.DataAnnotations;

namespace ViewModels
{
    public class InputDetailViewModel
    {
        public Input Input { get; set; }
        public List<InputDetail> InputDetails { get; set; }
        public InputDetail Detail { get; set; }
        [Display(Name ="نام کالا")]
        public Guid ProductId { get; set; }
        public bool EditMode { get; set; }

        [Display(Name ="شماره حواله")]
        public Guid OrderId { get; set; }
    }
}