namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V07 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InputDetails", "RemainQuantity", c => c.Int(nullable: false));
            AddColumn("dbo.InputDetails", "RemainDestinationWeight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.InputDetails", "RemainDestinationWeight");
            DropColumn("dbo.InputDetails", "RemainQuantity");
        }
    }
}
