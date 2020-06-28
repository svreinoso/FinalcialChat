namespace FinalcialChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessageType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "MessageType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "MessageType");
        }
    }
}
