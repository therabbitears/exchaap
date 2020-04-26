namespace loffers.api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImageAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Offers", "OriginalImage", c => c.String(maxLength: 200, unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Offers", "OriginalImage");
        }
    }
}
