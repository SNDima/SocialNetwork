namespace SocialNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedURLModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.URLs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Content = c.String(),
                        ResourceId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Resources", t => t.ResourceId, cascadeDelete: true)
                .Index(t => t.ResourceId);
            
            DropColumn("dbo.Resources", "URL");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Resources", "URL", c => c.String());
            DropForeignKey("dbo.URLs", "ResourceId", "dbo.Resources");
            DropIndex("dbo.URLs", new[] { "ResourceId" });
            DropTable("dbo.URLs");
        }
    }
}
