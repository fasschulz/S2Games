using PagedList;
using S2Games.Database;
using S2Games.Database.Models;
using S2Games.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace S2Games.Web.Controllers
{
    public class GameController : Controller
    {
        [RequireConnection]
        public async Task<ActionResult> Index(int? page, string search = null)
        {
            try
            {
                using (var context = new S2GamesContext())
                {
                    IQueryable<Game> query = null;

                    if (search == null)
                    {
                        query = context.Games
                            .Where(g => g.UserId == ConnectedId)
                            .Include(g => g.LentFor);                            
                    }
                    else
                    {
                        query = context.Games
                            .Where(g => (g.Label.Contains(search) || g.LentFor.Name.Contains(search)) && g.UserId == ConnectedId)
                            .Include(g => g.LentFor);
                    }

                    var games = await query.ToListAsync();
                    var pageNumber = page ?? 1;
                    var friends = await context.Friends
                        .Where(f => f.UserId == ConnectedId)
                        .ToListAsync();

                    ViewBag.GamesPagedList = games.ToPagedList(pageNumber, ItemsPerPage);
                    ViewBag.Search = search;
                    ViewBag.Friends = new SelectList(friends, "Id", "Name");

                    context.Dispose();
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                TransportData();
            }

            return View();
        }

        [RequireConnection]
        public async Task<ActionResult> Edit(int id = 0)
        {
            if (id > 0)
            {
                using (var context = new S2GamesContext())
                {
                    var repository = new GameRepository(context);
                    var game = await repository.GetByIdAsync(id, ConnectedId);

                    context.Dispose();
                    return View(game);
                }
            }
            else
                return View(new Game { UserId = ConnectedId });
        }

        [HttpPost]
        [RequireConnection]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Game game)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var context = new S2GamesContext())
                    {
                        context.Games.AddOrUpdate(game);

                        await context.SaveChangesAsync();

                        context.Dispose();
                        return RedirectToAction("Index");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                TransportData();
            }

            return View();
        }

        [RequireConnection]
        public async Task<ActionResult> Delete(int id = 0)
        {
            using (var context = new S2GamesContext())
            {
                var repository = new GameRepository(context);

                var game = await repository.GetByIdAsync(id, ConnectedId);

                if (game == null)
                    return HttpNotFound();

                context.Dispose();
                return View(game);
            }
        }

        [HttpPost]
        [RequireConnection]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id = 0)
        {
            try
            {
                using (var context = new S2GamesContext())
                {
                    var repository = new GameRepository(context);

                    var game = await repository.GetByIdAsync(id, ConnectedId);

                    if (game == null)
                        return HttpNotFound();

                    context.Games.Remove(game);

                    await context.SaveChangesAsync();

                    context.Dispose();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                TransportData();
            }

            return RedirectToAction("Index");
        }

        [RequireConnection]
        public async Task<ActionResult> LendGame(int friendId, int gameId)
        {
            try
            {
                using (var context = new S2GamesContext())
                {
                    var repository = new GameRepository(context);
                    await repository.LendOrReturn(friendId, gameId, ConnectedId);

                    await context.SaveChangesAsync();
                    context.Dispose();

                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json("Emprestado com sucesso", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [RequireConnection]
        public async Task<ActionResult> ReturnGame(int gameId)
        {
            try
            {
                using (var context = new S2GamesContext())
                {
                    var repository = new GameRepository(context);
                    await repository.LendOrReturn(null, gameId, ConnectedId);

                    await context.SaveChangesAsync();
                    context.Dispose();

                    Response.StatusCode = (int)HttpStatusCode.OK;
                    return Json("Devolvido com sucesso", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        
    }
}