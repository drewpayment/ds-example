using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Dominion.Utility.DataExport.Misc;
using Dominion.Utility.Mapping.DataReader;
using Dominion.Utility.Msg.Specific;
using Dominion.Utility.OpResult;

namespace Dominion.Utility.ExtensionMethods
{
    public static class DataSetTableExtensionMethods
    {
        /// <summary>
        /// Convert an embedded xml resource into a dataset.
        /// </summary>
        /// <param name="embeddedDatasetXml"></param>
        /// <returns></returns>
        public static DataSet DataSetFromXmlResource(this byte[] embeddedDatasetXml)
        {
            var ds = new DataSet();
            using(var ms = new MemoryStream(embeddedDatasetXml))
            {
                ds.ReadXml(ms);
            }

            return ds;
        }

        /// <summary>
        /// Generate a dataset from a path to an xml file.
        /// </summary>
        /// <param name="xmlFilePath">A path to an xml file that represents a dataset.</param>
        /// <returns></returns>
        public static DataSet DataSetFromPath(this string xmlFilePath)
        {
            var ds = new DataSet();
            ds.ReadXml(xmlFilePath);  
            return ds;
        }
        
        /// <summary>
        /// Generate a dataset from a xml string.
        /// </summary>
        /// <param name="xmlString">The xml string.</param>
        /// <returns></returns>
        public static DataSet DataSetFromXmlStr(this string xmlString)
        {
            var ds = new DataSet();
            var sr = new StringReader(xmlString);
            ds.ReadXml(sr);  
            return ds;
        }

        /// <summary>
        /// Create a dataset from a path to an xml file.
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        public static DataSet OpenDataSetFromXml(this string xmlPath)
        {
            DataSet dataSet = new DataSet();
            dataSet.ReadXml(xmlPath);
            return dataSet;
        }

        /// <summary>
        /// Create a dataset from a path to an xml file.
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable OpenDataTableFromDataSetXml(this string xmlPath, string tableName)
        {
            try
            {
                DataSet dataSet = new DataSet();
                dataSet.ReadXml(xmlPath);
                return dataSet.Tables[tableName];
            }
            catch (Exception)
            {
            }

            return null;
        }

        /// <summary>
        /// Returns a list of string that contain html ready to be written to file.
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static IEnumerable<string> ToHtml(this DataSet ds)
        {
            var list = new List<string>();

            foreach (DataTable dt in ds.Tables)
                list.Add(ToHtml(dt)); 

            return list;
        }

        /// <summary>
        /// Convert a datatable into an html string.
        /// Idea for viewing the data in the datatable by export the string to a viewer (or file).
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        public static string ToHtml(this DataTable dt)
        {
            if (dt == null)
                return string.Empty;

            //Get a worker object.
            var myBuilder = new StringBuilder();

            //Open tags and write the top portion.
            myBuilder.AppendLine("<html xmlns='http://www.w3.org/1999/xhtml'>");
            myBuilder.AppendLine("<head>");
            myBuilder.AppendLine("<title>");
            myBuilder.AppendLine(dt.TableName.IsNull(() => string.Empty));
            myBuilder.AppendLine(string.Empty);
            myBuilder.AppendLine("</title>");
            myBuilder.AppendLine("</head>");
            myBuilder.AppendLine("<body>");
            myBuilder.AppendLine("<table border='1px' cellpadding='5' cellspacing='0' ");
            myBuilder.AppendLine("style='border: solid 1px Silver; font-size: x-small;'>");

            //Add the headings row.
            myBuilder.AppendLine("<tr align='left' valign='top'>");

            foreach (DataColumn myColumn in dt.Columns)
            {
                string colInfo = string.Format("{0} - {1}", myColumn.ColumnName, myColumn.DataType.Name);
                myBuilder.AppendLine("<td align='left' valign='top'>");
                myBuilder.AppendLine(colInfo);
                myBuilder.AppendLine("</td>");
            }

            myBuilder.AppendLine("</tr>");

            //Add the data rows.
            foreach (DataRow myRow in dt.Rows)
            {
                myBuilder.AppendLine("<tr align='left' valign='top'>");

                foreach (DataColumn myColumn in dt.Columns)
                {
                    myBuilder.AppendLine("<td align='left' valign='top'>");
                    myBuilder.AppendLine(myRow[myColumn.ColumnName].ToString());
                    myBuilder.AppendLine("</td>");
                }

                myBuilder.AppendLine("</tr>");
            }

            //Close tags.
            myBuilder.AppendLine("</table>");
            myBuilder.AppendLine("</body>");
            myBuilder.AppendLine("</html>");

            return myBuilder.ToString();;
        }

        /// <summary>
        /// Determine the string widths of all the values for each column in a data table.
        /// </summary>
        /// <param name="dt">The dt.</param>
        /// <returns></returns>
        public static Dictionary<string, int> DetermineColStringWidths(this DataTable dt, int increaseBy = 0, IEnumerable<string> colsToIgnore = null)
        {
            var dict = new Dictionary<string, int>();

            foreach (DataColumn col in dt.Columns)
            {
                if(colsToIgnore != null)
                {
                    if(colsToIgnore.Contains(col.ColumnName))
                        continue;
                }

                var strings = dt.AsEnumerable().Select (x => 
                    x.IsNull(col.ColumnName) 
                    ? DataExportConstants.SQL_NULL
                    : x.Field<object>(col.ColumnName).ToString()).ToList();

                strings.Add(col.Caption);

                var longest = strings.OrderByDescending( s => s.Length ).First().Length;
                dict.Add(col.ColumnName, longest + increaseBy);
            }

            return dict;
        }

