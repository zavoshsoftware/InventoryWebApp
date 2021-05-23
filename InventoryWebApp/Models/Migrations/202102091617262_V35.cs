namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V35 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "ProductCreatorId", "dbo.ProductCreators");
            DropForeignKey("dbo.Products", "ProductFormId", "dbo.ProductForms");
            DropForeignKey("dbo.Products", "ProductStatusId", "dbo.ProductStatus");
            DropIndex("dbo.Products", new[] { "ProductFormId" });
            DropIndex("dbo.Products", new[] { "ProductCreatorId" });
            DropIndex("dbo.Products", new[] { "ProductStatusId" });
            AlterColumn("dbo.Products", "ProductFormId", c => c.Guid());
            AlterColumn("dbo.Products", "ProductCreatorId", c => c.Guid());
            AlterColumn("dbo.Products", "ProductStatusId", c => c.Guid());
            CreateIndex("dbo.Products", "ProductFormId");
            CreateIndex("dbo.Products", "ProductCreatorId");
            CreateIndex("dbo.Products", "ProductStatusId");
            AddForeignKey("dbo.Products", "ProductCreatorId", "dbo.ProductCreators", "Id");
            AddForeignKey("dbo.Products", "ProductFormId", "dbo.ProductForms", "Id");
            AddForeignKey("dbo.Products", "ProductStatusId", "dbo.ProductStatus", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "ProductStatusId", "dbo.ProductStatus");
            DropForeignKey("dbo.Products", "ProductFormId", "dbo.ProductForms");
            DropForeignKey("dbo.Products", "ProductCreatorId", "dbo.ProductCreators");
            DropIndex("dbo.Products", new[] { "ProductStatusId" });
            DropIndex("dbo.Products", new[] { "ProductCreatorId" });
            DropIndex("dbo.Products", new[] { "ProductFormId" });
            AlterColumn("dbo.Products", "ProductStatusId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Products", "ProductCreatorId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Products", "ProductFormId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Products", "ProductStatusId");
            CreateIndex("dbo.Products", "ProductCreatorId");
            CreateIndex("dbo.Products", "ProductFormId");
            AddForeignKey("dbo.Products", "ProductStatusId", "dbo.ProductStatus", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Products", "ProductFormId", "dbo.ProductForms", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Products", "ProductCreatorId", "dbo.ProductCreators", "Id", cascadeDelete: true);
        }
    }
}
