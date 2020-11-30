using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductGroup : BaseEntity
    {
        public ProductGroup()
        {
           Products=new List<Product>();
            ProductGroupCustomActions = new List<ProductGroupCustomAction>();
        }
        [Display(Name = "Title", ResourceType = typeof(Resources.Models.ProductGroup))]
        public string Title { get; set; }

        [Display(Name = "Code", ResourceType = typeof(Resources.Models.ProductGroup))]
        public int Code { get; set; }

        [Display(Name = "InventoryAmount", ResourceType = typeof(Resources.Models.ProductGroup))]
        public decimal InventoryAmount { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Resources.Models.ProductGroupUnit))]
        public Guid ProductGroupUnitId { get; set; }
        public virtual ProductGroupUnit ProductGroupUnit { get; set; }

        [Display(Name = "Density", ResourceType = typeof(Resources.Models.ProductGroup))]
        public decimal Density { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public virtual ICollection<ProductGroupCustomAction> ProductGroupCustomActions { get; set; }
    }
}
