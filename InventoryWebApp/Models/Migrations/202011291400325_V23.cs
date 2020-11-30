namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V23 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Actions", newName: "CustomActions");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.CustomActions", newName: "Actions");
        }
    }
}
