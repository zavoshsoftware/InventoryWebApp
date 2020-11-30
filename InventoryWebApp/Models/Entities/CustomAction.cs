using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class CustomAction:BaseEntity
    {
        public CustomAction()
        {
            ProductGroupCustomActions = new List<ProductGroupCustomAction>();
        }
        [Display(Name ="Title", ResourceType = typeof(Resources.Models.CustomAction))]
        public string Title { get; set; }

        public virtual ICollection<ProductGroupCustomAction> ProductGroupCustomActions { get; set; }
    }
}
