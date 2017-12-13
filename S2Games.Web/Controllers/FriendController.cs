using PagedList;
using S2Games.Database;
using S2Games.Database.Models;
using S2Games.Database.Repositories;
using S2Games.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace S2Games.Web.Controllers
{
    public class FriendController : Controller
    {
        [RequireConnection]
        public async Task<ActionResult> Index(int? page, string search = null)
        {
            try
            {
                using (var context = new S2GamesContext())
                {
                    IQueryable<Friend> query = null;

                    if (search == null)
                    {
                        query = context.Friends
                            .Where(f => f.UserId == ConnectedId);
                    }
                    else
                    {
                        query = context.Friends
                            .Where(f => f.Name.Contains(search) && f.UserId == ConnectedId);
                    }
                    
                    var friends = await query.ToListAsync();
                    var pageNumber = page ?? 1;
                    
                    ViewBag.FriendsPagedList = friends.ToPagedList(pageNumber, ItemsPerPage);
                    ViewBag.Search = search;

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
                    var repository = new FriendRepository(context);

                    var friend = await repository.GetByIdAsync(id, ConnectedId);

                    if (friend == null)
                        return HttpNotFound();

                    context.Dispose();
                    return View(friend);
                }
            }
            else
                return View(new Friend { UserId = ConnectedId });
        }

        [HttpPost]
        [RequireConnection]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Friend friend)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var context = new S2GamesContext())
                    {
                        context.Friends.AddOrUpdate(friend);

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
                var repository = new FriendRepository(context);

                var friend = await repository.GetByIdAsync(id, ConnectedId);

                if (friend == null)
                    return HttpNotFound();
                              
                context.Dispose();
                return View(friend);
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
                    var repository = new FriendRepository(context);
                    await repository.DeleteAsync(id, ConnectedId);                    

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
    }
}