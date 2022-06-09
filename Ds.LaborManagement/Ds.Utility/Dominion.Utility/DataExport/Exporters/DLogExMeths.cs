using System.Collections.Generic;
using System.Data;
using DebugDataExport.Exporters;

namespace Dominion.Utility.DataExport.Exporters
{
    public static class DLogExMeths
    {
        #region Data Table

        public static Dictionary<int, string> Log_ToInserts(this DataTable dt)
        {
            return SqlInsert.I.GetRows(dt);
        }

        public static IEnumerable<string> Log_ToInsertFiles(this DataTable dt)
        {
            var obj = SqlInsert.I;
            return obj.GenFiles(dt);
        }

        public static string Log_ToXmlFile(this DataTable ds, PathGen pg = null)
        {
            var obj = DsDt.I;
            obj.PathGen = pg ?? obj.PathGen;
            return obj.GenXmlFile(ds);
        }

        public static string Log_ToXmlFile(this DataSet dt, PathGen pg = null)
        {
            var obj = DsDt.I;
            obj.PathGen = pg ?? obj.PathGen;
            return obj.GenXmlFile(dt);
        }

        public static DsDt Log_ToDsDt(this DataTable ds)
        {
            var obj = DsDt.I;
            return obj;
        }

        public static DsDt Log_ToDsDt(this DataSet dt, PathGen pg = null)
        {
            var obj = DsDt.I;
            return obj;
        }

        public static DataTable AsDt(this object dataTableOrDataSet)
        {
            return dataTableOrDataSet as DataTable;
        }

        public static DataSet AsDs(this object dataTableOrDataSet)
        {
            return dataTableOrDataSet as DataSet;
        }

        #endregion

        #region Path Gen 

        public static Json Log_ToJson(this PathGen pg)
        {
            var obj = Json.I;
            obj.PathGen = pg;
            return obj;
        }

        public static DsDt Log_ToDsDt(this PathGen pg)
        {
            var obj = DsDt.I;
            obj.PathGen = pg;
            return obj;
        }

        public static SqlInsert Log_ToSqlIns(this PathGen pg)
        {
            var obj = SqlInsert.I;
            obj.PathGen = pg;
            return obj;
        }

        #endregion

        public static T SNU<T>(this T obj, string name)
            where T : FileExportBase
        {
            obj.PathGen.SNU(name);
            return (T)obj;
        }

        public static T SN<T>(this T obj, string name)
            where T : FileExportBase
        {
            obj.PathGen.SN(name);
            return (T)obj;
        }

        public static T ST<T>(this T obj, string name)
            where T : FileExportBase
        {
            obj.PathGen.ST(name);
            return (T)obj;
        }

        public static T TDTS<T>(this T obj)
            where T : FileExportBase
        {
            obj.PathGen.TDTS();
            return (T)obj;
        }

    }
}
