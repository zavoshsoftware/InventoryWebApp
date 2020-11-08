namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V15 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ExitDrivers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ExitId = c.Guid(nullable: false),
                        FullName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Exits", t => t.ExitId, cascadeDelete: true)
                .Index(t => t.ExitId);
            
            AddColumn("dbo.Exits", "CarNumber", c => c.String());
            AddColumn("dbo.Exits", "DriverPhone", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExitDrivers", "ExitId", "dbo.Exits");
            DropIndex("dbo.ExitDrivers", new[] { "ExitId" });
            DropColumn("dbo.Exits", "DriverPhone");
            DropColumn("dbo.Exits", "CarNumber");
            DropTable("dbo.ExitDrivers");
        }
    }
}
