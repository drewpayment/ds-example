using System;
using System.Collections.Generic;
using System.IdentityModel.Services;
using System.Security.Claims;
using System.Web;
using System.Web.Security;
using Dominion.Utility.Configs;

namespace Dominion.Utility.Sts
{
    public class StsSecurity
    {
        #region Constants

        /// <summary>
        /// The name of the ecode qry string parameter
        /// </summary>
        public const string EcodeQryStrParam = "ecode";

        /// <summary>
        /// 
        /// </summary>
        public const string ErrorQryStrParam = "error";

        /// <summary>
        /// I'm pretty sure this account for all instances of this string for error codes.
        /// </summary>
        public const string ErrorCodeAccess = "ACCESS";

        /// <summary>
        /// There are instances of this string hard coded in javascript.
        /// I have replaced all other hard coded instances with this constant.
        /// If you need to search for the usages of this error param; do a full search.
        /// </summary>
        public const string ErrorCodeTimeout = "TIMEOUT";

        #endregion

        #region Logout Methods

        /// <summary>
        /// This will log the user out if they don't have the provided role.
        /// </summary>
        /// <param name="role"></param>
        public static void LogoutAccess(string role)
        {
            if(!HttpContext.Current.User.IsInRole(role))
                Logout(DominionStsErrorCode.Access);
        }

        /// <summary>
        /// Logs out and sets ecode to Access.
        /// </summary>
        public static void LogoutAccess()
        {
            Logout(DominionStsErrorCode.Access);
        }

        /// <summary>
        /// Logs out and sets the ecode to Timeout.
        /// </summary>
        public static void LogoutTimeout()
        {
            Logout(DominionStsErrorCode.Timeout);
        }

        /// <summary>
        /// This logs you out of Form's Auth .
        /// Logs you out of STS.
        /// Abandons the current session.
        /// Redirects you to the default or 'customReplyUrl'.
        /// This logs the user out and then presents the STS login page.
        /// </summary>
        /// <param name="ecode"></param>
        /// <param name="customReplyUrl">
        /// The url to take the user when they log back in after this method logs them out.
        /// </param>
        public static void Logout(
            DominionStsErrorCode ecode = DominionStsErrorCode.None,
            string customReplyUrl = null)
        {
            LogoutForms();

            ClientSideClear();

            var stsSignoutUrl = StsLogout(ecode, customReplyUrl);

            //ExpireAllCookies();

            //If we have not already tried to send a response, redirect to Logout
            if (!HttpContext.Current.Response.HeadersWritten)
            {
                HttpContext.Current.Response.Redirect(stsSignoutUrl, true); //throws thread abort
                //TODO: to avoid the thread abort exception, we need to check each page, for events that occur after the post load event (ie, Pre-render event)
                //Some of the events throw exceptions since they use the session that has already been abandoned
            }
        }

        /// <summary>
        /// An applicant non-user is someone that just signed up and now wants to log in.
        /// They're on the page and there is a session, but they never signed in before getting to that page.
        /// This would be a HeaderNoLogin scenario.
        /// We still have to 'logout' the user so they can be logged in properly.
        /// It's weird, and there's probably a different fix; but this fix works for sure.
        /// </summary>
        public static void LogoutApplicantNonUser(bool allowRedirect = true)
        {
            var postLogoutUrl = AddEcodeToUrl(
                GetPostLogoutUrl(), 
                DominionStsErrorCode.Applicant);

            //remove forms auth and abandon session
            LogoutForms();
            
            ClientSideClear();

            //If we have not already tried to send a response, redirect to Logout
            if (allowRedirect && !HttpContext.Current.Response.HeadersWritten)
            {
                //redirect: this will for the applicant to login
                //the default
                HttpContext.Current.Response.Redirect(postLogoutUrl, true);
            }
        }

        /// <summary>
        /// This logs you out of Form's Auth .
        /// Abandons the current session.
        /// This does not log you out of STS.
        /// This does not log you out of the application.
        /// This is used in certain cases (ie. applicant registration) when there is no real user.
        /// </summary>
        public static void LogoutForms()
        {
            //Abandon our Asp.Net Session if we have one.
            if (HttpContext.Current?.Session != null)
                HttpContext.Current.Session.Abandon();
        }

