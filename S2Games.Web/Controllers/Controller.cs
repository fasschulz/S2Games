using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace S2Games.Web.Controllers
{
    public class Controller : System.Web.Mvc.Controller
    {
        protected int ItemsPerPage = 10;

        public Controller() : base()
        {
        }

        protected void TransportData()
        {
            TempData["ViewData"] = ViewData;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            if (TempData["ViewData"] != null)
                ViewData = (ViewDataDictionary)TempData["ViewData"];
        }

        protected int ConnectedId
        {
            get
            {
                int connectedId = 0;

                if (Session["ConnectedId"] != null)
                {
                    int.TryParse(Session["ConnectedId"].ToString(), out connectedId);
                }

                return connectedId;
            }
            set
            {
                Session["ConnectedId"] = value;
            }
        }
    }
}