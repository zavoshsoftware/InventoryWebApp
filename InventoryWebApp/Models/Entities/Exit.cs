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

        [Display(Name = "Order", ResourceType = typeof(Resources.Models.Exit))]
        public int Order { get; set; }

        [Display(Name = "Code", ResourceType = typeof(Resources.Models.Exit))]
        public int? Code { get; set; }

        [Display(Name = "ExitDate", ResourceType = typeof(Resources.Models.Exit))]
        public DateTime ExitDate { get; set; }

        [Display(Name = "FullName", ResourceType = typeof(Resources.Models.Customer))]
        public Guid? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }


        public virtual ICollection<ExitDetail> ExitDetails { get; set; }
     
        public bool ExitComplete { get; set; }

        [Display(Name = "CarNumber", ResourceType = typeof(Resources.Models.Input))]
        public string CarNumber { get; set; }

        public string DriverPhone { get; set; }

        public Guid? ExitDriverId { get; set; }
        public virtual ExitDriver ExitDriver { get; set; }

        internal class configuration : EntityTypeConfiguration<Exit>
        {
            public configuration()
            {
                HasRequired(p => p.ExitDriver).WithMany(t => t.Exits).HasForeignKey(p => p.ExitDriverId);
            }
        }


        public decimal? InventoryAmount { get; set; }
        public decimal? LoadAmount { get; set; }
        public decimal? WeighbridgeAmount { get; set; }
        public decimal? OtherAmount { get; set; }
        public decimal? CutAmount { get; set; }
        public decimal? SubTotalAmount { get; set; }

        public decimal? Vat { get; set; }
        public decimal? TotalAmount { get; set; }
    }
}
