namespace loffers.api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Discoveravle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Locations", "Explorable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Locations", "Explorable");
        }
    }
}
