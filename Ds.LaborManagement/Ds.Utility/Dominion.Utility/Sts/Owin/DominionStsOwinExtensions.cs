using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.ExtensionMethods;
using Microsoft.Owin.Security;

namespace Dominion.Utility.Sts.Owin
{
    /// <summary>
    /// Extensions used to manage the OWIN pipeline's Dominion STS data.
    /// </summary>
    public static class DominionStsOwinExtensions
    {
        /// <summary>
        /// Sets a custom STS error code on the <see cref="AuthenticationProperties"/> which will be passed to the
        /// OWIN pipeline for further request handling.
        /// </summary>
        /// <param name="properties"><see cref="AuthenticationProperties"/> to set the custom error code on.</param>
        /// <param name="code">Error code to set. This will eventually be added to the STS signout uri query string.</param>
        /// <returns></returns>
        public static AuthenticationProperties SetDominionStsErrorCode(this AuthenticationProperties properties, DominionStsErrorCode code)
        {
            var codeVal = code.ToValueString();

            return SetDominionStsErrorCode(properties, codeVal);
        }

        /// <summary>
        /// Sets a custom STS error code on the <see cref="AuthenticationProperties"/> which will be passed to the
        /// OWIN pipeline for further request handling.
        /// </summary>
        /// <param name="properties"><see cref="AuthenticationProperties"/> to set the custom error code on.</param>
        /// <param name="errorCode">Error code to set. This will eventually be added to the STS signout uri query string.</param>
        /// <returns></returns>
        public static AuthenticationProperties SetDominionStsErrorCode(this AuthenticationProperties properties, string errorCode)
        {
            if(!properties.Dictionary.ContainsKey(StsSecurity.EcodeQryStrParam))
            {
                properties.Dictionary.Add(StsSecurity.EcodeQryStrParam, errorCode);
            }
            else
            {
                properties.Dictionary[StsSecurity.EcodeQryStrParam] = errorCode;
            }

            return properties;
        }

        /// <summary>
        /// Gets a custom STS error code from the <see cref="AuthenticationProperties"/> which can be passed to the
        /// OWIN pipeline for further request handling.
        /// </summary>
        /// <param name="properties"><see cref="AuthenticationProperties"/> to get the custom error code from.</param>
        /// <param name="code">Error code from the <see cref="AuthenticationProperties"/> or DominionStsErrorCode.None 
        /// if no code was set.</param>
        /// <returns></returns>
        public static bool TryGetDominionStsErrorCode(this AuthenticationProperties properties, out DominionStsErrorCode code)
        {
            // make sure the error code is available
            var hasCode = properties != null 
                && properties.Dictionary.ContainsKey(StsSecurity.EcodeQryStrParam);

            code = hasCode ? 
                properties.Dictionary[StsSecurity.EcodeQryStrParam].ToEnum<DominionStsErrorCode>() : 
                default(DominionStsErrorCode);

            return hasCode;
        }
    }
}
