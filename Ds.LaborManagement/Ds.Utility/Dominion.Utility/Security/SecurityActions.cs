using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Services;

namespace Dominion.Utility.Security
{
	public static class SecurityActions
	{
		/// <summary>
		/// Logouts out user and returns the redirect URL
		/// </summary>
		/// <returns></returns>
		public static string LogOut()
		{
			var module = FederatedAuthentication.WSFederationAuthenticationModule;

			// clear the local cookie
			module.SignOut(false);

			//TODO:  Not signing out of all other apps.
			// initiate a federated sign out request to the Security Token Service (STS)
			// which will sign out of the STS and other applications using the STS.
			var request = new SignOutRequestMessage(new Uri(module.Issuer), module.Realm);

			return request.WriteQueryString();

		}
	}
}
