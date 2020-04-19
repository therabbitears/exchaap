namespace loffers.api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatGroupChanges : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ChatGroupUsers", "UserId", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.ChatGroups", "Name", c => c.String(nullable: false, maxLength: 100, unicode: false));
            AlterColumn("dbo.ChatGroups", "CreatedBy", c => c.String(nullable: false, maxLength: 50, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ChatGroups", "CreatedBy", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.ChatGroups", "Name", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.ChatGroupUsers", "UserId", c => c.String(nullable: false, maxLength: 20, unicode: false));
        }
    }
}
