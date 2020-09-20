using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class InputDetail : BaseEntity
    {
        public InputDetail()
        {
            InputDetails=new List<InputDetail>();
        }
       
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


        public Guid? ParentId { get; set; }
        public virtual InputDetail Parent { get; set; }
        public virtual ICollection<InputDetail> InputDetails { get; set; }


        internal class configuration : EntityTypeConfiguration<InputDetail>
        {
            public configuration()
            {
                HasOptional(p => p.Parent).WithMany(t => t.InputDetails).HasForeignKey(p => p.ParentId);
                HasRequired(p => p.Input).WithMany(t => t.InputDetails).HasForeignKey(p => p.InputId);
            }
        }
    }
}
