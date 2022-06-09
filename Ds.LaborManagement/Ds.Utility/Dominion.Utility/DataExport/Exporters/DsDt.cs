using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Utility.DataExport.Exporters;
using Dominion.Utility.DataExport.Misc;
using Dominion.Utility.ExtensionMethods;

namespace DebugDataExport.Exporters
{
    public class DsDt : FileExportBase
    {
        #region Static Create

        public static DsDt I
        {
            get
            {
                var instance = new DsDt();
                return instance;
            }
        }

        #endregion

        /// <summary>
        /// Write the dataset or table as xml to file.
        /// </summary>
        /// <param name="dataSetOrTable"></param>
        /// <param name="xmlWriteMode"></param>
        /// <returns></returns>
        public string GenXmlFile(object dataSetOrTable, XmlWriteMode xmlWriteMode = XmlWriteMode.WriteSchema)
        {
            GeneratedPaths.Clear();
            WriteXmlFile(dataSetOrTable, xmlWriteMode);
            return base.GeneratedPaths.FirstOrDefault() ?? string.Empty;
        }

        public IEnumerable<string> GenHtmlFile(object dataSetOrTable, string tag)
        {
            GeneratedPaths.Clear();
            WriteHtmlFile(dataSetOrTable, tag);
            return base.GeneratedPaths;
        }

        /// <summary>
        /// Proxy to write data set or table to xml but only when in debug mode.
        /// </summary>
        /// <param name="dataSetOrTable"></param>
        /// <param name="xmlWriteMode"></param>
        [Conditional(DataExportConstants.CONDITIONAL_DEBUG_TOKEN)]
        private void WriteXmlFile(object dataSetOrTable, XmlWriteMode xmlWriteMode)
        {
            PathGen.SEXT(LogType.DsToXml);
            //SetDatasetName(dataSetOrTable as DataSet); //sets if no current name and dataset
            SetTableName(dataSetOrTable as DataTable); //sets if no current name and datatable
            SetPathName(dataSetOrTable);

            if(dataSetOrTable is DataSet)
            {
                if(string.IsNullOrEmpty(PathGen.Name))
                    PathGen.SN((dataSetOrTable.AsDs()).DataSetName);

                dataSetOrTable.AsDs().WriteXml(PathGen.Gen(), xmlWriteMode);
            }
            else
            {
                if(string.IsNullOrEmpty(PathGen.Name))
                {
                    PathGen.SN(dataSetOrTable.AsDt().TableName);
                }

                dataSetOrTable.AsDt().WriteXml(PathGen.Gen(), xmlWriteMode);
            }

            GeneratedPaths.Add(PathGen.LastGeneratedPath);
        }

        [Conditional(DataExportConstants.CONDITIONAL_DEBUG_TOKEN)]
        private void WriteHtmlFile(object dataSetOrTable, string tag)
        {
            PathGen.SEXT(LogType.DsToHtml);
            SetTableName(dataSetOrTable as DataTable); //sets if no current name and datatable
            SetPathName(dataSetOrTable);

            if(dataSetOrTable is DataSet)
            {
                if(string.IsNullOrEmpty(PathGen.Name))
                    PathGen.SN((dataSetOrTable.AsDs()).DataSetName);

                var counter = 1;
                var tablesHtml = dataSetOrTable.AsDs().ToHtml();

                foreach(var html in tablesHtml)
                {
                    var path = PathGen.ST(tag + counter).Gen();
                    File.WriteAllText(path, html);
                    GeneratedPaths.Add(PathGen.LastGeneratedPath);            
                    counter++;
                }
            }
            else
            {
                if(string.IsNullOrEmpty(PathGen.Name))
                    PathGen.SN(dataSetOrTable.AsDt().TableName);

                var tableHtml = dataSetOrTable.AsDt().ToHtml();
                var path = PathGen.ST(tag).Gen();
                File.WriteAllText(path, tableHtml);
                GeneratedPaths.Add(PathGen.LastGeneratedPath); 
            }

        }

        ///// <summary>
        ///// Make sure the dataset and all of it's tables are named.
        ///// </summary>
        ///// <param name="ds"></param>
        ///// <param name="name"></param>
        ///// <param name="overrideName"></param>
        //private void SetDatasetName(DataSet ds, string name = null, bool overrideName = false)
        //{
        //    if(ds != null)
        //    {
        //        if(string.IsNullOrEmpty(name))
        //            name = "DataSet1";

        //        if(overrideName && string.IsNullOrEmpty(ds.DataSetName))
        //            ds.DataSetName = name;

        //        var counter = 1;
        //        foreach(DataTable table in ds.Tables)
        //        {
        //            SetTableName(table, "Table"+counter);
        //            counter++;
        //        }
        //    }
        //}

        /// <summary>
        /// Make sure the table is named.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="name"></param>
        /// <param name="overrideName"></param>
        private void SetTableName(DataTable dt, string name = null, bool overrideName = false)
        {
            if(dt != null)
            {
                if(string.IsNullOrEmpty(name))
                    name = "Table1";

                if(overrideName || string.IsNullOrEmpty(dt.TableName))
                    dt.TableName = name;
            }
        }

        /// <summary>
        /// Set the path's file name.
        /// </summary>
        /// <param name="dataSetOrTable"></param>
        private void SetPathName(object dataSetOrTable)
        {
            if(dataSetOrTable is DataSet)
            {
                if(string.IsNullOrEmpty(PathGen.Name))
                    PathGen.SN((dataSetOrTable.AsDs()).DataSetName);
            }
            else
            {
                if(string.IsNullOrEmpty(PathGen.Name))
                    PathGen.SN(dataSetOrTable.AsDt().TableName);
            }
        }

    }
}
