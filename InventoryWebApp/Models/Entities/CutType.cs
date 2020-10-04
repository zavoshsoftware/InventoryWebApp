using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
  public class CutType:BaseEntity
    {
        public CutType()
        {
            CutOrders = new List<CutOrder>();
        }
        [Display(Name = "Title", ResourceType = typeof(Resources.Models.Cut))]
        public string Title { get; set; }
        public virtual ICollection<CutOrder> CutOrders { get; set; }
    }
}
