using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class ExitDriver:BaseEntity
    {
        public ExitDriver()
        {
            Exits=new List<Exit>();
        }
        public virtual ICollection<Exit> Exits { get; set; }
        [Display(Name = "FullName", ResourceType = typeof(Resources.Models.ExitDriver))]
        public string FullName { get; set; }
        
    }
}
