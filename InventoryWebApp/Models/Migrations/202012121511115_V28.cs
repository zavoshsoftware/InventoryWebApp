namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V28 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accountings",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.Int(nullable: false),
                        Title = c.String(),
                        Bedehkar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Bestankar = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CustomerId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ExitId = c.Guid(),
                        InventoryAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CutAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LoadAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VatAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Exits", t => t.ExitId)
                .Index(t => t.ExitId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payments", "ExitId", "dbo.Exits");
            DropForeignKey("dbo.Accountings", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Payments", new[] { "ExitId" });
            DropIndex("dbo.Accountings", new[] { "CustomerId" });
            DropTable("dbo.Payments");
            DropTable("dbo.Accountings");
        }
    }
}
