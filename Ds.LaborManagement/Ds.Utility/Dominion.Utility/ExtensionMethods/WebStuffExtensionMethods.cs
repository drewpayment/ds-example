using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dominion.Utility.Constants;
using Dominion.Utility.Msg.Specific;
using Dominion.Utility.OpResult;

namespace Dominion.Utility.ExtensionMethods
{
    public static class WebStuffExtensionMethods
    {
        #region Get User IP Address

        /// <summary>
        /// Get current user ip address.
        /// </summary>
        /// <returns>The IP Address</returns>
        public static string GetUserIPAddress(this HttpContext ctx)
        {
            return GetUserIPAddressDyanmic(ctx);
        }

        public static string GetUserIPAddress(this HttpContextBase ctx)
        {
            return GetUserIPAddressDyanmic(ctx);
        }

        public static string GetUserIPAddress(this HttpRequest ctx)
        {
            return GetUserIPAddressDyanmic(ctx);
        }

        public static string GetUserIPAddress(this HttpRequestBase ctx)
        {
            return GetUserIPAddressDyanmic(ctx);
        }

        private static string GetUserIPAddressDyanmic(dynamic obj)
        {
            var ip = default(string);
            try
            {
                var isHttpCtxObj = obj is HttpContext || obj is HttpContextBase;
                obj = (isHttpCtxObj) ? obj.Request : obj;

                ip = obj?.ServerVariables[CommonConstants.SVR_VARS_HTTP_X_FORWARDED_FOR]?.ToString();
                ip = ip ?? obj?.UserHostAddress;
            }
            catch(Exception)
            {
                //review: auth: AUDIT ??
                ip = CommonConstants.IP_ERROR;
            }

            if(string.IsNullOrEmpty(ip))
                ip = CommonConstants.IP_NOT_FOUND;

            return ip;
        }

        #endregion

        /// <summary>
        /// Use this with the "serviceValidationSummary" validationSummary element on the following page:
        /// CompanyEEOCEport.aspx  Line #36 as of writing.
        /// Also check out line #169 (method btnExport_Click) to see errors being added.
        /// </summary>
        /// <param name="wp">The web page to report the errors on.</param>
        /// <param name="result">The result object containing the messages.</param>
        /// <param name="groupName">Needz to match the groupName attribute value of the validationSummary element in order for it to work.</param>
        public static void AddErrorsToPage(this Page wp, IOpResult result, string groupName, bool skipExceptions = true)
        {
            if(result.HasError)
            {
                foreach(var msg in result.MsgObjects)
                {
                    var skipMsg = msg is BasicExceptionMsg && skipExceptions;

                    if(!skipMsg)
                    {
                        var cv = new CustomValidator();
                        cv.IsValid = false;
                        cv.ErrorMessage = msg.Msg;
                        cv.ValidationGroup = groupName;
                        wp.Validators.Add(cv);                        
                    }

                }
            }
        }

        public static void AddErrorToPage(this Page wp, string errorMessage, string groupName = null)
        {
            var cv = new CustomValidator();
            cv.IsValid = false;
            cv.ErrorMessage = errorMessage;

            if(!string.IsNullOrEmpty(groupName))
                cv.ValidationGroup = groupName;

            wp.Validators.Add(cv);
        }

        public static string GetControlThatCausedPostBack(this Page page)
        {
            Control ctrl = null;

            try
            {
                //get the event target name and find the control
                var ctrlName = page.Request.Params.Get("__EVENTTARGET");

                if(!string.IsNullOrEmpty(ctrlName))
                    ctrl = page.FindControl(ctrlName);
            }
            catch(Exception e)
            {
            }

            //return the control to the calling method
            return ctrl?.ID ?? string.Empty;
        }

        public static string JQID(this Control ctrl)
        {
            return $"$('#{ctrl.ClientID}')";
        }

        public static void ClearAnonymousSession(this Page page)
        {
            try
            {
                FormsAuthentication.SignOut();
                page.Session.Abandon();
            }
            catch (Exception e)
            {
                // ignored
            }
        }

        public static void AddRemoveClass(this AttributeCollection attributes, string cls, bool add)
        {
            if(add)
                attributes.AddClass(cls);
            else
                attributes.RemoveClass(cls);
        }

        public static void AddClass(this AttributeCollection attributes, string cls)
        {
            var curClasses = attributes["class"] ?? string.Empty;

            attributes.Add("class", string.Join(" ", 
                curClasses
                .Split(' ')
                .Except(new []{"",cls})
                .Concat(new []{cls})
                .ToArray()
            ));
        }

        public static void RemoveClass(this AttributeCollection attributes, string cls)
        {
            var curClasses = attributes["class"] ?? string.Empty;

            attributes.Add("class", string.Join(" ", 
                curClasses
                .Split(' ')
                .Except(new []{"",cls})
                .ToArray()
            ));
                
        }


        public static string GetAbsoluteUrl(this HttpRequest request)
        {

            var pageName = Path.GetFileName(request.PhysicalPath);

            if (HttpContext.Current == null)
            {
                return pageName;
            }

            if (VirtualPathUtility.IsAbsolute(pageName))
            {
                return
                  request.Url.Scheme + "://"
                  + request.Url.Authority
                  + request.ApplicationPath
                  + pageName;
            }

            return
                request.Url.Scheme + "://"
                + request.Url.Authority
                + VirtualPathUtility.ToAbsolute(pageName);
        }

    }
}