        /// <summary>
        /// Carries out the STS signout.
        /// </summary>
        /// <param name="ecode">
        /// This is a code that helps dictate what to show on the login page.
        /// DO NOT SET THIS IF YOU ARE SETTING THE customReplyUrl.
        /// If you're setting the customReplyUrl add the 'ecode' parameter to your URL.
        /// Make sure to use the constant that defines what the name of the ecode query string parameter should be.
        /// The ecode qry string parameter should be 'ecode' by the way.
        /// </param>
        /// <param name="customReplyUrl">
        /// ONLY USE THIS IF YOUR PAGE WORKS WITH A LOGOUT SCENARIO.
        /// We've had situations in the past where returning to where the user left off was not supported.
        /// Make sure before you use a custom reply url that your page supports it.
        /// </param>
        /// <returns>The ws federation signout URL.</returns>
        private static string StsLogout(
            DominionStsErrorCode ecode = DominionStsErrorCode.None, 
            string customReplyUrl = null)
        {
            // GETTING STS INFORMATION NEEDED FOR LOGOUT
            // GETTING STS INFORMATION NEEDED FOR LOGOUT
            // GETTING STS INFORMATION NEEDED FOR LOGOUT
            //signout the user and get the issuer
            var authModule = FederatedAuthentication.WSFederationAuthenticationModule;
            authModule.SignOut(false);
            var issuer = authModule.Issuer;

            // GETTING THE POST LOGOUT REDIRECT URL
            // case: 5971: logout redirect to default dominion source page (legacy)
            // the legacy root url will only be used on the STS server if the STS server's appSetting 'Environment' is set to 'development'
            // since they're logged in; we know what site they were on because that web.config is setup with that url
            // if it's a normal user, we know 100% certain this is where they wan't to be, and we don't have to rely on the main.dominionsystems.com redirecting them
            var replyUrl = AddEcodeToUrl(
                GetPostLogoutUrl(customReplyUrl ?? authModule.Reply),
                ecode);

            // THE ACTUAL LOG OUT
            // THE ACTUAL LOG OUT
            // THE ACTUAL LOG OUT
            var response = new SignOutRequestMessage(new Uri(issuer), replyUrl);
            return response.WriteQueryString();
        }

        private static void ClientSideClear()
        {
            if (HttpContext.Current == null || HttpContext.Current.Response.HeadersWritten)
                return;

            // Invalidate the Cache on the Client Side
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();
        }
        #endregion

        #region Logout URL Utilities

        /// <summary>
        /// Gets the post logout url.
        /// If null is passed in it will default to app settings in a particular order.
        /// LegacyRootUrl.
        /// MainRedirectUrl.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetPostLogoutUrl(string url = null)
        {
            var postLogoutUrl = url;

            if (string.IsNullOrEmpty(postLogoutUrl))
            {
                postLogoutUrl =
                    ConfigValues.LegacyRootUrl ??
                    ConfigValues.MainRedirectRootUrl;
            }

            return postLogoutUrl;
        }

        /// <summary>
        /// Add the eeecode to the url.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="ecode"></param>
        /// <returns></returns>
        public static string AddEcodeToUrl(string url, DominionStsErrorCode ecode)
        {
            url = url.TrimEnd('/');
            var ecodeQryStr = ecode == DominionStsErrorCode.None
                ? string.Empty
                : $"/?{EcodeQryStrParam}={(int)ecode}";

            //combine the strings to build the URL
            url += ecodeQryStr;
            return url;
        }

        /// <summary>
        /// Builds the url to take the user when they log back in after this method logs them out.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="ecode"></param>
        /// <param name="stsReplyToValue"></param>
        /// <returns></returns>
        public static string BuildPostLogoutUrl(
            string url,
            DominionStsErrorCode ecode = DominionStsErrorCode.None)
        {
            //this checks the url
            var postLogoutUrl = GetPostLogoutUrl(url);

            var ecodeQryStr = ecode == DominionStsErrorCode.None
                ? string.Empty
                : $"/?{EcodeQryStrParam}={(int)ecode}";

            //combine the strings to build the URL
            postLogoutUrl += ecodeQryStr;

            return postLogoutUrl;
        }

        #endregion

        /// <summary>
        /// Converts the ole 'error code' qry string parameter to the new ecode enum.
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static DominionStsErrorCode ConvertTo(string error)
        {
            //            error = error.ToUpper() ?? string.Empty;
            error = (string.IsNullOrEmpty(error)) ? string.Empty : error;
            switch (error)
            {
                case ErrorCodeAccess:
                    return DominionStsErrorCode.Access;
                case ErrorCodeTimeout:
                    return DominionStsErrorCode.Timeout;
                default:
                    return DominionStsErrorCode.None;
            }
        }

