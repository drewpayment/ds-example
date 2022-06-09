using System.Configuration;
using System.Threading.Tasks;

using Microsoft.Owin;

namespace Dominion.Utility.Sts.Owin
{
    /// <summary>
    /// Middleware used to check for a wsignoutcleanup1.0 request and logs off accordingly.
    /// </summary>
    public class WSignoutCleanuptMiddleware : OwinMiddleware
    {
        /// <summary>
        /// Initializes a new <see cref="WSignoutCleanuptMiddleware"/>.
        /// </summary>
        /// <param name="next">The next middleware to be executed in the OWIN pipeline.</param>
        public WSignoutCleanuptMiddleware(OwinMiddleware next) : base(next)
        {
            
        }

        /// <summary>
        /// Called during the OWIN pipeline. Checks if a wsignoutcleanup1.0 request is being made.  If so, performs a
        /// complete logoff.
        /// </summary>
        /// <param name="context">OWIN context containing the request/response details.</param>
        /// <returns></returns>
        public override async Task Invoke(IOwinContext context)
        {
            // if signout cleanup is being requested (from STS) redirect to the logout page to perform a complete signout
            var wa = context.Request.Query.Get("wa");
            if (!string.IsNullOrEmpty(wa) && wa == "wsignoutcleanup1.0")
            {
                // build the logoff url & redirect
                var urlNoQuery = context.Request.Uri.AbsoluteUri.Substring(0, context.Request.Uri.AbsoluteUri.IndexOf('?'));
                var logoutUrl = urlNoQuery + ConfigurationManager.AppSettings["sts:logoutPath"];
                context.Response.Redirect(logoutUrl);
            }

            // otherwise allow the normal pipeline to handle the request
            await this.Next.Invoke(context);
        }
    }
}