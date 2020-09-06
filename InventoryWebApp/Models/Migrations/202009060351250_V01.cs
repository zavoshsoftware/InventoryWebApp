namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V01 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProvinceId = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        IsCenter = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Provinces", t => t.ProvinceId, cascadeDelete: true)
                .Index(t => t.ProvinceId);
            
            CreateTable(
                "dbo.Inputs",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Code = c.String(),
                        InputDate = c.DateTime(nullable: false),
                        CustomerId = c.Guid(nullable: false),
                        InputTime = c.String(),
                        PostRentAmount = c.Decimal(precision: 18, scale: 2),
                        WeighbridgeAmount = c.Decimal(precision: 18, scale: 2),
                        OtherAmount = c.Decimal(precision: 18, scale: 2),
                        CommissionAmount = c.Decimal(precision: 18, scale: 2),
                        SourceWeight = c.Decimal(precision: 18, scale: 2),
                        DestinationWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FullWeight = c.Decimal(precision: 18, scale: 2),
                        EmptyWeight = c.Decimal(precision: 18, scale: 2),
                        TransporterCode = c.String(),
                        TransporterId = c.Guid(nullable: false),
                        CityId = c.Guid(nullable: false),
                        CarNumber = c.String(),
                        InputDesc = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Transporters", t => t.TransporterId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.TransporterId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FullName = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InputDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        InputId = c.Guid(nullable: false),
                        ProductId = c.Guid(nullable: false),
                        Code = c.String(),
                        Quantity = c.Int(nullable: false),
                        DestinationWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SourceWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Inputs", t => t.InputId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.InputId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        ProductGroupId = c.Guid(nullable: false),
                        ProductFormId = c.Guid(nullable: false),
                        ProductCreatorId = c.Guid(nullable: false),
                        ProductStatusId = c.Guid(nullable: false),
                        Length = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Thickness = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Width = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Weight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsPureWeight = c.Boolean(nullable: false),
                        Grid = c.String(),
                        Other = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductCreators", t => t.ProductCreatorId, cascadeDelete: true)
                .ForeignKey("dbo.ProductForms", t => t.ProductFormId, cascadeDelete: true)
                .ForeignKey("dbo.ProductGroups", t => t.ProductGroupId, cascadeDelete: true)
                .ForeignKey("dbo.ProductStatus", t => t.ProductStatusId, cascadeDelete: true)
                .Index(t => t.ProductGroupId)
                .Index(t => t.ProductFormId)
                .Index(t => t.ProductCreatorId)
                .Index(t => t.ProductStatusId);
            
            CreateTable(
                "dbo.ProductCreators",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductForms",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductGroups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        Code = c.Int(nullable: false),
                        InventoryAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProductGroupUnitId = c.Guid(nullable: false),
                        Density = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ProductGroupUnits", t => t.ProductGroupUnitId, cascadeDelete: true)
                .Index(t => t.ProductGroupUnitId);
            
            CreateTable(
                "dbo.ProductGroupUnits",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProductStatus",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transporters",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Provinces",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Title = c.String(nullable: false, maxLength: 50),
                        Name = c.String(nullable: false, maxLength: 50),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Username = c.String(nullable: false),
                        Code = c.Int(nullable: false),
                        Password = c.String(maxLength: 150),
                        FullName = c.String(nullable: false, maxLength: 250),
                        RoleId = c.Guid(nullable: false),
                        Email = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        LastModifiedDate = c.DateTime(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletionDate = c.DateTime(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Cities", "ProvinceId", "dbo.Provinces");
            DropForeignKey("dbo.Inputs", "TransporterId", "dbo.Transporters");
            DropForeignKey("dbo.Products", "ProductStatusId", "dbo.ProductStatus");
            DropForeignKey("dbo.Products", "ProductGroupId", "dbo.ProductGroups");
            DropForeignKey("dbo.ProductGroups", "ProductGroupUnitId", "dbo.ProductGroupUnits");
            DropForeignKey("dbo.Products", "ProductFormId", "dbo.ProductForms");
            DropForeignKey("dbo.Products", "ProductCreatorId", "dbo.ProductCreators");
            DropForeignKey("dbo.InputDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.InputDetails", "InputId", "dbo.Inputs");
            DropForeignKey("dbo.Inputs", "CustomerId", "dbo.Customers");
            DropForeignKey("dbo.Inputs", "CityId", "dbo.Cities");
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.ProductGroups", new[] { "ProductGroupUnitId" });
            DropIndex("dbo.Products", new[] { "ProductStatusId" });
            DropIndex("dbo.Products", new[] { "ProductCreatorId" });
            DropIndex("dbo.Products", new[] { "ProductFormId" });
            DropIndex("dbo.Products", new[] { "ProductGroupId" });
            DropIndex("dbo.InputDetails", new[] { "ProductId" });
            DropIndex("dbo.InputDetails", new[] { "InputId" });
            DropIndex("dbo.Inputs", new[] { "CityId" });
            DropIndex("dbo.Inputs", new[] { "TransporterId" });
            DropIndex("dbo.Inputs", new[] { "CustomerId" });
            DropIndex("dbo.Cities", new[] { "ProvinceId" });
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.Provinces");
            DropTable("dbo.Transporters");
            DropTable("dbo.ProductStatus");
            DropTable("dbo.ProductGroupUnits");
            DropTable("dbo.ProductGroups");
            DropTable("dbo.ProductForms");
            DropTable("dbo.ProductCreators");
            DropTable("dbo.Products");
            DropTable("dbo.InputDetails");
            DropTable("dbo.Customers");
            DropTable("dbo.Inputs");
            DropTable("dbo.Cities");
        }
    }
}
