namespace loffers.api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatGroupLocation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChatGroups", "OfferLocationID", c => c.Long(nullable: false));
            CreateIndex("dbo.ChatGroups", "OfferLocationID");
            AddForeignKey("dbo.ChatGroups", "OfferLocationID", "dbo.OfferLocations", "OfferLocationID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChatGroups", "OfferLocationID", "dbo.OfferLocations");
            DropIndex("dbo.ChatGroups", new[] { "OfferLocationID" });
            DropColumn("dbo.ChatGroups", "OfferLocationID");
        }
    }
}
