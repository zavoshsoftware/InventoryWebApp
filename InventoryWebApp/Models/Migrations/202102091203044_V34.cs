namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V34 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "Grade", c => c.String());
            AddColumn("dbo.Products", "Condition", c => c.String());
            DropColumn("dbo.Products", "Grid");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Grid", c => c.String());
            DropColumn("dbo.Products", "Condition");
            DropColumn("dbo.Products", "Grade");
        }
    }
}
