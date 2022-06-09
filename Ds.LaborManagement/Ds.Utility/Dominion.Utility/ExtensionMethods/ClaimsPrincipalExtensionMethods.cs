using System;
using System.Linq;
using System.Security.Claims;
using System.Web.Configuration;
using Dominion.Utility.Security;
using Dominion.Utility.Transform;

namespace Dominion.Utility.ExtensionMethods
{
    /// <summary>
    /// This class contains method definitions that extend ClaimsPrincipal objects
    /// </summary>
    public static class ClaimsPrincipalExtensionMethods
    {
        /// <summary>
        /// Get the value for the UserId claim.
        /// </summary>
        /// <param name="principal">The principal object from which to retrieve the UserId claim.</param>
        /// <returns>The value for the UserId claim, or zero if there's no such claim.</returns>
        public static int GetUserIdClaim(this ClaimsPrincipal principal)
        {
            return GetIntegerClaim(principal, DominionClaimTypes.UserId);
        }

        /// <summary>
        /// Get the value for the ClientId claim.
        /// </summary>
        /// <param name="principal">The principal object from which to retrieve the ClientId claim.</param>
        /// <returns>The value for the ClientId claim, or zero if there's no such claim.</returns>
        public static int GetClientIdClaim(this ClaimsPrincipal principal)
        {
            return GetIntegerClaim(principal, DominionClaimTypes.ClientId);
        }

        /// <summary>
        /// Get the value for the EmployeeId claim.
        /// </summary>
        /// <param name="principal">The principal object from which to retrieve the EmployeeId claim.</param>
        /// <returns>The value for the EmployeeId claim, or zero if there's no such claim.</returns>
        public static int GetEmployeeIdClaim(this ClaimsPrincipal principal)
        {
            return GetIntegerClaim(principal, DominionClaimTypes.EmployeeId);
        }

        /// <summary>
        /// Get the value for the UserType claim.
        /// </summary>
        /// <param name="principal">The principal object from which to retrieve the EmployeeId claim.</param>
        /// <returns></returns>
        public static int GetUserTypeIdClaim(this ClaimsPrincipal principal)
        {
            return GetIntegerClaim(principal, DominionClaimTypes.UserTypeId);
        }

        public static string GetHomeSiteClaim(this ClaimsPrincipal principal)
        {
            return principal?.Claims?.FirstOrDefault(c => c.Type == DominionClaimTypes.HomeSite)?.Value;
        }

        public static byte GetSiteConfigurationIdClaim(this ClaimsPrincipal principal)
        {
            return GetByteClaim(principal, DominionClaimTypes.SiteConfigId);
        }

        internal static byte GetByteClaim(ClaimsPrincipal principal, string claimType)
        {
            byte cliamValue = 0;
            Claim claim = principal.Claims.FirstOrDefault(c => c.Type == claimType);

            if (claim != null)
            {
                try
                {
                    cliamValue = Convert.ToByte(claim.Value);
                }
                catch (Exception)
                {
                    // do nothing.
                }
            }

            return cliamValue;
        }

        /// <summary>
        /// Get the integer value for the given claim.
        /// </summary>
        /// <param name="principal">The principal object from which to retrieve the claim value.</param>
        /// <returns>The integer value for the requested claim. Zero is returned if there's no such claim.</returns>
        internal static int GetIntegerClaim(ClaimsPrincipal principal, string claimType)
        {
            int cliamValue = 0;
            Claim claim = principal.Claims.FirstOrDefault(c => c.Type == claimType);

            if (claim != null)
            {
                try
                {
                    cliamValue = Convert.ToInt32(claim.Value);
                }
                catch (Exception)
                {
                    // do nothing.
                }
            }

            return cliamValue;
        }

    }
}