using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ExitDetail : BaseEntity
    {
        public Guid ExitId { get; set; }

        public virtual Exit Exit { get; set; }

        [Display(Name = "Quantity", ResourceType = typeof(Resources.Models.ExitDetail))]
        public decimal InitialQuantity { get; set; }

        [Display(Name = "Weight", ResourceType = typeof(Resources.Models.ExitDetail))]
        public decimal InitialWeight { get; set; }
         
        public Guid InputDetailId { get; set; }
        public virtual InputDetail InputDetail { get; set; }

        [Display(Name = "FullWeight", ResourceType = typeof(Resources.Models.ExitDetail))]
        public decimal? FullWeight { get; set; }
        [Display(Name = "EmptyWeight", ResourceType = typeof(Resources.Models.ExitDetail))]
        public decimal? EmptyWeight { get; set; }
        [Display(Name = "PureWeight", ResourceType = typeof(Resources.Models.ExitDetail))]
        public decimal? PureWeight { get; set; }

        internal class configuration : EntityTypeConfiguration<ExitDetail>
        {
            public configuration()
            {
                HasRequired(p => p.InputDetail).WithMany(t => t.ExitDetails).HasForeignKey(p => p.InputDetailId);
                HasRequired(p => p.Exit).WithMany(t => t.ExitDetails).HasForeignKey(p => p.ExitId);
            }
        }
    }
}
