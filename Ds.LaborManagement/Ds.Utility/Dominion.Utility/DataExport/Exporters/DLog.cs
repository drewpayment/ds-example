using System.Data;
using System.Diagnostics;
using Dominion.Utility.DataExport.Misc;
using Dominion.Utility.ExtensionMethods;

namespace Dominion.Utility.DataExport.Exporters
{   
    public class DLog
    {
        #region Static Create

        public static DLog INDTS(string fileName)
        {
            var instance = new DLog(PathGen.I); 
            CreateDebugInstanceDefaultLog(ref instance, fileName);
            return instance;
        }

        /// <summary>
        /// Create an instance of this class.
        /// </summary>
        /// <param name="pathGenerator"></param>
        /// <returns></returns>
        public static DLog I(PathGen pathGenerator)
        {
            var instance = new DLog(PathGen.I); 
            CreateDebugInstance(ref instance, pathGenerator);
            return instance;
        }

        #endregion

        #region Properties and Variables

        /// <summary>
        /// The path to write the data to.
        /// </summary>
        private PathGen _pathGen;

        /// <summary>
        /// The log type. This will be set each time a log is requested to be generated.
        /// </summary>
        private LogType _logType;

        /// <summary>
        /// Get the last generated path for this instance.
        /// </summary>
        public string LastExportedPath
        {
            get { return _pathGen.LastGeneratedPath;  }
        }
        
        #endregion

        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="path"></param>
        public DLog(PathGen pathGenerator)
        {
            _pathGen = pathGenerator;
        }

        #endregion

        #region Methods Dataset or DataTable

        /// <summary>
        /// Write dataset or datatable.
        /// </summary>
        /// <param name="dataSetOrTable"></param>
        /// <param name="logType">By default this will be xml</param>
        /// <returns></returns>
        public DLog DSDT(object dataSetOrTable, XmlWriteMode xmlWriteMode = XmlWriteMode.WriteSchema)
        {
            _logType = LogType.DsToXml;
            WriteXml(dataSetOrTable, xmlWriteMode);
            return this;
        }

        /// <summary>
        /// Proxy to write data set or table to xml but only when in debug mode.
        /// </summary>
        /// <param name="dataSetOrTable"></param>
        /// <param name="xmlWriteMode"></param>
        [Conditional(DataExportConstants.CONDITIONAL_DEBUG_TOKEN)]
        private void WriteXml(object dataSetOrTable, XmlWriteMode xmlWriteMode)
        {
            string path = _pathGen.SEXT(DetermineFileExtensionByLogType()).Gen();

            if(dataSetOrTable is DataSet)
                (dataSetOrTable as DataSet).WriteXml(path, xmlWriteMode);
            else
                (dataSetOrTable as DataTable).WriteXml(path, xmlWriteMode);
        }

        #endregion

        #region Methods Json

        /// <summary>
        /// Export an object to json file.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DLog JSON(object obj)
        {
            ExportJson(obj);
            return this;
        }

        /// <summary>
        /// The conditional json export method.
        /// </summary>
        /// <param name="obj"></param>
        [Conditional(DataExportConstants.CONDITIONAL_DEBUG_TOKEN)]
        private void ExportJson(object obj)
        {
            string path = _pathGen.SEXT(DetermineFileExtensionByLogType()).Gen();
            obj.SerializeJson(path);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Figure out the file extension type based on the log type.
        /// </summary>
        private Ext DetermineFileExtensionByLogType()
        {
            switch(_logType)
            {
                case LogType.ObjToJson:
                    return Ext.Json;
                case LogType.DsToXml:
                    return Ext.Xml;
                case LogType.DsToXmlWS:
                    return Ext.Xml;
                case LogType.DsToHtml:
                    return Ext.Html;
                default:
                    return Ext.Txt;
            }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// A proxy to used to build a fully functioning DLog object.
        /// This is to control it with the Conditional("") attribute the best I can.
        /// This ensures that it won't try to write if not in debug mode.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="pathGenerator"></param>
        [Conditional(DataExportConstants.CONDITIONAL_DEBUG_TOKEN)]
        private static void CreateDebugInstance(ref DLog instance, PathGen pathGenerator)
        {
            instance = new DLog(pathGenerator);
        }

        [Conditional(DataExportConstants.CONDITIONAL_DEBUG_TOKEN)]
        private static void CreateDebugInstanceDefaultLog(ref DLog instance, string fileName)
        {
            instance = new DLog(PathGen.INDTS(fileName));
        }

        #endregion

    }
}
