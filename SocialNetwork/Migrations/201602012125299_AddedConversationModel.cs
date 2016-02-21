namespace SocialNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedConversationModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "RecipientId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "SenderId", "dbo.AspNetUsers");
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropIndex("dbo.Messages", new[] { "RecipientId" });
            CreateTable(
                "dbo.Conversations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SenderId = c.String(maxLength: 128),
                        RecipientId = c.String(maxLength: 128),
                        MessageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Messages", t => t.MessageId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.RecipientId)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderId)
                .Index(t => t.SenderId)
                .Index(t => t.RecipientId)
                .Index(t => t.MessageId);
            
            DropColumn("dbo.Messages", "SenderId");
            DropColumn("dbo.Messages", "RecipientId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "RecipientId", c => c.String(maxLength: 128));
            AddColumn("dbo.Messages", "SenderId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Conversations", "SenderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Conversations", "RecipientId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Conversations", "MessageId", "dbo.Messages");
            DropIndex("dbo.Conversations", new[] { "MessageId" });
            DropIndex("dbo.Conversations", new[] { "RecipientId" });
            DropIndex("dbo.Conversations", new[] { "SenderId" });
            DropTable("dbo.Conversations");
            CreateIndex("dbo.Messages", "RecipientId");
            CreateIndex("dbo.Messages", "SenderId");
            AddForeignKey("dbo.Messages", "SenderId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Messages", "RecipientId", "dbo.AspNetUsers", "Id");
        }
    }
}
