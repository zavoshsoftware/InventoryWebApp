namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V20 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exits", "InventoryAmount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Exits", "LoadAmount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Exits", "WeighbridgeAmount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Exits", "OtherAmount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Exits", "CutAmount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Exits", "SubTotalAmount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Exits", "Vat", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Exits", "TotalAmount", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exits", "TotalAmount");
            DropColumn("dbo.Exits", "Vat");
            DropColumn("dbo.Exits", "SubTotalAmount");
            DropColumn("dbo.Exits", "CutAmount");
            DropColumn("dbo.Exits", "OtherAmount");
            DropColumn("dbo.Exits", "WeighbridgeAmount");
            DropColumn("dbo.Exits", "LoadAmount");
            DropColumn("dbo.Exits", "InventoryAmount");
        }
    }
}
