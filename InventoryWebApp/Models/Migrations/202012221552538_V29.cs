namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V29 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "IsLatest", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "IsLatest");
        }
    }
}
