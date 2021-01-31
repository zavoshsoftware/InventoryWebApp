using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class CutOrderDetail:BaseEntity
    {
        public Guid CutOrderId { get; set; }
        public virtual CutOrder CutOrder { get; set; }
        [Display(Name = "CutDetailTypeId", ResourceType = typeof(Resources.Models.Cut))]
        public Guid CustomActionId { get; set; }
        public virtual CustomAction CustomAction { get; set; }

        [Display(Name = "Weight", ResourceType = typeof(Resources.Models.Cut))]
        public decimal Weight { get; set; }

        [Display(Name = "Length", ResourceType = typeof(Resources.Models.Cut))]
        public decimal Length { get; set; }

        [Display(Name = "Quantity", ResourceType = typeof(Resources.Models.Cut))]
        public int Quantity { get; set; }
    }
}
