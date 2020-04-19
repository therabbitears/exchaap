namespace loffers.api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatGroupMessages",
                c => new
                    {
                        ChatGroupMessageID = c.Long(nullable: false, identity: true),
                        CreatedOn = c.DateTime(nullable: false),
                        Message = c.String(nullable: false, maxLength: 500),
                        ChatGroupUserID = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ChatGroupMessageID)
                .ForeignKey("dbo.ChatGroupUsers", t => t.ChatGroupUserID, cascadeDelete: true)
                .Index(t => t.ChatGroupUserID);
            
            CreateTable(
                "dbo.ChatGroupUsers",
                c => new
                    {
                        ChatGroupUserID = c.Long(nullable: false, identity: true),
                        GroupID = c.Long(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 20, unicode: false),
                        Active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ChatGroupUserID)
                .ForeignKey("dbo.ChatGroups", t => t.GroupID, cascadeDelete: true)
                .Index(t => t.GroupID);
            
            CreateTable(
                "dbo.ChatGroups",
                c => new
                    {
                        GroupID = c.Long(nullable: false, identity: true),
                        OfferID = c.Long(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        CreatedBy = c.String(nullable: false, maxLength: 20, unicode: false),
                        CreatedOn = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GroupID)
                .ForeignKey("dbo.Offers", t => t.OfferID, cascadeDelete: true)
                .Index(t => t.OfferID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChatGroupUsers", "GroupID", "dbo.ChatGroups");
            DropForeignKey("dbo.ChatGroups", "OfferID", "dbo.Offers");
            DropForeignKey("dbo.ChatGroupMessages", "ChatGroupUserID", "dbo.ChatGroupUsers");
            DropIndex("dbo.ChatGroups", new[] { "OfferID" });
            DropIndex("dbo.ChatGroupUsers", new[] { "GroupID" });
            DropIndex("dbo.ChatGroupMessages", new[] { "ChatGroupUserID" });
            DropTable("dbo.ChatGroups");
            DropTable("dbo.ChatGroupUsers");
            DropTable("dbo.ChatGroupMessages");
        }
    }
}
