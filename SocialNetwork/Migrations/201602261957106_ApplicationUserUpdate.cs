namespace SocialNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApplicationUserUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Position", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Position");
        }
    }
}
