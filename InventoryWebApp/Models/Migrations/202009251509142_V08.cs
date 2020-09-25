namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V08 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.InputDetails", "Quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.InputDetails", "RemainQuantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.InputDetails", "RemainQuantity", c => c.Int(nullable: false));
            AlterColumn("dbo.InputDetails", "Quantity", c => c.Int(nullable: false));
        }
    }
}
