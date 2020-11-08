namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "IsMultyProduct", c => c.Boolean(nullable: false));
            AddColumn("dbo.Orders", "ProductId", c => c.Guid());
            CreateIndex("dbo.Orders", "ProductId");
            AddForeignKey("dbo.Orders", "ProductId", "dbo.Products", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "ProductId", "dbo.Products");
            DropIndex("dbo.Orders", new[] { "ProductId" });
            DropColumn("dbo.Orders", "ProductId");
            DropColumn("dbo.Orders", "IsMultyProduct");
        }
    }
}
