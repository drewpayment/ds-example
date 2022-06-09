using Dominion.Core.InjectionBindings.Binders;
using Dominion.Utility.Configs;
using Dominion.Utility.Security;
using Serilog;
using SerilogWeb.Classic.Enrichers;
using System.Web.Mvc;
using System.Web.Routing;

namespace Dominion.CompanyManagement.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Log.Logger = new LoggerConfiguration()
                .Enrich.With<HttpRequestIdEnricher>()
                .Enrich.WithUserName()
                .Enrich.WithClaimValue(DominionClaimTypes.UserId, "UserId")
                .Enrich.WithWebApiControllerName()
                .Enrich.WithHttpRequestTraceId()
#if DEBUG
                .WriteTo.Console()
#endif
                .WriteTo.Seq(ConfigValues.SeqUrl, apiKey: ConfigValues.SeqKey)
                .CreateLogger();

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Register Views
            RegisterCustomViews();
        }

        protected void Application_End()
        {
            Log.CloseAndFlush();
        }

        private static void RegisterCustomViews()
        {
            var rve = new RazorViewEngine();
            var viewLocations = new string[]
            {
                "~/WebCore/dist/ds-company/Index.cshtml"
            };

            rve.PartialViewLocationFormats = rve.ViewLocationFormats = rve.MasterLocationFormats = viewLocations;

            rve.AreaMasterLocationFormats =
                rve.AreaPartialViewLocationFormats = rve.AreaViewLocationFormats = viewLocations;

            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(rve);
        }
    }
}
