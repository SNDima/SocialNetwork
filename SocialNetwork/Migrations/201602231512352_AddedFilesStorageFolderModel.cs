namespace SocialNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFilesStorageFolderModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FilesStorageFolders",
                c => new
                    {
                        ResourceId = c.Long(nullable: false),
                        Path = c.String(),
                    })
                .PrimaryKey(t => t.ResourceId)
                .ForeignKey("dbo.Resources", t => t.ResourceId)
                .Index(t => t.ResourceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FilesStorageFolders", "ResourceId", "dbo.Resources");
            DropIndex("dbo.FilesStorageFolders", new[] { "ResourceId" });
            DropTable("dbo.FilesStorageFolders");
        }
    }
}
