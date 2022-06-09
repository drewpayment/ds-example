using System;
using System.Security.Claims;

namespace Dominion.Utility.Security
{
    public static class ClaimsPrincipalExtensions
    {
        /// <summary>
        /// Try to get the specified claim value from the given principal.
        /// </summary>
        /// <typeparam name="TClaim">The data type of the target claim.</typeparam>
        /// <param name="principal">The principal from which to retrieve the claim.</param>
        /// <param name="claimType">The claim designation.</param>
        /// <param name="claimValue">Set to the value of the specified claim.</param>
        /// <returns>True if the claim was found and successfully converted to the desired type.</returns>
        public static bool TryGetClaimValue<TClaim>(this ClaimsPrincipal principal, string claimType, 
            out TClaim claimValue)
        {
            claimValue = default(TClaim);

            try
            {
                var claim = principal.FindFirst(claimType);
                if (claim != null)
                {
                    claimValue = (TClaim) Convert.ChangeType(claim.Value, typeof (TClaim));
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // TryGetClaimValue<TClaim>()
    }
}