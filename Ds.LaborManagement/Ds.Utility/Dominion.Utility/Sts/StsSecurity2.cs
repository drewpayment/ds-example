using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Dominion.Utility.Sts
{
    public class StsSecurity2
    {
        public const string EcodeCookieName = "ECODE";

        public static void WriteEcodeCookie(string ecode)
        {
            var cookie = new HttpCookie(EcodeCookieName, ecode)
            {
                Secure = true,
                HttpOnly = true,
                Path = HttpRuntime.AppDomainAppVirtualPath,
                Domain = "localhost",
                Shareable = true,
            };

            HttpContext.Current.Response.Cookies.Add(cookie);
        }

    }
}
