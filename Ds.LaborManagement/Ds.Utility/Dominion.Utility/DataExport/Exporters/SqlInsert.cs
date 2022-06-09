using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Dominion.Utility.DataExport.Misc;
using Dominion.Utility.ExtensionMethods;

namespace Dominion.Utility.DataExport.Exporters
{
    public class SqlInsert : FileExportBase
    {
        #region Static Create

        public static SqlInsert I
        {
            get
            {
                var instance = new SqlInsert();
                return instance;
            }
        }

        #endregion

        #region Variables and Properties

        //internal values
        private DataTable _dt;
        private string _tableName;
        private List<string> _colsToInsert = new List<string>(); 
        private Dictionary<string, int> _columnWidths;

        //options
        private bool _setIdentity;
        private IEnumerable<string> _colsToIgnore = new List<string>(); 
        private IDictionary<string, ICustomValueObject> _colValueReplacements;

        #endregion

        #region Constructors and Initializers
        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="tableName"></param>
        public SqlInsert()
        {
            _setIdentity = true;
        }

        #endregion

        #region Set Methods

        /// <summary>
        /// Set information about replacing values for columns when generating the sql statement.
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public SqlInsert SCR(IDictionary<string, ICustomValueObject> dict)
        {
            _colValueReplacements = dict;
            return this;
        }

        /// <summary>
        /// Set the columns you want ignored.
        /// </summary>
        /// <param name="colsToIngore"></param>
        /// <returns></returns>
        public SqlInsert SCTI(IEnumerable<string> colsToIngore)
        {
            _colsToIgnore = colsToIngore;
            return this;
        }

        /// <summary>
        /// These 2 items are needed to do an export.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="tableName"></param>
        private void SetVariables(DataTable dt, string tableName)
        {
            _dt = dt;
            _tableName = tableName;
        }

        #endregion

        #region Toggle Methods

        /// <summary>
        /// Toggle the set identity option.
        /// When this is set you can insert AUTO INCREMENT values explicitly.
        /// </summary>
        /// <returns></returns>
        public SqlInsert TSI()
        {
            _setIdentity = !_setIdentity;
            return this;
        }

        #endregion

        #region Generate Insert Sql

