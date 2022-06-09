using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.ExtensionMethods;
using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin;

namespace Dominion.Utility.Sts.Owin
{
    /// <summary>
    /// Middleware used to highjack a WSFederation signout response message to add custom Dominion STS query parameters.
    /// </summary>
    public class DominionStsErrorCodeLogger : OwinMiddleware
    {
        /// <summary>
        /// Initializes a new <see cref="DominionStsErrorCodeLogger"/>.
        /// </summary>
        /// <param name="next">The next middleware to be executed in the OWIN pipeline.</param>
        public DominionStsErrorCodeLogger(OwinMiddleware next) : base(next)
        { }

        /// <summary>
        /// Called during the OWIN pipeline. Adds a custom STS error code parameter if a wsfed signout request/response 
        /// is being executed.
        /// </summary>
        /// <param name="context">OWIN context containing the request/response details.</param>
        /// <returns></returns>
        public override async Task Invoke(IOwinContext context)
        {
            // allow the normal wsfed authentication handler to do its signout magic
            await Next.Invoke(context);
            
            // validate: this is a signout request/response
            if(context.Authentication.AuthenticationResponseRevoke != null)
            {
                // validate: there is an STS error code to be set 
                DominionStsErrorCode ecodeVal;
                var props = context.Authentication.AuthenticationResponseRevoke.Properties;
                if (props.TryGetDominionStsErrorCode(out ecodeVal) && ecodeVal != DominionStsErrorCode.None) 
                { 
                    // highjack the redirect response from the wsfed handler
                    var resp = context.Response;
                    var stsSignoutUri = resp.Headers.Get("location");

                    // validate: we are dealing with a wsfed signout message
                    var wsFedMsg = WsFederationMessage.FromUri(new Uri(stsSignoutUri));
                    if (wsFedMsg.IsSignOutMessage)
                    { 
                        // append wsfed signout url w/ custom error code parameter & redirect 
                        resp.Redirect(ecodeVal.AddToUrl(stsSignoutUri));
                    }
                }
            }
        }
    }
}
