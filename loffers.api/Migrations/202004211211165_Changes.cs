namespace loffers.api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Changes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "ParentCategoryID", c => c.Int());
            AddColumn("dbo.Offers", "CategoryID", c => c.Int(nullable: false));
            AlterColumn("dbo.Offers", "ValidFrom", c => c.DateTime());
            AlterColumn("dbo.Offers", "ValidTill", c => c.DateTime());
            CreateIndex("dbo.Categories", "ParentCategoryID");
            CreateIndex("dbo.Offers", "CategoryID");
            AddForeignKey("dbo.Offers", "CategoryID", "dbo.Categories", "CategoryID", cascadeDelete: true);
            AddForeignKey("dbo.Categories", "ParentCategoryID", "dbo.Categories", "CategoryID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Categories", "ParentCategoryID", "dbo.Categories");
            DropForeignKey("dbo.Offers", "CategoryID", "dbo.Categories");
            DropIndex("dbo.Offers", new[] { "CategoryID" });
            DropIndex("dbo.Categories", new[] { "ParentCategoryID" });
            AlterColumn("dbo.Offers", "ValidTill", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Offers", "ValidFrom", c => c.DateTime(nullable: false));
            DropColumn("dbo.Offers", "CategoryID");
            DropColumn("dbo.Categories", "ParentCategoryID");
        }
    }
}
