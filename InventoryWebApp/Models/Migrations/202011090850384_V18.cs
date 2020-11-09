namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V18 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exits", "Order", c => c.Int(nullable: false));
            DropColumn("dbo.Exits", "Code");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Exits", "Code", c => c.Int(nullable: false));
            DropColumn("dbo.Exits", "Order");
        }
    }
}
