namespace SocialNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFileModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Files",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Path = c.String(),
                        ResourceId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Resources", t => t.ResourceId, cascadeDelete: true)
                .Index(t => t.ResourceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "ResourceId", "dbo.Resources");
            DropIndex("dbo.Files", new[] { "ResourceId" });
            DropTable("dbo.Files");
        }
    }
}
