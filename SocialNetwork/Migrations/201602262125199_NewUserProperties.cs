namespace SocialNetwork.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewUserProperties : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdditionalEmails",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Email = c.String(maxLength: 256),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AdditionalPhoneNumbers",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        PhoneNumber = c.String(maxLength: 13),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Skypes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Value = c.String(maxLength: 256),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            AddColumn("dbo.AspNetUsers", "Birthday", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Skypes", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AdditionalPhoneNumbers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AdditionalEmails", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Skypes", new[] { "UserId" });
            DropIndex("dbo.AdditionalPhoneNumbers", new[] { "UserId" });
            DropIndex("dbo.AdditionalEmails", new[] { "UserId" });
            DropColumn("dbo.AspNetUsers", "Birthday");
            DropTable("dbo.Skypes");
            DropTable("dbo.AdditionalPhoneNumbers");
            DropTable("dbo.AdditionalEmails");
        }
    }
}
