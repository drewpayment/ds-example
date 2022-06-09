using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.ExtensionMethods;

namespace Dominion.Utility.Sts
{
    /// <summary>
    /// Dominion STS error codes used to set the error status on the STS login page.
    /// </summary>
    public enum DominionStsErrorCode
    {
        /// <summary>
        /// NADA
        /// </summary>
        None    = 0,

        /// <summary>
        /// UMM
        /// </summary>
        Access  = 1,

        /// <summary>
        /// Timeout Scenario.
        /// </summary>
        Timeout = 2,

        /// <summary>
        /// HMMM
        /// </summary>
        General = 3,
        
        /// <summary>
        /// Applicant scenario.
        /// </summary>
        Applicant = 4, 
    }

    public static class DominionStsErrorCodeExtensions
    {
        /// <summary>
        /// Returns the specified error code as a query string parameter 
        /// (eg: DominionStsErrorCode.Timeout => 'ecode=2')
        /// </summary>
        /// <param name="code">Error code to convert to query string.</param>
        /// <returns></returns>
        public static string AsQueryString(this DominionStsErrorCode code)
        {
            return StsSecurity.EcodeQryStrParam + "=" + code.ToValueString();
        }

        /// <summary>
        /// Adds the specified STS error code as a query string parameter to an existing URL.
        /// </summary>
        /// <param name="code">Error code to convert to query string.</param>
        /// <param name="url">URL to append the </param>
        /// <returns></returns>
        public static string AddToUrl(this DominionStsErrorCode code, string url)
        {
            var uri = new Uri(url);
            if(string.IsNullOrEmpty(uri.Query))
                return url + ("?" + code.AsQueryString());

            return url + ("&" + code.AsQueryString());
        }
    }
}
