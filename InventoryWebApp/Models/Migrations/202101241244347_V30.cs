namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V30 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CutOrders", "CutTypeId", "dbo.CutTypes");
            DropIndex("dbo.CutOrders", new[] { "CutTypeId" });
            AlterColumn("dbo.CutOrders", "CutTypeId", c => c.Guid());
            CreateIndex("dbo.CutOrders", "CutTypeId");
            AddForeignKey("dbo.CutOrders", "CutTypeId", "dbo.CutTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CutOrders", "CutTypeId", "dbo.CutTypes");
            DropIndex("dbo.CutOrders", new[] { "CutTypeId" });
            AlterColumn("dbo.CutOrders", "CutTypeId", c => c.Guid(nullable: false));
            CreateIndex("dbo.CutOrders", "CutTypeId");
            AddForeignKey("dbo.CutOrders", "CutTypeId", "dbo.CutTypes", "Id", cascadeDelete: true);
        }
    }
}
