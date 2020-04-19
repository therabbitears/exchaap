namespace loffers.api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SnapshotAdded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfileSnapshots",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 50),
                        FullName = c.String(nullable: false, maxLength: 100),
                        PrimaryEmail = c.String(nullable: false, maxLength: 65),
                        PrimaryPhone = c.String(),
                        DateOfBirth = c.DateTime(),
                        LastLat = c.Long(),
                        LastLong = c.Long(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserProfileSnapshots");
        }
    }
}
