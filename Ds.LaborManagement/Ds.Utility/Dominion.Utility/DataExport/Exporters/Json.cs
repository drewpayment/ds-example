using System.Diagnostics;
using System.Linq;
using System.Threading;
using Dominion.Utility.DataExport.Misc;
using Dominion.Utility.ExtensionMethods;

namespace Dominion.Utility.DataExport.Exporters
{
    public class Json : FileExportBase
    {
        #region Static Create

        public static Json I
        {
            get
            {
                var instance = new Json();
                return instance;
            }
        }

        #endregion

        #region Methods

        public string GenFile<T>(T obj, bool alwaysLog = false, int? depth = null)
            where T : class
        {
            base.GeneratedPaths.Clear();

            if(string.IsNullOrEmpty(base.PathGen.Name))
            {
                var typeName = typeof(T).Name;
                base.PathGen.TDTS().SN(typeName);
            }
            if(alwaysLog == true)
            {
                ExportJsonAlways(obj, depth);
            } else
            {
                ExportJson(obj, depth);
            }
            return base.GeneratedPaths.FirstOrDefault() ?? string.Empty;
        }

        [Conditional(DataExportConstants.CONDITIONAL_DEBUG_TOKEN)]
        private void ExportJson(object obj, int? depth = null)
        {
            ExportJsonAlways(obj, depth);
        }

        private void ExportJsonAlways(object obj, int? depth = null)
        {
            var path = base.PathGen.SEXT(LogType.ObjToJson).Gen();
            Thread.Sleep(1);
            obj.SerializeJson(path, depth);
            base.GeneratedPaths.Add(path);
        }

        #endregion

    }
}
