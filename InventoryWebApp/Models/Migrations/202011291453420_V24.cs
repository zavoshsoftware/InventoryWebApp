namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V24 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductGroupActions", "ActionId", "dbo.CustomActions");
            DropIndex("dbo.ProductGroupActions", new[] { "ActionId" });
            AddColumn("dbo.ProductGroupActions", "CustomAction_Id", c => c.Guid());
            CreateIndex("dbo.ProductGroupActions", "CustomAction_Id");
            AddForeignKey("dbo.ProductGroupActions", "CustomAction_Id", "dbo.CustomActions", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductGroupActions", "CustomAction_Id", "dbo.CustomActions");
            DropIndex("dbo.ProductGroupActions", new[] { "CustomAction_Id" });
            DropColumn("dbo.ProductGroupActions", "CustomAction_Id");
            CreateIndex("dbo.ProductGroupActions", "ActionId");
            AddForeignKey("dbo.ProductGroupActions", "ActionId", "dbo.CustomActions", "Id", cascadeDelete: true);
        }
    }
}
