namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V04 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Inputs", "ParentId", "dbo.Inputs");
            DropIndex("dbo.Inputs", new[] { "ParentId" });
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(),
                        CustomerId = c.Guid(nullable: false),
                        ParentId = c.Guid(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Orders", t => t.ParentId)
                .Index(t => t.CustomerId)
                .Index(t => t.ParentId);
            
            AddColumn("dbo.Inputs", "OrderId", c => c.Guid());
            AlterColumn("dbo.Inputs", "Code", c => c.Int(nullable: false));
            CreateIndex("dbo.Inputs", "OrderId");
            AddForeignKey("dbo.Inputs", "OrderId", "dbo.Orders", "Id");
            DropColumn("dbo.Inputs", "ParentId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inputs", "ParentId", c => c.Guid());
            DropForeignKey("dbo.Orders", "ParentId", "dbo.Orders");
            DropForeignKey("dbo.Inputs", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "CustomerId", "dbo.Customers");
            DropIndex("dbo.Orders", new[] { "ParentId" });
            DropIndex("dbo.Orders", new[] { "CustomerId" });
            DropIndex("dbo.Inputs", new[] { "OrderId" });
            AlterColumn("dbo.Inputs", "Code", c => c.String());
            DropColumn("dbo.Inputs", "OrderId");
            DropTable("dbo.Orders");
            CreateIndex("dbo.Inputs", "ParentId");
            AddForeignKey("dbo.Inputs", "ParentId", "dbo.Inputs", "Id");
        }
    }
}
