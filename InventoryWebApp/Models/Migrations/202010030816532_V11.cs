namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CutOrders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CutTypeId = c.Guid(nullable: false),
                        InputDetailId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CutTypes", t => t.CutTypeId, cascadeDelete: true)
                .ForeignKey("dbo.InputDetails", t => t.InputDetailId, cascadeDelete: true)
                .Index(t => t.CutTypeId)
                .Index(t => t.InputDetailId);
            
            CreateTable(
                "dbo.CutOrderDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CutOrderId = c.Guid(nullable: false),
                        CutDetailTypeId = c.Guid(nullable: false),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Length = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CutDetailTypes", t => t.CutDetailTypeId, cascadeDelete: true)
                .ForeignKey("dbo.CutOrders", t => t.CutOrderId, cascadeDelete: true)
                .Index(t => t.CutOrderId)
                .Index(t => t.CutDetailTypeId);
            
            CreateTable(
                "dbo.CutDetailTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CutTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CutOrders", "InputDetailId", "dbo.InputDetails");
            DropForeignKey("dbo.CutOrders", "CutTypeId", "dbo.CutTypes");
            DropForeignKey("dbo.CutOrderDetails", "CutOrderId", "dbo.CutOrders");
            DropForeignKey("dbo.CutOrderDetails", "CutDetailTypeId", "dbo.CutDetailTypes");
            DropIndex("dbo.CutOrderDetails", new[] { "CutDetailTypeId" });
            DropIndex("dbo.CutOrderDetails", new[] { "CutOrderId" });
            DropIndex("dbo.CutOrders", new[] { "InputDetailId" });
            DropIndex("dbo.CutOrders", new[] { "CutTypeId" });
            DropTable("dbo.CutTypes");
            DropTable("dbo.CutDetailTypes");
            DropTable("dbo.CutOrderDetails");
            DropTable("dbo.CutOrders");
        }
    }
}
