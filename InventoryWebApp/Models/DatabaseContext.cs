using System;
using System.Collections.Generic;
using System.Data.Entity;
namespace Models
{
   public class DatabaseContext:DbContext
    {
        static DatabaseContext()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DatabaseContext, Migrations.Configuration>());
        }
        public DbSet<City> Cities { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProductGroup> ProductGroups { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Transporter> Transporters { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<ProductGroupUnit> ProductGroupUnits { get; set; }
        public DbSet<Input> Inputs { get; set; }
        public DbSet<InputDetail> InputDetails { get; set; }
        public DbSet<ProductCreator> ProductCreators { get; set; }
        public DbSet<ProductForm> ProductForms { get; set; }
        public DbSet<ProductStatus> ProductStatuses { get; set; }
    }
}
