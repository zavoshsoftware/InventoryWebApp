using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class ManageConfiguration:BaseEntity
    {
        [Display(Name = "VAT", ResourceType = typeof(Resources.Models.ManageConfiguration))]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal VAT { get; set; }
        [Display(Name = "Amount", ResourceType = typeof(Resources.Models.ManageConfiguration))]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal Amount { get; set; }
    }
}