        /// <summary>
        /// Look into the data table and set some variables used to generate the sql.
        /// </summary>
        private void Initialize()
        {
            var sb = new StringBuilder();
            _columnWidths = _dt.DetermineColStringWidths(5, _colsToIgnore);

            //determine the list of columns to include
            foreach(DataColumn col in _dt.Columns)
            {
                if(!_colsToIgnore.Contains(col.ColumnName))
                    _colsToInsert.Add(col.ColumnName);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="on"></param>
        /// <returns></returns>
        private string GenerateSetIdentity(bool on = true)
        {
            if(_setIdentity)
            {
                var onOff = on ? "on" : "off";

                return string.Format(
                    "set identity_insert {0} {1}{2}{2}",
                    _tableName,
                    onOff,
                    Environment.NewLine);
            }

            return string.Empty;
        }

        /// <summary>
        /// Generates the insert into [table] statement and the column (....) list
        /// </summary>
        /// <returns></returns>
        private string GenerateInsertHeader()
        {
            var sb = new StringBuilder();

            //-----------------------------------------
            //add sql insert part
            //-----------------------------------------
            sb.AppendFormat("INSERT INTO {0}\r\n", _tableName)
                .Append(DataExportConstants.OPEN_PARENTH);

            //-----------------------------------------
            //create the columns part
            //-----------------------------------------
            var columnCounter = 1;
            foreach(var colName in _colsToInsert)
            {
                var val = colName.PadStringRight(_columnWidths[colName]);
                sb.Append(val)
                    .Append(columnCounter != _colsToInsert.Count ? DataExportConstants.CSV_SEPARATOR : string.Empty);

                columnCounter++;
            }

            sb.AppendLine(DataExportConstants.CLOSE_PARENTH + " VALUES");

            return sb.ToString();
        }

        /// <summary>
        /// Generates a dictionary of data insert lines.
        /// </summary>
        /// <returns></returns>
        private Dictionary<int, List<string>> GenerateInserts()
        {
            var dict = new Dictionary<int, List<string>>();

            //-----------------------------------------
            //add the value part(s)
            //-----------------------------------------
            var groupCounter = 0;
            var lineCounter = 1;
            var newList = true;
            foreach(DataRow row in _dt.Rows)
            {
                //if it's a new list we need to add it to the dictionary
                if(newList)
                {
                    groupCounter++;
                    dict.Add(groupCounter, new List<string>());
                    newList = false;
                }

                //add line to the current list
                var val = GenerateSingleInsert(row);
                dict[groupCounter].Add(val);
            
                //let the code know on the next iteration it's time to add a new group
                if (lineCounter % 1000 == 0)
                { 
                    newList = true;
                }

                lineCounter++;
            }

            return dict;
        }

        /// <summary>
        /// Generate a single data insert line based on the row passed in.
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        private string GenerateSingleInsert(DataRow row)
        {
            var sb = new StringBuilder();
            sb.Append(DataExportConstants.OPEN_PARENTH); 

            var columnCounter = 1;
            foreach(var colName in _colsToInsert)
            {
                //get the value and pad it
                var val = GetColumnValue(row, _dt.Columns[colName])
                    .PadStringRight(_columnWidths[colName]);

                sb.Append(val);

                if(columnCounter < _colsToInsert.Count)
                    sb.Append(DataExportConstants.CSV_SEPARATOR);

                columnCounter++;
            }

            sb.Append(DataExportConstants.CLOSE_PARENTH);

            return sb.ToString();
        }

        /// <summary>
        /// Get the value from a row by column name padded with the correct spaces to the right.
        /// </summary>
        /// <param name="row">The row of data.</param>
        /// <param name="col">The data column obj.</param>
        /// <returns></returns>
        private string GetColumnValue(DataRow row, DataColumn col)
        {
            var length = _columnWidths[col.ColumnName];
            var value = DataExportConstants.SQL_NULL;

            ICustomValueObject obj;
            if(_colValueReplacements != null && _colValueReplacements.TryGetValue(col.ColumnName, out obj))
            {
                value = obj.GeneralValueString();
            }
            else
            {
                if(col.DataType == typeof(bool))
                {
                    value = 
                        row[col.ColumnName] == DBNull.Value 
                        ? DataExportConstants.SQL_NULL 
                        : (bool)row[col.ColumnName] ? "1" : "0";
                }
                else
                {
                    bool addQuotes = false;
                    addQuotes = addQuotes || (col.DataType == typeof(string));
                    addQuotes = addQuotes || (col.DataType == typeof(DateTime));
                    addQuotes = addQuotes || (col.DataType == typeof(TimeSpan));

                    if(addQuotes)
                    {
                        value = "'"+row[col.ColumnName]+"'";
                    }
                    else
                    {
                        value = row[col.ColumnName].ToString();
                    }
                }
            }

            return value.PadStringRight(length);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Generate a dictionary of inserts.
        /// The number of inserts will vary based on the number of records requested.
        /// 1000 per insert. 
        /// One dictionary entry per 1000 rows to insert.
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetRows(DataTable dt, string tableName = null)
        {
            SetVariables(dt, tableName ?? dt.TableName);
            Initialize();
            var items = new Dictionary<int, string>();
            var inserts = GenerateInserts();
            var setIdentityOn = GenerateSetIdentity(true);
            var setIdentityOff = GenerateSetIdentity(false);
            var header = GenerateInsertHeader();

            var counter = 1;
            foreach(var insertGroup in inserts.Values)
            {
                var sb = new StringBuilder();

                sb.Append(setIdentityOn)
                    .Append(header);

                for(int i = 0; i < insertGroup.Count; i++)
                {
                    sb.Append(insertGroup[i]);

                    if(i+1 != insertGroup.Count)
                        sb.AppendLine(DataExportConstants.CSV_SEPARATOR);
                }

                sb.AppendLine().AppendLine()
                    .Append(setIdentityOff);

                items.Add(counter, sb.ToString());
                counter++;
            }

            return items;
        }

        /// <summary>
        /// Write the sql file(s) to a directory.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GenFiles(DataTable dt, string tableName = null)
        {
            base.GeneratedPaths.Clear();
            WriteSql(dt, tableName);
            return GeneratedPaths;
        }

        /// <summary>
        /// COMPILATION CONDITIONAL: Write the sql file(s) to a directory.
        /// </summary>
        /// <returns></returns>
        [Conditional(DataExportConstants.CONDITIONAL_DEBUG_TOKEN)]
        private void WriteSql(DataTable dt, string tableName)
        {
            var dict = GetRows(dt, tableName);
            var counter = 1;
            foreach(var sql in dict.Values)
            {
                var path = base.PathGen.TDTS().SN(_tableName + "-" + counter).SEXT(Ext.Sql).Gen();
                File.WriteAllText(path, sql);
                GeneratedPaths.Add(path);
                counter++;
            }            
        }

        #endregion

        #region Inner Classes

        public interface ICustomValueObject
        {
            string GeneralValueString();
        }

        public class IncrementableLong : ICustomValueObject
        {
            private long _val;

            public IncrementableLong(long val)
            {
                _val = val;
            }

            public string GeneralValueString()
            {
                var val = _val.ToString();
                _val++;
                return val;
            }
        }

        public class ExplicitVal : ICustomValueObject
        {
            private string _val;

            public ExplicitVal(string val)
            {
                _val = val;
            }

            public string GeneralValueString()
            {
                return _val;
            }
        }

        #endregion

    }
}
