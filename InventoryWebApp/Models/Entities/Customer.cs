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
        }
        [Display(Name = "FullName", ResourceType = typeof(Resources.Models.Customer))]
        public string FullName { get; set; }

        public virtual ICollection<Input> Inputs { get; set; }
    }
}
