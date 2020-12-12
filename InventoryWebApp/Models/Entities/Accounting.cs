using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class Accounting:BaseEntity
    {
        [Display(Name = "Code", ResourceType = typeof(Resources.Models.Accounting))]
        public int Code { get; set; }
        [Display(Name = "Title", ResourceType = typeof(Resources.Models.Accounting))]
        public string Title { get; set; }
        [Display(Name = "Bedehkar", ResourceType = typeof(Resources.Models.Accounting))]
        public decimal Bedehkar { get; set; }
        [Display(Name = "Bestankar", ResourceType = typeof(Resources.Models.Accounting))]
        public decimal Bestankar { get; set; }
        [Display(Name = "CustomerId", ResourceType = typeof(Resources.Models.Accounting))]
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

    }
}
