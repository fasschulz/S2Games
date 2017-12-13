using S2Games.Database;
using S2Games.Database.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace S2Games.Web
{
    public class RequireConnectionAttribute : ActionFilterAttribute
    {
        public bool ThrowExceptions = false;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["ConnectedId"] == null)
            {
                filterContext.HttpContext.Session.Abandon();
                if (ThrowExceptions)
                    throw new HttpException("Usuário desconectado");
                else
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "Action", "Index" } });
            }
            else
            {
                var id = (int?)filterContext.HttpContext.Session["ConnectedId"];

                var context = new S2GamesContext();
                var repository = new UserRepository(context);

                var user = repository.GetById(id);
                if (user == null)
                {
                    if (ThrowExceptions)
                        throw new HttpException("Usuário inválido");
                    else
                        filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "Controller", "Home" }, { "Action", "Index" } });
                }                
                
            }
        }
    }
}