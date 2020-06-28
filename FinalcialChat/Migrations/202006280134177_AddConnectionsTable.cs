namespace FinalcialChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddConnectionsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatConnections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ConnectionID = c.String(),
                        UserAgent = c.String(),
                        Connected = c.Boolean(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChatConnections", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ChatConnections", new[] { "ApplicationUser_Id" });
            DropTable("dbo.ChatConnections");
        }
    }
}
