using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace Dominion.LaborManagement.Service.Api
{
    /// <summary>
    /// ''' Used to construct and manipulate a popup window.
    /// ''' </summary>
    /// ''' <remarks>J. DeGram | 2014.08.27 | Case: showModalDialog() deprecated by Google Chrome v37.x</remarks>

    public class Popup
    {
        public string Url { get; set; }
        public string WindowName { get; set; }
        public Int32? Width { get; set; }
        public Int32? Height { get; set; }
        public Int32? Top { get; set; }
        public Int32? Left { get; set; }
        public bool? FullScreen { get; set; }
        public bool? IncludeStatusBar { get; set; }
        public bool? IncludeToolbar { get; set; }
        public bool? IncludeMenuBar { get; set; }
        public bool? IncludeLocationBar { get; set; }
        public bool? IncludeScrollbars { get; set; }
        public bool? AllowResize { get; set; }
        public string PageTitle { get; set; }
        public int? PopupDelayMs { get; set; }

        /// <summary>
        ///     ''' The showPopup() script based on the <see cref="Popup" /> settings.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public string Script
        {
            get
            {
                return GetScript();
            }
        }

        /// <summary>
        ///     ''' Script which returns the results of showPopup() (ie: return false). Can be used to block a button's click event
        ///     ''' postback from occurring if the script is registered as OnClientClick.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public string ScriptWithReturn
        {
            get
            {
                return "return " + GetScript();
            }
        }

        /// <summary>
        ///     ''' JSON object representing the window settings (options) used to construct the popup.
        ///     ''' </summary>
        ///     ''' <value></value>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public string OptionsParamJsonObject
        {
            get
            {
                return GetOptionsParamJsonObject();
            }
        }

        /// <summary>
        ///     ''' Initializes a new <see cref="Popup"/> object.
        ///     ''' </summary>
        ///     ''' <param name="url">URL to open in the popup window.</param>
        ///     ''' <param name="windowName">Name of the window to create.</param>
        ///     ''' <remarks></remarks>
        public Popup(string url, string windowName = null, string pageTitle = "")
        {
            this.Url = url;
            this.WindowName = string.IsNullOrEmpty(windowName) ? DateTime.Now.ToString("YYYYMMddHHmmss") + "_Window" : windowName;
            this.PageTitle = pageTitle;
        }

        /// <summary>
        ///     ''' Creates the showPopup() script based on the <see cref="Popup" /> settings.
        ///     ''' </summary>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public string GetScript()
        {
            string script = "showPopup(";

            script += "'" + Url + "',";
            script += "'" + WindowName + "',";
            script += GetOptionsParamJsonObject() + ",";
            script += "'" + PageTitle + "');";

            return script;
        }

        /// <summary>
        ///     ''' Returns the 'options' JSON object parameter for the showPopup() script call.
        ///     ''' </summary>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        public string GetOptionsParamJsonObject()
        {
            string options = "";

            // Window Options
            options += AddIfProvided(options, "width", this.Width);
            options += AddIfProvided(options, "height", this.Height);
            options += AddIfProvided(options, "left", this.Left);
            options += AddIfProvided(options, "top", this.Top);
            options += AddIfProvided(options, "status", this.IncludeStatusBar);
            options += AddIfProvided(options, "menubar", this.IncludeMenuBar);
            options += AddIfProvided(options, "location", this.IncludeLocationBar);
            options += AddIfProvided(options, "scrollbars", this.IncludeScrollbars);
            options += AddIfProvided(options, "resizable", this.AllowResize);
            options += AddIfProvided(options, "fullscreen", this.FullScreen);
            options += AddIfProvided(options, "delay", this.PopupDelayMs);

            return "{" + options + "}";
        }

        /// <summary>
        ///     ''' Registers the current popup script and opens the popup.
        ///     ''' </summary>
        ///     ''' <param name="page">Page to open the popup from.</param>
        ///     ''' <remarks></remarks>
        public void Open(Page page)
        {
            RegisterScript(page, Script);
        }

        /// <summary>
        ///     ''' Registers a script that will redirect the specified page to the currently configured popup URL.
        ///     ''' </summary>
        ///     ''' <param name="page"></param>
        ///     ''' <remarks></remarks>
        public void RedirectToUrl(Page page)
        {
            RegisterScript(page, "window.location = '" + Url + "';");
        }

        /// <summary>
        ///     ''' Adds the specified option if the value is not null.
        ///     ''' </summary>
        ///     ''' <param name="existingOptions">Current option values already constructed.</param>
        ///     ''' <param name="optionName">Option to add (this will be the name of the JSON object property).</param>
        ///     ''' <param name="value">Value to check if null. If not null, value will be added. Boolean values will be converted to 'yes' or 'no'.</param>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        private string AddIfProvided(string existingOptions, string optionName, object value)
        {
            string opt = "";

            if (value != null)
            {
                if ((!string.IsNullOrEmpty(existingOptions)))
                    opt += ",";

                if (value is bool?)
                    opt += optionName + ":" + (Convert.ToBoolean(value) ? "'yes'" : "'no'");
                else
                    opt += optionName + ":" + value;
            }

            return opt;
        }

        /// <summary>
        ///     ''' Creates a new popup with the specified window options. Only included optional parameters will be set in the 
        ///     ''' generated script.
        ///     ''' </summary>
        ///     ''' <param name="url"></param>
        ///     ''' <param name="windowName"></param>
        ///     ''' <param name="width"></param>
        ///     ''' <param name="height"></param>
        ///     ''' <param name="top"></param>
        ///     ''' <param name="left"></param>
        ///     ''' <param name="fullscreen"></param>
        ///     ''' <param name="includeStatusBar"></param>
        ///     ''' <param name="includeToolbar"></param>
        ///     ''' <param name="includeMenuBar"></param>
        ///     ''' <param name="includeLocationBar"></param>
        ///     ''' <param name="includeScrollbars"></param>
        ///     ''' <param name="allowResize"></param>
        ///     ''' <returns></returns>
        ///     ''' <remarks>See: https://developer.mozilla.org/en-US/docs/Web/API/Window.open for parameter definitions.</remarks>
        public static Popup Create(string url, string windowName = null, Int32? width = default(int?), Int32? height = default(int?), Int32? top = default(int?), Int32? left = default(int?), bool? fullscreen = default(Boolean?), bool? includeStatusBar = default(Boolean?), bool? includeToolbar = default(Boolean?), bool? includeMenuBar = default(Boolean?), bool? includeLocationBar = default(Boolean?), bool? includeScrollbars = default(Boolean?), bool? allowResize = default(Boolean?), string pageTitle = "")
        {
            return new Popup(url, windowName) { Width = width, Height = height, Top = top, Left = left, FullScreen = fullscreen, IncludeStatusBar = includeStatusBar, IncludeToolbar = includeToolbar, IncludeMenuBar = includeMenuBar, IncludeLocationBar = includeLocationBar, IncludeScrollbars = includeScrollbars, AllowResize = allowResize, PageTitle = pageTitle };
        }

        /// <summary>
        ///     ''' Registers a startup script that will close the current popup.
        ///     ''' </summary>
        ///     ''' <param name="page">Popup page.</param>
        ///     ''' <param name="returnVal">Parameter to pass to the window.top.opener's handleClose() function.</param>
        ///     ''' <param name="closeDelayMs">If specified will delay closing the popup by the given number of milliseconds.</param>
        ///     ''' <remarks></remarks>
        public static void Close(Page page, string returnVal = null, int? closeDelayMs = default(int?))
        {
            string script;

            script = GetReturnValueScript(returnVal) + GetCloseScript(closeDelayMs);

            RegisterScript(page, script);
        }

        /// <summary>
        ///     ''' Registers a startup script that will close the current popup and refresh the parent page.
        ///     ''' </summary>
        ///     ''' <param name="page">Popup page.</param>
        ///     ''' <param name="returnVal">Parameter to pass to the window.top.opener's handleClose() function.</param>
        ///     ''' <param name="closeDelayMs">If specified will delay closing the popup by the given number of milliseconds.</param>
        ///     ''' <remarks></remarks>
        public static void CloseAndRefreshParent(Page page, string returnVal = null, int? closeDelayMs = default(int?))
        {
            string script;

            script = GetReturnValueScript(returnVal) + GetParentRefreshScript() + GetCloseScript(closeDelayMs);

            RegisterScript(page, script);
        }

        /// <summary>
        ///     ''' Registers a script that will refresh the parent page.
        ///     ''' </summary>
        ///     ''' <param name="page">Child page.</param>
        public static void RefreshParent(Page page)
        {
            RegisterScript(page, GetParentRefreshScript());
        }

        /// <summary>
        ///     ''' Registers a startup script that will close the current popup after executing the specified javascript.
        ///     ''' </summary>
        ///     ''' <param name="script">The script to execute before closing the popup. Note: Script tags will be added automatically.</param>
        ///     ''' <param name="page">Popup page.</param>
        ///     ''' <param name="returnVal">Parameter to pass to the window.top.opener's handleClose() function.</param>
        ///     ''' <param name="closeDelayMs">If specified will delay closing the popup by the given number of milliseconds.</param>
        ///     ''' <remarks></remarks>
        public static void ExecuteScriptThenClose(Page page, string script, string returnVal = null, int? closeDelayMs = default(int?))
        {
            string s;

            s = script + GetReturnValueScript(returnVal) + GetCloseScript(closeDelayMs);

            RegisterScript(page, s);
        }

        /// <summary>
        ///     ''' Executes the specified script in the popup window.
        ///     ''' </summary>
        ///     ''' <param name="page">Popup page.</param>
        ///     ''' <param name="script">Script to execute. Note: Script tags will be added automatically.</param>
        ///     ''' <param name="returnVal">Parameter to pass to the window.top.opener's handleClose() function.</param>
        ///     ''' <remarks></remarks>
        public static void ExecuteScript(Page page, string script, string returnVal = null)
        {
            string s;

            s = script + GetReturnValueScript(returnVal);

            RegisterScript(page, s);
        }

        /// <summary>
        ///     ''' Calls the handleClose() function on the parent page with the specified returnVal.
        ///     ''' </summary>
        ///     ''' <param name="page">Popup page.</param>
        ///     ''' <param name="returnVal">Parameter to pass to the window.top.opener's handleClose() function.</param>
        ///     ''' <remarks></remarks>
        public static void SendReturnValueToParent(Page page, string returnVal = null)
        {
            RegisterScript(page, GetReturnValueScript(returnVal));
        }

        /// <summary>
        ///     ''' Constructs a script that will call the popup opener's handleClose() function if specified with the given
        ///     ''' return value.
        ///     ''' </summary>
        ///     ''' <param name="returnVal">Parameter to pass to the window.top.opener's handleClose() function.</param>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        private static string GetReturnValueScript(string returnVal = null)
        {
            string script = string.Empty;

            if (returnVal != null)
                script = "if(window.top.opener.handleClose) window.top.opener.handleClose('" + returnVal + "'); ";

            return script;
        }

        /// <summary>
        ///     ''' Constructs a script that will refresh the popup opener's page.
        ///     ''' </summary>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        private static string GetParentRefreshScript()
        {
            return "if(window.top.opener) window.top.opener.location.href = window.top.opener.location;";
        }

        /// <summary>
        ///     ''' Constructs a script that will close the popup window.
        ///     ''' </summary>
        ///     ''' <param name="closeDelayMs">If specified will delay closing the popup by the given number of milliseconds.</param>
        ///     ''' <returns></returns>
        ///     ''' <remarks></remarks>
        private static string GetCloseScript(int? closeDelayMs = default(int?))
        {
            return closeDelayMs != null ? "window.top.close();" : "setTimeout(function(){window.top.close();}, " + closeDelayMs + ");";
        }

        /// <summary>
        ///     ''' Registers the specified script on the given page. Will use <see cref="ScriptManager"></see> if one is present
        ///     ''' on the page.
        ///     ''' </summary>
        ///     ''' <param name="page">Page to register the script on.</param>
        ///     ''' <param name="script">Script to execute on start up. Note: Script tags will be added automatically.</param>
        ///     ''' <remarks></remarks>
        private static void RegisterScript(Page page, string script)
        {
                page.ClientScript.RegisterStartupScript(page.GetType(), "popupScript", script, true);
        }
    }

}
