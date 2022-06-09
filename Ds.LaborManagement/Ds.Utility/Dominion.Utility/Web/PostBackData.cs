using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace Dominion.Utility.Web
{
    public class PostBackData
    {
        public const string EVENT_ARGUMENT_KEY = "__EVENTARGUMENT";

        #region STATIC CREATE

        public static PostBackData Create(Page page, ScriptManager sm)
        {
            var obj = new PostBackData(page, sm);
            return obj;
        }

        #endregion

        #region FIELDS

        private readonly Page _page;
        private readonly ScriptManager _sm;

        #endregion

        #region PROPERTIES

        /// <summary>
        /// 1st param from the javascript call.
        /// </summary>
        public List<string> UpdatePanelsToUpdate { get; set; }

        /// <summary>
        /// 2nd param from the javascript call.
        /// </summary>
        public string TargetControlId { get; set; }

        /// <summary>
        /// 3rd param from the javascript call.
        /// </summary>
        public string Args { get; set; }
        
        /// <summary>
        /// This is the value from the script managers key.
        /// </summary>
        public string ScriptManagerDetail { get; set; }

        /// <summary>
        /// The id of the script manager.
        /// </summary>
        public string ScriptManagerId { get; set; }

        #endregion

        public PostBackData(Page page, ScriptManager sm)
        {
            _page = page;
            _sm = sm;
            Initialize();
        }

        private void Initialize()
        {
            ScriptManagerId = ScriptManager.GetCurrent(_page)?.UniqueID;

            if(ScriptManagerId != null)
            {
                ScriptManagerDetail = _page.Request.Form[ScriptManagerId];

                Args = _page.Request.Form[EVENT_ARGUMENT_KEY];

                if(ScriptManagerDetail != null)
                {
                    var list = new List<string>();
                    var parts = ScriptManagerDetail.Split('|').ToList();

                    TargetControlId = parts.Last();
                    parts.RemoveAt(parts.Count - 1);
                    UpdatePanelsToUpdate = parts.Select(x => x).ToList();
                }                
            }

            
        }
    }
}
