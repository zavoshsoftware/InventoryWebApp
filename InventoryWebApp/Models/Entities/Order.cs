using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Order : BaseEntity
    {
        public Order()
        {
            Orders = new List<Order>();
            InputDetails = new List<InputDetail>();
        }
        [Display(Name = "Code", ResourceType = typeof(Resources.Models.Order))]
        public string Code { get; set; }

        [Display(Name = "FullName", ResourceType = typeof(Resources.Models.Customer))]
        public Guid CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [Display(Name = "ParentId", ResourceType = typeof(Resources.Models.Order))]
        public Guid? ParentId { get; set; }
        public virtual Order Parent { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<InputDetail> InputDetails { get; set; }

        [Display(Name="چند کالایی هست؟")]
        public bool IsMultyProduct { get; set; }

        [Display(Name="محصول")]
        public Guid? ProductId { get; set; }
        public virtual Product Product { get; set; }

        internal class configuration : EntityTypeConfiguration<Order>
        {
            public configuration()
            {
                HasOptional(p => p.Parent).WithMany(t => t.Orders).HasForeignKey(p => p.ParentId);
                HasOptional(p => p.Product).WithMany(t => t.Orders).HasForeignKey(p => p.ProductId);
                HasRequired(p => p.Customer).WithMany(t => t.Orders).HasForeignKey(p => p.CustomerId);
            }
        }
    }
}
