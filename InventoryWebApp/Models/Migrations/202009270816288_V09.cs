namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V09 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExitDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ExitId = c.Guid(nullable: false),
                        Quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        InputDetailId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Exits", t => t.ExitId, cascadeDelete: true)
                .ForeignKey("dbo.InputDetails", t => t.InputDetailId, cascadeDelete: true)
                .Index(t => t.ExitId)
                .Index(t => t.InputDetailId);
            
            CreateTable(
                "dbo.Exits",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.Int(nullable: false),
                        ExitDate = c.DateTime(nullable: false),
                        CustomerId = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.InputDetailStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Code = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.InputDetails", "InputDetailStatusId", c => c.Guid());
            CreateIndex("dbo.InputDetails", "InputDetailStatusId");
            AddForeignKey("dbo.InputDetails", "InputDetailStatusId", "dbo.InputDetailStatus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InputDetails", "InputDetailStatusId", "dbo.InputDetailStatus");
            DropForeignKey("dbo.ExitDetails", "InputDetailId", "dbo.InputDetails");
            DropForeignKey("dbo.ExitDetails", "ExitId", "dbo.Exits");
            DropForeignKey("dbo.Exits", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Exits", new[] { "CustomerId" });
            DropIndex("dbo.ExitDetails", new[] { "InputDetailId" });
            DropIndex("dbo.ExitDetails", new[] { "ExitId" });
            DropIndex("dbo.InputDetails", new[] { "InputDetailStatusId" });
            DropColumn("dbo.InputDetails", "InputDetailStatusId");
            DropTable("dbo.InputDetailStatus");
            DropTable("dbo.Exits");
            DropTable("dbo.ExitDetails");
        }
    }
}
