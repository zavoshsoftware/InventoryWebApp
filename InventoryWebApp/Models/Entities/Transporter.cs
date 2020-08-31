using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Transporter : BaseEntity
    {
        public Transporter()
        {
            Inputs=new List<Input>();
        }

        [Display(Name = "Title", ResourceType = typeof(Resources.Models.Transporter))]
        public string Title { get; set; }

        public virtual ICollection<Input> Inputs { get; set; }
    }
}