        /// <summary>
        /// Determines if the user is at their home site as listed in their security claims
        /// Returns the site they need to be redirected to.   If they are OK, then it returns empty string
        /// </summary>
        /// <returns></returns>
        public static string CompareUriAndRedirectToHomeSite(
            ref IEnumerable<Claim> claimList, 
            Uri currentUserLocationUri, 
            int numberOfSegmentsToCheck)
        {
            string returnValue = "";
            try
            {
                // Note: Non-sysAdmins will only have 1 homesite claim.  They can only be allowed to go to 1 site.
                foreach (Claim value in claimList)
                {
                    // Extract the Url
                    // String is pipe delimited in the claim.   [Name]|[Url]
                    var claimUrlStringValues = value.Value.Split(new char[] {'|'});

                    var currentClaimUri = new Uri(claimUrlStringValues[1]);
                    
                    // Compare the host.  If the host is OK... then check the segments.
                    if ((currentUserLocationUri.Host.ToUpper().Trim() != currentClaimUri.Host.ToUpper().Trim()))
                    {
                        returnValue = currentClaimUri.AbsoluteUri.ToString();
                        break;
                        // POSSIBLE FUTURE CODE
                        // For index As Integer = 0 To NumberOfSegmentsToCheck
                        //     Dim current_ClaimUriApplicationSegment As String = currentClaimUri.Segments(index).Replace("/", "")
                        //     Dim current_UserLocationUriApplicationSegment As String = currentUserLocationUri.Segments(index).Replace("/", "")
                        //     If current_ClaimUriApplicationSegment.ToUpper.Trim <> current_UserLocationUriApplicationSegment.ToUpper.Trim Then
                        //         returnValue = currentClaimUri.AbsoluteUri.ToString()
                        //         FoundAMatch = False
                        //         Exit For
                        //     End If
                        // Next
                    }

                }

            }
            catch (Exception)
            {
                returnValue = ConfigValues.MainRedirectRootUrl; 
            }

            return returnValue;
        }
    }
}


//public static string GetPostLogoutUrl(string url = null)
//{
//    var postLogoutUrl = url?.TrimEnd('/');

//    if (string.IsNullOrEmpty(postLogoutUrl))
//    {
//        postLogoutUrl =
//            ConfigValues.LegacyRootUrl?.TrimEnd('/') ??
//            ConfigValues.MainRedirectRootUrl?.TrimEnd('/');
//    }

//    return postLogoutUrl;
//}

///// <summary>
///// Builds the url to take the user when they log back in after this method logs them out.
///// </summary>
///// <param name="stsReply"></param>
///// <param name="ecode"></param>
///// <param name="customReplyUrl"></param>
///// <returns></returns>
//public static string BuildPostLogoutUrl(
//    string stsReply,
//    DominionStsErrorCode ecode = DominionStsErrorCode.None,
//    string customReplyUrl = null)
//{
//    var postLogoutUrl = 
//        customReplyUrl?.TrimEnd('/') ?? 
//        ConfigValues.LegacyRootUrl?.TrimEnd('/') ??
//        stsReply.TrimEnd('/');

//    var ecodeQryStr = ecode == DominionStsErrorCode.None
//        ? string.Empty
//        : $"/?{EcodeQryStrParam}={(int)ecode}";

//    //combine the strings to build the URL
//    postLogoutUrl += ecodeQryStr;

//    return postLogoutUrl;
//}


//    'Logs Users out locally and Redirects to STS to complete the process.
//FormsAuthentication.SignOut()

//Dim ecode As DminionStsErrorCode = StsSecurity.ConvertTo(Request.QueryString("error"))
//Dim result As String = StsSecurity.Logout(ecode)

//Session.Abandon()
//Response.Redirect(result, True)

///// <summary>
///// 
///// </summary>
///// <param name="errorCode">
///// This is an error code that will be translated into an ECODE.
///// Error codes are the old way, ECODES are the new way.
///// </param>
///// <param name="customReplyUrl"></param>
//[Obsolete("Use the logout that takes the DominionErrorCode enum (overload of this method).")]
//public static void Logout(
//    string errorCode = null, 
//    string customReplyUrl = null)
//{
//    var ecode = StsSecurity.ConvertTo(errorCode ?? string.Empty);
//    Logout(ecode, customReplyUrl);

//    //FormsAuthentication.SignOut();
//    //var ecode = StsSecurity.ConvertTo(errorCode ?? string.Empty);
//    //var stsSignoutUrl = StsLogout(ecode);
//    //HttpContext.Current.Session.Abandon();
//    //HttpContext.Current.Response.Redirect(stsSignoutUrl, true);
//}