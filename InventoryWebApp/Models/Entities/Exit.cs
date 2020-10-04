using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Exit : BaseEntity
    {
        public Exit()
        {
            ExitDetails = new List<ExitDetail>();
        }

        [Display(Name = "Code", ResourceType = typeof(Resources.Models.Input))]
        public int Code { get; set; }

        [Display(Name = "InputDate", ResourceType = typeof(Resources.Models.Input))]
        public DateTime ExitDate { get; set; }

        [Display(Name = "FullName", ResourceType = typeof(Resources.Models.Customer))]
        public Guid? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }


        public virtual ICollection<ExitDetail> ExitDetails { get; set; }
        public bool IsOpen { get; set; }

}
}
