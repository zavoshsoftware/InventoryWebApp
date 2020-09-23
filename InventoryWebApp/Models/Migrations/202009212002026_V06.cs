namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V06 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inputs", "OrderId", "dbo.Orders");
            DropIndex("dbo.Inputs", new[] { "OrderId" });
            AddColumn("dbo.InputDetails", "OrderId", c => c.Guid());
            CreateIndex("dbo.InputDetails", "OrderId");
            AddForeignKey("dbo.InputDetails", "OrderId", "dbo.Orders", "Id");
            DropColumn("dbo.Inputs", "OrderId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inputs", "OrderId", c => c.Guid());
            DropForeignKey("dbo.InputDetails", "OrderId", "dbo.Orders");
            DropIndex("dbo.InputDetails", new[] { "OrderId" });
            DropColumn("dbo.InputDetails", "OrderId");
            CreateIndex("dbo.Inputs", "OrderId");
            AddForeignKey("dbo.Inputs", "OrderId", "dbo.Orders", "Id");
        }
    }
}
