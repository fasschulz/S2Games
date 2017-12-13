using S2Games.Database.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S2Games.Database.Repositories
{
    public class GameRepository
    {
        S2GamesContext Context;
        public GameRepository(S2GamesContext context)
        {
            this.Context = context;
        }

        public async Task<Game> GetByIdAsync (int id, int connectedId)
        {
            var game = await Context.Games
                .Where(g => g.Id == id && g.UserId == connectedId)
                .FirstOrDefaultAsync();

            return game;
        }

        public async Task LendOrReturn (int? friendId, int gameId, int connectedId)
        {
            var game = await Context.Games
                .Where(g => g.Id == gameId && g.UserId == connectedId)
                .FirstOrDefaultAsync();

            if (game == null)
                throw new Exception("Esse jogo não existe");

            game.LentForId = friendId;
            Context.Games.AddOrUpdate(game);
        }
    }
}
