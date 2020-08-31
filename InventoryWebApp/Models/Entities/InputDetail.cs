using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class InputDetail : BaseEntity
    {
       
        [Display(Name = "InputId", ResourceType = typeof(Resources.Models.InputDetail))]
        public Guid InputId { get; set; }

        public virtual Input Input { get; set; }

        [Display(Name = "Title", ResourceType = typeof(Resources.Models.Product))]
        public Guid ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Display(Name = "Code", ResourceType = typeof(Resources.Models.InputDetail))]
        public string Code { get; set; }

        [Display(Name = "Quantity", ResourceType = typeof(Resources.Models.InputDetail))]
        public int Quantity { get; set; }

        [Display(Name = "DestinationWeight", ResourceType = typeof(Resources.Models.InputDetail))]
        public decimal DestinationWeight { get; set; }

        [Display(Name = "SourceWeight", ResourceType = typeof(Resources.Models.InputDetail))]
        public decimal SourceWeight { get; set; }
    }
}
