using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductCreator : BaseEntity
    {
        public ProductCreator()
        {
            Products = new List<Product>();
        }
        [Display(Name = "Title", ResourceType = typeof(Resources.Models.ProductCreator))]
        public string Title { get; set; }

         public virtual ICollection<Product> Products { get; set; }
    }
}
