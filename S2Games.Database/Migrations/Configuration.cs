namespace S2Games.Database.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using S2Games.Framework;
    using S2Games.Database.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<S2Games.Database.S2GamesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(S2Games.Database.S2GamesContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            context.Users.AddOrUpdate(a =>
                a.Name,
                new Models.User
                {
                    Name = "admin",
                    Password = Encryption.Encrypt("password", Model.Key),
                    Email = "admin@s2games.com.br",
                    Enabled = true
                });

            context.SaveChanges();
        }
    }
}
