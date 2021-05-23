namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V33 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TempCustomers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Buyer_Code = c.String(),
                        Buyer_Name = c.String(),
                        Buyer_FirstName = c.String(),
                        Buyer_LastName = c.String(),
                        Buyer_Address = c.String(),
                        Buyer_Tel = c.String(),
                        Buyer_Mobile = c.String(),
                        Buyer_Fax = c.String(),
                        Buyer_Bank = c.Boolean(nullable: false),
                        Bussiness = c.Boolean(nullable: false),
                        Buyer_Type = c.String(),
                        Pass_Date = c.String(),
                        Remind_Cash = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Is_Shown = c.Boolean(nullable: false),
                        Buyer_Include = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TempCustomers");
        }
    }
}
