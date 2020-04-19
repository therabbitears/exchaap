namespace loffers.api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TrackLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Categories", "CreatedBy", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Categories", "LastEditedBy", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.OfferCategories", "CreatedBy", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.OfferCategories", "LastEditedBy", c => c.String(maxLength: 50, unicode: false));
            AlterColumn("dbo.Offers", "LastEditedBy", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Offers", "CreatedBy", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.OfferLocations", "CreatedBy", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.OfferLocations", "LastEditedBy", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.PublisherLocations", "CreatedBy", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.PublisherLocations", "LastEditedBy", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Locations", "CreatedBy", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Locations", "LastEditedBy", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.PublisherLocationCategories", "CreatedBy", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.PublisherLocationCategories", "LastEditedBy", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Publishers", "CreatedBy", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Publishers", "LastEditedBy", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Publishers", "LastEditedBy", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.Publishers", "CreatedBy", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.PublisherLocationCategories", "LastEditedBy", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.PublisherLocationCategories", "CreatedBy", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.Locations", "LastEditedBy", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.Locations", "CreatedBy", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.PublisherLocations", "LastEditedBy", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.PublisherLocations", "CreatedBy", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.OfferLocations", "LastEditedBy", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.OfferLocations", "CreatedBy", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.Offers", "CreatedBy", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.Offers", "LastEditedBy", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.OfferCategories", "LastEditedBy", c => c.String(maxLength: 20, unicode: false));
            AlterColumn("dbo.OfferCategories", "CreatedBy", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.Categories", "LastEditedBy", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.Categories", "CreatedBy", c => c.String(nullable: false, maxLength: 20, unicode: false));
        }
    }
}
