using System;
using System.Data;
using System.Diagnostics;
using Dominion.Utility.DataExport.Misc;
using Dominion.Utility.ExtensionMethods;

namespace Dominion.Utility.DataExport.Exporters
{
    public class SqlQry
    {
        #region Static Create

        public static SqlQry I
        {
            get
            {
                var instance = new SqlQry();
                return instance;
            }
        }

        #endregion

        #region Variables and Properties

        public string TableName { get; private set; }
        public string CustomSql { get; private set; }

        private string _selector;
        private string _where;
        private string _orderBy;

        #endregion

        #region Set Methods

        public SqlQry SSQL(string sql)
        {
            CustomSql = sql;
            return this;
        }

        public SqlQry STN(string tableName)
        {
            TableName = tableName;
            return this;
        }

        public SqlQry SS(string selector)
        {
            _selector = selector;
            return this;
        }

        public SqlQry SW(string where)
        {
            _where = where;
            return this;
        }

        public SqlQry SOB(string orderBy)
        {
            _orderBy = orderBy;
            return this;
        }

        #endregion

        #region Methods

        public string GenSql()
        {
            var sql = GenerateSql();
            return sql;
        }

        public DataTable GenDt(string connStr)
        {
            var dt = default(DataTable);

            SetDataTable(ref dt, connStr);

            return dt;
        }

        public string GenerateSql()
        {
            if(!string.IsNullOrEmpty(CustomSql))
                return CustomSql;

            var sql = string.Format(
                "SELECT {0}{4}FROM {1}{2}{3} ",
                string.IsNullOrEmpty(_selector) ? " * " : _selector,
                TableName,
                string.IsNullOrEmpty(_where) ? string.Empty : Environment.NewLine+"WHERE "+_where+Environment.NewLine,
                string.IsNullOrEmpty(_orderBy)
                    ? string.Empty
                    : Environment.NewLine+"ORDER BY "+_orderBy+Environment.NewLine,
                Environment.NewLine);

            return sql;
        }

        [Conditional(DataExportConstants.CONDITIONAL_DEBUG_TOKEN)]
        private void SetDataTable(ref DataTable dt, string connStr)
        {
            dt = GenSql().ToDataTable(connStr, TableName ?? string.Empty);
        }

        #endregion




    }
}
