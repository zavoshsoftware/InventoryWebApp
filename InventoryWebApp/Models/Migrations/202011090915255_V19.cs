namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V19 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exits", "Code", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exits", "Code");
        }
    }
}
