namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V16 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ExitDrivers", "ExitId", "dbo.Exits");
            DropIndex("dbo.ExitDrivers", new[] { "ExitId" });
            AddColumn("dbo.Exits", "ExitDriverId", c => c.Guid());
            CreateIndex("dbo.Exits", "ExitDriverId");
            AddForeignKey("dbo.Exits", "ExitDriverId", "dbo.ExitDrivers", "Id");
            DropColumn("dbo.ExitDrivers", "ExitId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ExitDrivers", "ExitId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.Exits", "ExitDriverId", "dbo.ExitDrivers");
            DropIndex("dbo.Exits", new[] { "ExitDriverId" });
            DropColumn("dbo.Exits", "ExitDriverId");
            CreateIndex("dbo.ExitDrivers", "ExitId");
            AddForeignKey("dbo.ExitDrivers", "ExitId", "dbo.Exits", "Id", cascadeDelete: true);
        }
    }
}
