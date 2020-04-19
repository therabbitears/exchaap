namespace loffers.api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Security : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfileSnapshots", "SecurityCode", c => c.String(maxLength: 10));
            AddColumn("dbo.UserProfileSnapshots", "SecurityCodeValidatity", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfileSnapshots", "SecurityCodeValidatity");
            DropColumn("dbo.UserProfileSnapshots", "SecurityCode");
        }
    }
}
