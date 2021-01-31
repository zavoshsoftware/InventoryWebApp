namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V31 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CutOrderDetails", "CutDetailTypeId", "dbo.CutDetailTypes");
            DropIndex("dbo.CutOrderDetails", new[] { "CutDetailTypeId" });
            RenameColumn(table: "dbo.CutOrderDetails", name: "CutDetailTypeId", newName: "CutDetailType_Id");
            AddColumn("dbo.CutOrderDetails", "CustomActionId", c => c.Guid(nullable: false));
            AlterColumn("dbo.CutOrderDetails", "CutDetailType_Id", c => c.Guid());
            CreateIndex("dbo.CutOrderDetails", "CustomActionId");
            CreateIndex("dbo.CutOrderDetails", "CutDetailType_Id");
            AddForeignKey("dbo.CutOrderDetails", "CustomActionId", "dbo.CustomActions", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CutOrderDetails", "CutDetailType_Id", "dbo.CutDetailTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CutOrderDetails", "CutDetailType_Id", "dbo.CutDetailTypes");
            DropForeignKey("dbo.CutOrderDetails", "CustomActionId", "dbo.CustomActions");
            DropIndex("dbo.CutOrderDetails", new[] { "CutDetailType_Id" });
            DropIndex("dbo.CutOrderDetails", new[] { "CustomActionId" });
            AlterColumn("dbo.CutOrderDetails", "CutDetailType_Id", c => c.Guid(nullable: false));
            DropColumn("dbo.CutOrderDetails", "CustomActionId");
            RenameColumn(table: "dbo.CutOrderDetails", name: "CutDetailType_Id", newName: "CutDetailTypeId");
            CreateIndex("dbo.CutOrderDetails", "CutDetailTypeId");
            AddForeignKey("dbo.CutOrderDetails", "CutDetailTypeId", "dbo.CutDetailTypes", "Id", cascadeDelete: true);
        }
    }
}
