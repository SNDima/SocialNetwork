namespace SocialNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedTheWholeSchema : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Conversations", "MessageId", "dbo.Messages");
            DropForeignKey("dbo.Conversations", "RecipientId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Conversations", "SenderId", "dbo.AspNetUsers");
            DropIndex("dbo.Conversations", new[] { "SenderId" });
            DropIndex("dbo.Conversations", new[] { "RecipientId" });
            DropIndex("dbo.Conversations", new[] { "MessageId" });
            DropPrimaryKey("dbo.Messages");
            CreateTable(
                "dbo.UserToConversationLinks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        ConversationId = c.Int(nullable: false),
                        LastReadMessageId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Conversations", t => t.ConversationId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.ConversationId);
            
            AddColumn("dbo.Conversations", "Name", c => c.String());
            AddColumn("dbo.Conversations", "IsPublic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Messages", "SenderName", c => c.String());
            AddColumn("dbo.Messages", "ConversationId", c => c.Int(nullable: false));
            AlterColumn("dbo.Messages", "Id", c => c.Long(nullable: false, identity: true));
            AddPrimaryKey("dbo.Messages", "Id");
            CreateIndex("dbo.Messages", "ConversationId");
            AddForeignKey("dbo.Messages", "ConversationId", "dbo.Conversations", "Id", cascadeDelete: true);
            DropColumn("dbo.Conversations", "SenderId");
            DropColumn("dbo.Conversations", "RecipientId");
            DropColumn("dbo.Conversations", "MessageId");
            DropColumn("dbo.Messages", "isRead");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "isRead", c => c.Boolean(nullable: false));
            AddColumn("dbo.Conversations", "MessageId", c => c.Int(nullable: false));
            AddColumn("dbo.Conversations", "RecipientId", c => c.String(maxLength: 128));
            AddColumn("dbo.Conversations", "SenderId", c => c.String(maxLength: 128));
            DropForeignKey("dbo.Messages", "ConversationId", "dbo.Conversations");
            DropForeignKey("dbo.UserToConversationLinks", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserToConversationLinks", "ConversationId", "dbo.Conversations");
            DropIndex("dbo.Messages", new[] { "ConversationId" });
            DropIndex("dbo.UserToConversationLinks", new[] { "ConversationId" });
            DropIndex("dbo.UserToConversationLinks", new[] { "UserId" });
            DropPrimaryKey("dbo.Messages");
            AlterColumn("dbo.Messages", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Messages", "ConversationId");
            DropColumn("dbo.Messages", "SenderName");
            DropColumn("dbo.Conversations", "IsPublic");
            DropColumn("dbo.Conversations", "Name");
            DropTable("dbo.UserToConversationLinks");
            AddPrimaryKey("dbo.Messages", "Id");
            CreateIndex("dbo.Conversations", "MessageId");
            CreateIndex("dbo.Conversations", "RecipientId");
            CreateIndex("dbo.Conversations", "SenderId");
            AddForeignKey("dbo.Conversations", "SenderId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Conversations", "RecipientId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Conversations", "MessageId", "dbo.Messages", "Id", cascadeDelete: true);
        }
    }
}
