using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ProductForm : BaseEntity
    {
        public ProductForm()
        {
            Products=new List<Product>();
        }
        [Display(Name = "Title", ResourceType = typeof(Resources.Models.ProductForm))]
        public string Title { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
