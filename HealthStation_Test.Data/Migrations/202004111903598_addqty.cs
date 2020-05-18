namespace HealthStation_Test.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addqty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookedHistories", "ExpRet_Date", c => c.DateTime(nullable: false));
            AddColumn("dbo.Books", "Quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Books", "Quantity");
            DropColumn("dbo.BookedHistories", "ExpRet_Date");
        }
    }
}