        /// <summary>
        /// Set the name and return the data table for your chaining or object passing needs.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DataTable SetName(this DataTable dt, string name)
        {
            dt.TableName = name;
            return dt;
        }

        /// <summary>
        /// Set the name and return the dataset for your chaining or object passing needs.
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static DataSet SetName(this DataSet ds, string name)
        {
            ds.DataSetName = name;
            return ds;
        }

        /// <summary>
        /// Returns the value, of type T, from the <see cref="IDataReader"/>, accounting for both generic and non-generic types.
        /// This should be called within a <see cref="IDataReader.Read"/> loop when an actual record is available.
        /// </summary>
        /// <typeparam name="T">T, type applied</typeparam>
        /// <param name="row">The <see cref="IDataReader"/> object that queried the database.</param>
        /// <param name="columnName">The column of data to retrieve a value from</param>
        /// <param name="defaultValue">Optional. Default value to return if the object is null or <see cref="DBNull"/>.</param>
        /// <returns>T, type applied; default value of type if database value is null</returns>
        /// <remarks>
        /// Derived From: <see cref="DataReaderExtensionMethods.GetValueOrDefault{T}"/>
        /// </remarks>
        public static T GetValueOrDefault<T>(this DataRow row, string columnName, T defaultValue = default(T))
        {
            // Read the value out of the reader by string (column name); returns object
            var value = row[columnName];

            // Check for null value from the datasource 
            // If value was null in the datasource, return the default value for T; this will vary based on what T is (i.e. int has a default of 0)
            // otherwise convert & return
            return value != DBNull.Value ? value.ConvertTo(defaultValue: defaultValue) : defaultValue;

        }

        /// <summary>
        /// Converts a <see cref="DataSet"/> to an <see cref="IDataReader"/>.  Then maps the results to an enumeration.
        /// </summary>
        /// <typeparam name="T">Type to map the dataset to.</typeparam>
        /// <param name="ds">DataSet to map.</param>
        /// <param name="mapper">Optional. Mapper to use to convert the dataset to objects.  If not provided a 
        /// default mapper will be used.</param>
        /// <returns></returns>
        public static IEnumerable<T> AsEnumerable<T>(this DataSet ds, IDataReaderMapper<T> mapper = null)
        {
            return ds.CreateDataReader().AsEnumerable(mapper);
        }

        /// <summary>
        /// Converts a <see cref="DataTable"/> to an <see cref="IDataReader"/>.  Then maps the results to an enumeration.
        /// </summary>
        /// <typeparam name="T">Type to map the dataset to.</typeparam>
        /// <param name="dt">DataTable to map.</param>
        /// <param name="mapper">Optional. Mapper to use to convert the datatable to objects.  If not provided a 
        /// default mapper will be used.</param>
        /// <returns></returns>
        public static IEnumerable<T> AsEnumerable<T>(this DataTable dt, IDataReaderMapper<T> mapper = null)
        {
            return dt.CreateDataReader().AsEnumerable(mapper);
        }

        /// <summary>
        /// Validates the given table exists in the dataset and has the specified columns.
        /// </summary>
        /// <param name="ds">Dataset to validate.</param>
        /// <param name="tableIndex">Index of the table to validate.</param>
        /// <param name="columnNames">Columns the table should contain.</param>
        /// <returns></returns>
        public static IOpResult ValidateTable(this DataSet ds, int tableIndex, params string[] columnNames)
        {
            var result = new OpResult.OpResult();

            // verify the table exists
            var table = result.TryGetData(() => ds.Tables[tableIndex]);

            if (result.Success)
            {
                // verify the table contains the specified columns
                result.CombineSuccessAndMessages(table.HasColumns(columnNames));
            }

            return result;

        }

        /// <summary>
        /// Verifies the table has the specified columns.
        /// </summary>
        /// <param name="table">Data table to validate.</param>
        /// <param name="columnNames">Columns the table should contain.</param>
        /// <returns></returns>
        public static IOpResult HasColumns(this DataTable table, params string[] columnNames)
        {
            var result = new OpResult.OpResult();

            foreach (var col in columnNames.Where(col => !table.Columns.Contains(col)))
                result.AddMessage(new DataColumnNotFoundMsg(table, col)).SetToFail();

            return result;
        }

        /// <summary>
        /// Verifies that the dataset has data.
        /// </summary>
        /// <param name="ds">Dataset to validate.</param>
        /// <returns></returns>
        public static bool HasData(this DataSet ds)
        {
            return ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0;
        }

        /// <summary>
        /// Returns the first row of the dataset.
        /// 
        /// Be sure to use HasData() before calling  this!
        /// </summary>
        /// <param name="ds">The data set to return the first row from.</param>
        /// <returns></returns>
        public static DataRow FirstRow(this DataSet ds)
        {
            return ds.Tables[0].Rows[0];
        }

        public static bool TableHasData(this DataSet ds, int tableIndex = 0)
        {
            return ds.Tables.Count > 0 && ds.Tables[tableIndex].Rows.Count > 0;
        }
    }
}


