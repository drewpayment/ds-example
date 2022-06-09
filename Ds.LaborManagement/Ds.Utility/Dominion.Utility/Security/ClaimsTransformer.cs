using System.IdentityModel.Services;
using System.IdentityModel.Tokens;
using System.Security.Claims;

namespace Dominion.Utility.Security
{
    public class ClaimsTransformer : ClaimsAuthenticationManager
    {
        /// <summary>
        /// Populate the authenticated user with any additional claims needed by the system.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="incomingPrincipal"></param>
        /// <returns></returns>
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            if (!incomingPrincipal.Identity.IsAuthenticated)
            {
                return base.Authenticate(resourceName, incomingPrincipal);
            }

            EstablishSession(incomingPrincipal);

            return incomingPrincipal;
        }

        /// <summary>
        /// This method is responsible for creating the session token and writing the auth cookie to the users computer.
        /// TODO:  Implement sliding expiration if needed, based on the user.
        /// </summary>
        /// <param name="newPrincipal"></param>
        private void EstablishSession(ClaimsPrincipal newPrincipal)
        {
            var sessionToken = new SessionSecurityToken(newPrincipal);


            // TODO:  May need to add a lifetime value here.  Ex: ,TimeSpan.FromHours(8)
            FederatedAuthentication.SessionAuthenticationModule.WriteSessionTokenToCookie(sessionToken);
        }
    }
}