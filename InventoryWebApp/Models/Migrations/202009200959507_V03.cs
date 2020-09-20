namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V03 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Inputs", "ParentId", c => c.Guid());
            CreateIndex("dbo.Inputs", "ParentId");
            AddForeignKey("dbo.Inputs", "ParentId", "dbo.Inputs", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Inputs", "ParentId", "dbo.Inputs");
            DropIndex("dbo.Inputs", new[] { "ParentId" });
            DropColumn("dbo.Inputs", "ParentId");
        }
    }
}
