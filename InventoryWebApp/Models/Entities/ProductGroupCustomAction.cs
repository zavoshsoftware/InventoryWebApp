using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class ProductGroupCustomAction:BaseEntity
    {
        [Display(Name = "Title", ResourceType = typeof(Resources.Models.CustomAction))]
        public Guid CustomActionId { get; set; }
        public CustomAction CustomAction { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Resources.Models.ProductGroup))]
        public Guid ProductGroupId { get; set; }
        public ProductGroup ProductGroup { get; set; }

        [Display(Name = "Amount", ResourceType = typeof(Resources.Models.CustomAction))]
        public decimal Amount { get; set; }
    }
}
