namespace SocialNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedResourceModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        URL = c.String(),
                        PostingTime = c.DateTime(nullable: false),
                        ViewsNumber = c.Int(nullable: false),
                        OwnerId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.OwnerId)
                .Index(t => t.OwnerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resources", "OwnerId", "dbo.AspNetUsers");
            DropIndex("dbo.Resources", new[] { "OwnerId" });
            DropTable("dbo.Resources");
        }
    }
}
