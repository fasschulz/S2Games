namespace S2Games.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GenreEnumGamesTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Games", "Genre", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Games", "Genre", c => c.String());
        }
    }
}
