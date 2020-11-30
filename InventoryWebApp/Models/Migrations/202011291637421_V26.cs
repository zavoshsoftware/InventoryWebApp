namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V26 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductGroupCustomActions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CustomActionId = c.Guid(nullable: false),
                        ProductGroupId = c.Guid(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CustomActions", t => t.CustomActionId, cascadeDelete: true)
                .ForeignKey("dbo.ProductGroups", t => t.ProductGroupId, cascadeDelete: true)
                .Index(t => t.CustomActionId)
                .Index(t => t.ProductGroupId);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductGroupCustomActions", "ProductGroupId", "dbo.ProductGroups");
            DropForeignKey("dbo.ProductGroupCustomActions", "CustomActionId", "dbo.CustomActions");
            DropIndex("dbo.ProductGroupCustomActions", new[] { "ProductGroupId" });
            DropIndex("dbo.ProductGroupCustomActions", new[] { "CustomActionId" });
            DropTable("dbo.CustomActions");
            DropTable("dbo.ProductGroupCustomActions");
        }
    }
}
