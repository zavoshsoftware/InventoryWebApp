using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class CutDetailType:BaseEntity
    {
        public CutDetailType()
        {
            CutOrderDetails = new List<CutOrderDetail>();
        }
        [Display(Name = "Title", ResourceType = typeof(Resources.Models.Cut))]
        public string Title { get; set; }
        public virtual ICollection<CutOrderDetail> CutOrderDetails { get; set; }
    }
}
