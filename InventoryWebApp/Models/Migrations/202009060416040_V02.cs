namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V02 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "FullName", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "FullName", c => c.Guid(nullable: false));
        }
    }
}
