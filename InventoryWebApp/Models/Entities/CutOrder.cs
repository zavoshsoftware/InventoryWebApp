using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class CutOrder:BaseEntity
    {
        public CutOrder()
        {
            CutOrderDetails = new List<CutOrderDetail>();
        }
        public Guid CutTypeId { get; set; }
        public virtual CutType CutType { get; set; }
        public Guid InputDetailId { get; set; }
        public virtual InputDetail InputDetail { get; set; }
        public virtual ICollection<CutOrderDetail> CutOrderDetails { get; set; }
    }
}
