namespace FinalcialChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddChatBot : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "UserType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "UserType");
        }
    }
}
