namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V17 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ExitDetails", "InitialQuantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ExitDetails", "InitialWeight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ExitDetails", "FullWeight", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ExitDetails", "EmptyWeight", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ExitDetails", "PureWeight", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Exits", "ExitComplete", c => c.Boolean(nullable: false));
            DropColumn("dbo.ExitDetails", "Quantity");
            DropColumn("dbo.ExitDetails", "Weight");
            DropColumn("dbo.Exits", "IsOpen");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Exits", "IsOpen", c => c.Boolean(nullable: false));
            AddColumn("dbo.ExitDetails", "Weight", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ExitDetails", "Quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.Exits", "ExitComplete");
            DropColumn("dbo.ExitDetails", "PureWeight");
            DropColumn("dbo.ExitDetails", "EmptyWeight");
            DropColumn("dbo.ExitDetails", "FullWeight");
            DropColumn("dbo.ExitDetails", "InitialWeight");
            DropColumn("dbo.ExitDetails", "InitialQuantity");
        }
    }
}
