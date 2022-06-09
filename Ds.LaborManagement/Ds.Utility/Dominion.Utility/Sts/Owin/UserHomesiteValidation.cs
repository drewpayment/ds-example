using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Dominion.Utility.Configs;
using Dominion.Utility.Constants;
using Dominion.Utility.ExtensionMethods;
using Microsoft.Owin;
using Microsoft.Owin.Security.Notifications;

namespace Dominion.Utility.Sts.Owin
{
    /// <summary>
    /// Middleware used to check for a wsignoutcleanup1.0 request and signout accordingly.  Important: Should be 
    /// registered AFTER all other Authentication OWIN middleware.
    /// </summary>
    public class UserHomesiteValidation : OwinMiddleware
    {
        /// <summary>
        /// Initializes a new <see cref="WSignoutCleanupMiddleware"/>.
        /// </summary>
        /// <param name="next">The next middleware to be executed in the OWIN pipeline.</param>
        public UserHomesiteValidation(OwinMiddleware next)
            : base(next)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task Invoke(IOwinContext context)
        {

            var returnUrl = CheckUserHomesiteAndRedirect(context);

            if(returnUrl.Trim() != "")
            {
                context.Response.Redirect(returnUrl);
              }

            // otherwise allow the normal pipeline to handle the request
            return this.Next.Invoke(context);
        }


        /// <summary>
        /// This will extract the homesite value from the users claim and compare it with the users current location.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string CheckUserHomesiteAndRedirect(IOwinContext context)
        {
            const int USER_TYPE_ID_SYSTEM_ADMIN = 1;

            if (ConfigValues.ActivateRedirection)
            {
                var redirectTo = "";

                try
                {
                    ClaimsPrincipal principal = HttpContext.Current.User as ClaimsPrincipal;
                   
                    if (null != principal)
                    {
                        var userTypeIdClaim = principal.Claims.FirstOrDefault(x => x.Type == ConfigValues.DominionClaimType_UserTypeId);
                        var homeSiteClaim = principal.Claims.Where(x => x.Type == ConfigValues.DominionClaimType_HomeSite);

                        if(userTypeIdClaim != null)
                        {
                            if (Convert.ToInt16( userTypeIdClaim.Value ) != USER_TYPE_ID_SYSTEM_ADMIN)
                            {
                                var NumberOfSegmentsToCheck = ConfigValues.NumberOfUriSegmentsToCheckForRedirection;
                                redirectTo = CompareUriAndRedirectToHomeSite(homeSiteClaim, HttpContext.Current.Request.Url, NumberOfSegmentsToCheck); 
                            }
                        }
                    }
                }
                catch { }

                return redirectTo;
            }

            return "";
        }


        /// <summary>
        /// Determines if the user is at their home site as listed in their security claims
        /// Returns the site they need to be redirected to. 
        /// </summary>
        /// <remarks></remarks>
        public static string CompareUriAndRedirectToHomeSite(IEnumerable<Claim> claimList, System.Uri currentUserLocationUri, int numberOfSegmentsToCheck)
        {
            string returnValue = "";
            var foundAMatch = false;

            try
            {
                //Note: Non-sysAdmins will only have 1 homesite claim.  They can only be allowed to go to 1 site.
                foreach (Claim value in claimList)
                {
                    //Extract the Url
                    //String is pipe delimited in the claim.   [Name]|[Url]
                    string[] claimUrlStringValues = value.Value.Split(new char[] { '|' });
                    var currentClaimUri = new Uri(claimUrlStringValues[1]);

                    //Compare the host.
                    if (currentUserLocationUri.Host.ToUpper().Trim() != currentClaimUri.Host.ToUpper().Trim())
                    {
                        returnValue = currentClaimUri.AbsoluteUri;
                        break;
                    }
                }
            }
            catch
            {
                returnValue = ConfigValues.MainRedirectRootUrl;
            }

           return returnValue;
        }
    }
}