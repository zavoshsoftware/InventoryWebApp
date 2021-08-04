using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Customer : BaseEntity
    {
        public Customer()
        {
            Inputs=new List<Input>();
            Orders = new List<Order>();
        }
        [Display(Name = "FullName", ResourceType = typeof(Resources.Models.Customer))]
        public string FullName { get; set; }
        [Display(Name = "CellNumber", ResourceType = typeof(Resources.Models.Customer))]
        public string CellNumber { get; set; }
        [Display(Name = "Address", ResourceType = typeof(Resources.Models.Customer))]
        public string Address { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Input> Inputs { get; set; }
    }
}
