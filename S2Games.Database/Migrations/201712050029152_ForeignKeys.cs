namespace S2Games.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForeignKeys : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Friends", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Games", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Games", "LentForId", c => c.Int());
            CreateIndex("dbo.Friends", "UserId");
            CreateIndex("dbo.Games", "UserId");
            CreateIndex("dbo.Games", "LentForId");
            AddForeignKey("dbo.Friends", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Games", "LentForId", "dbo.Friends", "Id");
            AddForeignKey("dbo.Games", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Games", "UserId", "dbo.Users");
            DropForeignKey("dbo.Games", "LentForId", "dbo.Friends");
            DropForeignKey("dbo.Friends", "UserId", "dbo.Users");
            DropIndex("dbo.Games", new[] { "LentForId" });
            DropIndex("dbo.Games", new[] { "UserId" });
            DropIndex("dbo.Friends", new[] { "UserId" });
            DropColumn("dbo.Games", "LentForId");
            DropColumn("dbo.Games", "UserId");
            DropColumn("dbo.Friends", "UserId");
        }
    }
}
