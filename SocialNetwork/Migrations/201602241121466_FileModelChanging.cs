namespace SocialNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileModelChanging : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Files", "ResourceId", "dbo.Resources");
            DropIndex("dbo.Files", new[] { "ResourceId" });
            AlterColumn("dbo.Files", "ResourceId", c => c.Long());
            CreateIndex("dbo.Files", "ResourceId");
            AddForeignKey("dbo.Files", "ResourceId", "dbo.Resources", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Files", "ResourceId", "dbo.Resources");
            DropIndex("dbo.Files", new[] { "ResourceId" });
            AlterColumn("dbo.Files", "ResourceId", c => c.Long(nullable: false));
            CreateIndex("dbo.Files", "ResourceId");
            AddForeignKey("dbo.Files", "ResourceId", "dbo.Resources", "Id", cascadeDelete: true);
        }
    }
}
