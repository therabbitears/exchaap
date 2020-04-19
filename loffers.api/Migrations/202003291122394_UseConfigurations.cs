namespace loffers.api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UseConfigurations : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Categories",
            //    c => new
            //        {
            //            CategoryID = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 50, unicode: false),
            //            Image = c.String(maxLength: 200, unicode: false),
            //            Active = c.Boolean(nullable: false),
            //            CreatedBy = c.String(nullable: false, maxLength: 20, unicode: false),
            //            CreatedOn = c.DateTime(nullable: false),
            //            LastEditedBy = c.String(nullable: false, maxLength: 20, unicode: false),
            //            LastEditedOn = c.DateTime(nullable: false),
            //            Id = c.String(nullable: false, maxLength: 70, unicode: false),
            //            CategoryType = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.CategoryID);
            
            //CreateTable(
            //    "dbo.OfferCategories",
            //    c => new
            //        {
            //            OfferCategoryID = c.Int(nullable: false, identity: true),
            //            OfferID = c.Long(nullable: false),
            //            CategoryID = c.Int(nullable: false),
            //            CreatedBy = c.String(nullable: false, maxLength: 20, unicode: false),
            //            CreatedOn = c.DateTime(nullable: false),
            //            LastEditedBy = c.String(maxLength: 20, unicode: false),
            //            LastEditedOn = c.DateTime(),
            //            Id = c.String(nullable: false, maxLength: 70, unicode: false),
            //            Active = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.OfferCategoryID)
            //    .ForeignKey("dbo.Offers", t => t.OfferID)
            //    .ForeignKey("dbo.Categories", t => t.CategoryID)
            //    .Index(t => t.OfferID)
            //    .Index(t => t.CategoryID);
            
            //CreateTable(
            //    "dbo.Offers",
            //    c => new
            //        {
            //            OfferID = c.Long(nullable: false, identity: true),
            //            PublisherID = c.Int(nullable: false),
            //            OfferHeadline = c.String(nullable: false, maxLength: 255, unicode: false),
            //            OfferDescription = c.String(nullable: false, maxLength: 500, unicode: false),
            //            TermsAndConditions = c.String(nullable: false, maxLength: 5000, unicode: false),
            //            ValidFrom = c.DateTime(nullable: false),
            //            ValidTill = c.DateTime(nullable: false),
            //            Active = c.Boolean(nullable: false),
            //            CreatedOn = c.DateTime(nullable: false),
            //            LastEditedBy = c.String(nullable: false, maxLength: 20, unicode: false),
            //            LastEditedOn = c.DateTime(nullable: false),
            //            CreatedBy = c.String(nullable: false, maxLength: 20, unicode: false),
            //            Image = c.String(maxLength: 200, unicode: false),
            //            Id = c.String(nullable: false, maxLength: 70, unicode: false),
            //        })
            //    .PrimaryKey(t => t.OfferID)
            //    .ForeignKey("dbo.Publishers", t => t.PublisherID)
            //    .Index(t => t.PublisherID);
            
            //CreateTable(
            //    "dbo.OfferComplaints",
            //    c => new
            //        {
            //            ReportId = c.Long(nullable: false, identity: true),
            //            OfferId = c.Long(nullable: false),
            //            ReportContent = c.String(nullable: false, maxLength: 1000, unicode: false),
            //            CreatedOn = c.DateTime(nullable: false),
            //            CreatedBy = c.String(maxLength: 100, unicode: false),
            //        })
            //    .PrimaryKey(t => t.ReportId)
            //    .ForeignKey("dbo.Offers", t => t.OfferId)
            //    .Index(t => t.OfferId);
            
            //CreateTable(
            //    "dbo.OfferLocations",
            //    c => new
            //        {
            //            OfferLocationID = c.Long(nullable: false, identity: true),
            //            OfferID = c.Long(nullable: false),
            //            PublisherLocationID = c.Long(),
            //            Active = c.Boolean(nullable: false),
            //            CreatedBy = c.String(nullable: false, maxLength: 20, unicode: false),
            //            CreatedOn = c.DateTime(nullable: false),
            //            LastEditedBy = c.String(nullable: false, maxLength: 20, unicode: false),
            //            LastEditedOn = c.DateTime(nullable: false),
            //            ValidFrom = c.DateTime(),
            //            ValidTill = c.DateTime(),
            //        })
            //    .PrimaryKey(t => t.OfferLocationID)
            //    .ForeignKey("dbo.PublisherLocations", t => t.PublisherLocationID)
            //    .ForeignKey("dbo.Offers", t => t.OfferID)
            //    .Index(t => t.OfferID)
            //    .Index(t => t.PublisherLocationID);
            
            //CreateTable(
            //    "dbo.PublisherLocations",
            //    c => new
            //        {
            //            PublisherLocationID = c.Long(nullable: false, identity: true),
            //            PublisherID = c.Int(),
            //            LocationID = c.Long(nullable: false),
            //            Active = c.Boolean(),
            //            CreatedBy = c.String(nullable: false, maxLength: 20, unicode: false),
            //            CreatedOn = c.DateTime(nullable: false),
            //            LastEditedBy = c.String(nullable: false, maxLength: 20, unicode: false),
            //            LastEditedOn = c.DateTime(nullable: false),
            //            Image = c.String(maxLength: 200, unicode: false),
            //            Id = c.String(nullable: false, maxLength: 70, unicode: false),
            //        })
            //    .PrimaryKey(t => t.PublisherLocationID)
            //    .ForeignKey("dbo.Locations", t => t.LocationID)
            //    .ForeignKey("dbo.Publishers", t => t.PublisherID)
            //    .Index(t => t.PublisherID)
            //    .Index(t => t.LocationID);
            
            //CreateTable(
            //    "dbo.Locations",
            //    c => new
            //        {
            //            LocationID = c.Long(nullable: false, identity: true),
            //            Lat = c.Decimal(nullable: false, precision: 18, scale: 9),
            //            Long = c.Decimal(nullable: false, precision: 18, scale: 9),
            //            Active = c.Boolean(nullable: false),
            //            CreatedBy = c.String(nullable: false, maxLength: 20, unicode: false),
            //            CreatedOn = c.DateTime(nullable: false),
            //            LastEditedBy = c.String(nullable: false, maxLength: 20, unicode: false),
            //            LastEditedOn = c.DateTime(nullable: false),
            //            Name = c.String(maxLength: 100, unicode: false),
            //            DisplayAddress = c.String(maxLength: 500, unicode: false),
            //        })
            //    .PrimaryKey(t => t.LocationID);
            
            //CreateTable(
            //    "dbo.PublisherLocationCategories",
            //    c => new
            //        {
            //            PublisherLocationCategoryID = c.Long(nullable: false, identity: true),
            //            PublisherLocationID = c.Long(),
            //            CategoryID = c.Int(),
            //            Active = c.Boolean(),
            //            CreatedBy = c.String(nullable: false, maxLength: 20, unicode: false),
            //            CreatedOn = c.DateTime(nullable: false),
            //            LastEditedBy = c.String(nullable: false, maxLength: 20, unicode: false),
            //            LastEditedOn = c.DateTime(nullable: false),
            //        })
            //    .PrimaryKey(t => t.PublisherLocationCategoryID)
            //    .ForeignKey("dbo.Categories", t => t.CategoryID)
            //    .ForeignKey("dbo.PublisherLocations", t => t.PublisherLocationID)
            //    .Index(t => t.PublisherLocationID)
            //    .Index(t => t.CategoryID);
            
            //CreateTable(
            //    "dbo.Publishers",
            //    c => new
            //        {
            //            PublisherID = c.Int(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 100, unicode: false),
            //            Description = c.String(maxLength: 300, unicode: false),
            //            Image = c.String(maxLength: 200, unicode: false),
            //            Active = c.Boolean(nullable: false),
            //            CreatedBy = c.String(nullable: false, maxLength: 20, unicode: false),
            //            CreatedOn = c.DateTime(nullable: false),
            //            LastEditedBy = c.String(nullable: false, maxLength: 20, unicode: false),
            //            LastEditedOn = c.DateTime(nullable: false),
            //            Id = c.String(nullable: false, maxLength: 70, unicode: false),
            //        })
            //    .PrimaryKey(t => t.PublisherID);
            
            //CreateTable(
            //    "dbo.UserStarredOffers",
            //    c => new
            //        {
            //            StarredOfferId = c.Int(nullable: false, identity: true),
            //            UserId = c.String(nullable: false, maxLength: 50, unicode: false),
            //            OfferId = c.Long(nullable: false),
            //            Active = c.Boolean(nullable: false),
            //            CreatedOn = c.DateTime(nullable: false),
            //            LastEditedOn = c.DateTime(nullable: false),
            //            OfferLocationID = c.Long(nullable: false),
            //        })
            //    .PrimaryKey(t => t.StarredOfferId)
            //    .ForeignKey("dbo.OfferLocations", t => t.OfferLocationID)
            //    .ForeignKey("dbo.Offers", t => t.OfferId)
            //    .Index(t => t.OfferId)
            //    .Index(t => t.OfferLocationID);
            
            //CreateTable(
            //    "dbo.UserCategories",
            //    c => new
            //        {
            //            UserCategoryId = c.Int(nullable: false, identity: true),
            //            CategoryID = c.Int(nullable: false),
            //            UserId = c.String(nullable: false, maxLength: 50, unicode: false),
            //            CreatedOn = c.DateTime(nullable: false),
            //            LastEditedOn = c.DateTime(nullable: false),
            //            Active = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.UserCategoryId)
            //    .ForeignKey("dbo.Categories", t => t.CategoryID)
            //    .Index(t => t.CategoryID);
            
            //CreateTable(
            //    "dbo.sysdiagrams",
            //    c => new
            //        {
            //            diagram_id = c.Int(nullable: false, identity: true),
            //            name = c.String(nullable: false, maxLength: 128),
            //            principal_id = c.Int(nullable: false),
            //            version = c.Int(),
            //            definition = c.Binary(),
            //        })
            //    .PrimaryKey(t => t.diagram_id);
            
            CreateTable(
                "dbo.UseConfigurations",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 65, unicode: false),
                        Configuration = c.String(nullable: false, maxLength: 500),
                        LastEditedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.UserCategories", "CategoryID", "dbo.Categories");
            //DropForeignKey("dbo.OfferCategories", "CategoryID", "dbo.Categories");
            //DropForeignKey("dbo.UserStarredOffers", "OfferId", "dbo.Offers");
            //DropForeignKey("dbo.OfferLocations", "OfferID", "dbo.Offers");
            //DropForeignKey("dbo.UserStarredOffers", "OfferLocationID", "dbo.OfferLocations");
            //DropForeignKey("dbo.PublisherLocations", "PublisherID", "dbo.Publishers");
            //DropForeignKey("dbo.Offers", "PublisherID", "dbo.Publishers");
            //DropForeignKey("dbo.PublisherLocationCategories", "PublisherLocationID", "dbo.PublisherLocations");
            //DropForeignKey("dbo.PublisherLocationCategories", "CategoryID", "dbo.Categories");
            //DropForeignKey("dbo.OfferLocations", "PublisherLocationID", "dbo.PublisherLocations");
            //DropForeignKey("dbo.PublisherLocations", "LocationID", "dbo.Locations");
            //DropForeignKey("dbo.OfferComplaints", "OfferId", "dbo.Offers");
            //DropForeignKey("dbo.OfferCategories", "OfferID", "dbo.Offers");
            //DropIndex("dbo.UserCategories", new[] { "CategoryID" });
            //DropIndex("dbo.UserStarredOffers", new[] { "OfferLocationID" });
            //DropIndex("dbo.UserStarredOffers", new[] { "OfferId" });
            //DropIndex("dbo.PublisherLocationCategories", new[] { "CategoryID" });
            //DropIndex("dbo.PublisherLocationCategories", new[] { "PublisherLocationID" });
            //DropIndex("dbo.PublisherLocations", new[] { "LocationID" });
            //DropIndex("dbo.PublisherLocations", new[] { "PublisherID" });
            //DropIndex("dbo.OfferLocations", new[] { "PublisherLocationID" });
            //DropIndex("dbo.OfferLocations", new[] { "OfferID" });
            //DropIndex("dbo.OfferComplaints", new[] { "OfferId" });
            //DropIndex("dbo.Offers", new[] { "PublisherID" });
            //DropIndex("dbo.OfferCategories", new[] { "CategoryID" });
            //DropIndex("dbo.OfferCategories", new[] { "OfferID" });
            DropTable("dbo.UseConfigurations");
            //DropTable("dbo.sysdiagrams");
            //DropTable("dbo.UserCategories");
            //DropTable("dbo.UserStarredOffers");
            //DropTable("dbo.Publishers");
            //DropTable("dbo.PublisherLocationCategories");
            //DropTable("dbo.Locations");
            //DropTable("dbo.PublisherLocations");
            //DropTable("dbo.OfferLocations");
            //DropTable("dbo.OfferComplaints");
            //DropTable("dbo.Offers");
            //DropTable("dbo.OfferCategories");
            //DropTable("dbo.Categories");
        }
    }
}
