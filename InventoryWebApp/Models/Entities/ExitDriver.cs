using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class ExitDriver:BaseEntity
    {
        public Guid ExitId { get; set; }
        public virtual Exit Exit { get; set; }
        public string FullName { get; set; }
        internal class configuration : EntityTypeConfiguration<ExitDriver>
        {
            public configuration()
            {
                HasRequired(p => p.Exit).WithMany(t => t.ExitDrivers).HasForeignKey(p => p.ExitId);
            }
        }
    }
}
