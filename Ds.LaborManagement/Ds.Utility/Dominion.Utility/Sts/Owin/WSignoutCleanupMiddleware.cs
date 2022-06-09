using System.Configuration;
using System.Threading.Tasks;

using Microsoft.Owin;

namespace Dominion.Utility.Sts.Owin
{
    /// <summary>
    /// Middleware used to check for a wsignoutcleanup1.0 request and signout accordingly.  Important: Should be 
    /// registered AFTER all other Authentication OWIN middleware.
    /// </summary>
    public class WSignoutCleanupMiddleware : OwinMiddleware
    {
        /// <summary>
        /// Initializes a new <see cref="WSignoutCleanupMiddleware"/>.
        /// </summary>
        /// <param name="next">The next middleware to be executed in the OWIN pipeline.</param>
        public WSignoutCleanupMiddleware(OwinMiddleware next) : base(next)
        {
        }

        /// <summary>
        /// Called during the OWIN pipeline. Checks if a wsignoutcleanup1.0 request is being made.  If so, initiates a
        /// Authentication.SignOut() call.
        /// </summary>
        /// <param name="context">OWIN context containing the request/response details.</param>
        /// <returns></returns>
        public override Task Invoke(IOwinContext context)
        {
            // if signout cleanup is being requested (from STS) initiate a SignOut call to all registered 
            // AuthenticationHandlers
            var wa = context.Request.Query.Get("wa");
            if (!string.IsNullOrEmpty(wa) && wa == "wsignoutcleanup1.0")
            {
                context.Authentication.SignOut();
                return context.Response.WriteAsync("signing out");
            }

            // otherwise allow the normal pipeline to handle the request
            return this.Next.Invoke(context);
        }
    }
}