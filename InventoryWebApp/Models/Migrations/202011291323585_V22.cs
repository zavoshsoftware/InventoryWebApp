namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V22 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Actions",
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Actions", t => t.ActionId, cascadeDelete: true)
                .ForeignKey("dbo.ProductGroups", t => t.ProductGroupId, cascadeDelete: true)
                .Index(t => t.ActionId)
                .Index(t => t.ProductGroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductGroupActions", "ProductGroupId", "dbo.ProductGroups");
            DropForeignKey("dbo.ProductGroupActions", "ActionId", "dbo.Actions");
            DropIndex("dbo.ProductGroupActions", new[] { "ProductGroupId" });
            DropIndex("dbo.ProductGroupActions", new[] { "ActionId" });
            DropTable("dbo.ProductGroupActions");
            DropTable("dbo.Actions");
        }
    }
}
