using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class InputDetailStatus : BaseEntity
    {
        public InputDetailStatus()
        {
            InputDetails=new List<InputDetail>();
        }

        public string Title { get; set; }
        public int Code { get; set; }

        public virtual ICollection<InputDetail> InputDetails { get; set; }
    }
}
