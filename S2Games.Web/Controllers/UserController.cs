using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using S2Games.Database;
using S2Games.Database.Repositories;

namespace S2Games.Web.Controllers
{
    public class UserController : Controller
    {
        [HttpPost]
        public async Task<ActionResult> Connect(string username, string password)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var context = new S2GamesContext())
                    {
                        var repository = new UserRepository(context);

                        int accountId = 0;
                        accountId = await repository.ConnectAsync(username, password);
                        ConnectedId = accountId;

                        if (accountId > 0)
                            return RedirectToAction("Index", "Game");
                        else
                            throw new Exception("Usuário ou senha incorretos");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    TransportData();
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Disconnect()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}