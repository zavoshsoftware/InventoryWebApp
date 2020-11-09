namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V21 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exits", "PaymentAmount", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.Exits", "RemainAmount", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exits", "RemainAmount");
            DropColumn("dbo.Exits", "PaymentAmount");
        }
    }
}
