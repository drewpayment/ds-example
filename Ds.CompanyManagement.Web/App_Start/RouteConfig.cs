using System.Web.Mvc;
using System.Web.Routing;

namespace Dominion.CompanyManagement.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                "Catchall-Last",
                "{*catchAll}",
                new { controller = "Root", action = "CatchAll" },
                null,
                new string[] { "Dominion.Ess.Web.Controllers" });
        }
    }
}
