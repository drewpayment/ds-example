using System.Web;
using System.Web.Mvc;
using Dominion.Utility.Configs;
using Dominion.Utility.Constants;
using Dominion.Utility.Sts;
using Dominion.Utility.Sts.Owin;
using Dominion.Utility.Web;
using Dominion.WebApi.Core.Models;
using Microsoft.Owin.Security;

namespace Dominion.CompanyManagement.Web.Controllers
{
    [Authorize]
    public class RootController : Controller
    {
        /// <summary>
        /// Serves up the index page containing the Admin site's Angular JS app.  Has a CatchAll route so any unhandled 
        /// routes will serve up the Angular app (allows an route handled by angular to be refreshed). 
        /// </summary>
        /// <returns></returns>
        [Route("~/")]    // GET /
        [Route("Index")] // GET /Home/Index
        public ActionResult Index()
        {
            return View("Index");
        }

        //[Authorize]
        [Route("Login")]
        public ActionResult Login()
        {
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Logs the user out of the Admin site and redirects to the Dominion STS with any error codes as necessary.
        /// </summary>
        /// <param name="ecode">Error code used by the STS to display error messages as appropriate (such as Timeout)
        /// </param>
        /// <returns></returns>
        [Route("Logout")]
        public ActionResult Logout(DominionStsErrorCode ecode = DominionStsErrorCode.None)
        {
            return Redirect(ApplicationLinks.BuildUrl(ConfigValues.LegacyRootUrl, "Logout.aspx"));
        }

        /// <summary>
        /// Redirects to the legacy site specified in the Web.config app setting.  This allows app HTML files to reference
        /// a relative path in order to link to the legacy system. (Eg: &lt;a href='Legacy' target='_parent' /&gt;)
        /// </summary>
        /// <returns></returns>
        [Route("Legacy/{*catchAll}")]
        public ActionResult Legacy(string catchAll = CommonConstants.EMPTY_STRING)
        {
            var query = this.Request.Url == null ? CommonConstants.EMPTY_STRING : this.Request.Url.Query;
            return Redirect(System.Configuration.ConfigurationManager.AppSettings["legacyRootUrl"]  + "/" + catchAll + query);
        }

        /// <summary>
        /// This handles all routes that are defined in angular but aren't defined at the server.
        /// For example. If the user goes to the address bar, enters a valid angular URL, this will run to IIS to look for the proper route.
        /// Since the angualr URL's are virtual, and not defined here as a route on IIS; this CATCHALL will handle the request and return to the client properly to finish the client side angular request.
        /// </summary>
        /// <returns></returns>
        public ActionResult CatchAll()
        {
            return View("Index");
        }
    }
}