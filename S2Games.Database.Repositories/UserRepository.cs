using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using S2Games.Database;
using S2Games.Database.Models;
using S2Games.Framework;

namespace S2Games.Database.Repositories
{
    public class UserRepository
    {
        S2GamesContext Context;
        public UserRepository(S2GamesContext context)
        {
            this.Context = context;
        }

        public async Task<int> ConnectAsync(string username, string password)
        {
            password = Encryption.Encrypt(password, Model.Key);

            var query = from u in Context.Users
                        where u.Name == username && u.Password == password
                        select u;
            var user = await query.FirstOrDefaultAsync();

            if (user != null)
            {
                if (!user.Enabled)
                    throw new Exception("Usuário desativado");
                else
                    return user.Id;
            }
            else
                return 0;
                
        }

        public User GetById(int? id)
        {
            var query = from u in Context.Users
                        where u.Id == id
                        select u;
            var user = query.FirstOrDefault();

            return user;
        }
    }
}
