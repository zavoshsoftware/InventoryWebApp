namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V12 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.InputDetails", "Code");
        }
        
        public override void Down()
        {
            AddColumn("dbo.InputDetails", "Code", c => c.String());
        }
    }
}
