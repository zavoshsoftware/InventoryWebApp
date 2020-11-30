namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V25 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductGroupActions", "CustomAction_Id", "dbo.CustomActions");
            DropForeignKey("dbo.ProductGroupActions", "ProductGroupId", "dbo.ProductGroups");
            DropIndex("dbo.ProductGroupActions", new[] { "ProductGroupId" });
            DropIndex("dbo.ProductGroupActions", new[] { "CustomAction_Id" });
            DropTable("dbo.ProductGroupActions");
            DropTable("dbo.CustomActions");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CustomActions",
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
                "dbo.ProductGroupActions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ActionId = c.Guid(nullable: false),
                        ProductGroupId = c.Guid(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                        CustomAction_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.ProductGroupActions", "CustomAction_Id");
            CreateIndex("dbo.ProductGroupActions", "ProductGroupId");
            AddForeignKey("dbo.ProductGroupActions", "ProductGroupId", "dbo.ProductGroups", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProductGroupActions", "CustomAction_Id", "dbo.CustomActions", "Id");
        }
    }
}
