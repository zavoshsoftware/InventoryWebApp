namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V05 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InputDetails", "ParentId", c => c.Guid());
            CreateIndex("dbo.InputDetails", "ParentId");
            AddForeignKey("dbo.InputDetails", "ParentId", "dbo.InputDetails", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.InputDetails", "ParentId", "dbo.InputDetails");
            DropIndex("dbo.InputDetails", new[] { "ParentId" });
            DropColumn("dbo.InputDetails", "ParentId");
        }
    }
}
