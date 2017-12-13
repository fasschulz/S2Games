using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S2Games.Database;
using S2Games.Database.Models;

namespace S2Games.Database.Repositories
{
    public class FriendRepository
    {
        S2GamesContext Context;
        public FriendRepository(S2GamesContext context)
        {
            this.Context = context;
        }

        public async Task<List<Friend>> GetFriendsAsync(int connectedId)
        {
            var friends = await Context.Friends
                        .Where(f => f.UserId == connectedId)
                        .ToListAsync();

            return friends;
        }

        public async Task<Friend> GetByIdAsync(int id, int connectedId)
        {
            var friend = await Context.Friends
                .Where(f => f.Id == id && f.UserId == connectedId)
                .FirstOrDefaultAsync();

            return friend;
        }

        public async Task DeleteAsync(int id, int connectedId)
        {
            var friend = await Context.Friends
                .Where(f => f.Id == id && f.UserId == connectedId)
                .FirstOrDefaultAsync();

            var hasLendedGame = await Context.Games
                .Where(g => g.LentForId == friend.Id)
                .AnyAsync();

            if (hasLendedGame)
                throw new Exception("Não é possível excluir este amigo, pois ele tem um jogo emprestado");
            else
                Context.Friends.Remove(friend);
        }
    }
}
