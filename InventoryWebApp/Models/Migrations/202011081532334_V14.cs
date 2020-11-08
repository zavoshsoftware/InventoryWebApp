namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "CellNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Customers", "CellNumber");
        }
    }
}
