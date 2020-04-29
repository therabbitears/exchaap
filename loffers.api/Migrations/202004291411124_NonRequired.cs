namespace loffers.api.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NonRequired : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Offers", "TermsAndConditions", c => c.String(maxLength: 5000, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Offers", "TermsAndConditions", c => c.String(nullable: false, maxLength: 5000, unicode: false));
        }
    }
}
