namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V32 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempProducts",
                c => new
                    {
                        Product_Code = c.Int(nullable: false, identity: true),
                        Product_Name = c.String(),
                        Product_Name1 = c.String(),
                        Product_Type_Code = c.Int(nullable: false),
                        Height = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Length = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Width = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Condition = c.String(),
                        Grade = c.String(),
                        Maker = c.String(),
                        Type = c.String(),
                        Other = c.String(),
                        Net_weight = c.Boolean(nullable: false),
                        App_Weight = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Product_Code);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TempProducts");
        }
    }
}
