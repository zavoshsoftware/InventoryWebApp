using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class Payment:BaseEntity
    {
        [Display(Name = "ExitId", ResourceType = typeof(Resources.Models.Customer))]
        public Guid? ExitId { get; set; }
        public Exit Exit { get; set; }

        [Display(Name = "InventoryAmount", ResourceType = typeof(Resources.Models.Customer))]
        public decimal InventoryAmount { get; set; }

        [Display(Name = "CutAmount", ResourceType = typeof(Resources.Models.Customer))]
        public decimal CutAmount { get; set; }

        [Display(Name = "LoadAmount", ResourceType = typeof(Resources.Models.Customer))]
        public decimal LoadAmount { get; set; }

        [Display(Name = "VatAmount", ResourceType = typeof(Resources.Models.Customer))]
        public decimal VatAmount { get; set; }

        [Display(Name = "TotalAmount", ResourceType = typeof(Resources.Models.Customer))]
        public decimal TotalAmount { get; set; }
    }
}
