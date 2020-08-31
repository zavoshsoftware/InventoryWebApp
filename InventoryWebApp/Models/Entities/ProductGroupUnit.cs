using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductGroupUnit : BaseEntity
    {
        public ProductGroupUnit()
        {
            ProductGroups = new List<ProductGroup>();
        }
        [Display(Name = "Title", ResourceType = typeof(Resources.Models.ProductGroupUnit))]
        public string Title { get; set; }

         public virtual ICollection<ProductGroup> ProductGroups { get; set; }
    }
}
