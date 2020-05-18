namespace HealthStation_Test.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addprice : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Books", "ISBN", c => c.String());
            AddColumn("dbo.Books", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.Books", "Publisher_Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Books", "Publisher_Date", c => c.String());
            DropColumn("dbo.Books", "Price");
            DropColumn("dbo.Books", "ISBN");
        }
    }
}
