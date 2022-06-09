using System;
using System.IO;
using System.Threading;
using Dominion.Utility.Constants;
using Dominion.Utility.DataExport.Misc;
using Dominion.Utility.ExtensionMethods;

namespace Dominion.Utility.DataExport.Exporters
{
    public class PathGen
    {
        #region Static Create

        /// <summary>
        /// Create a path generator that builds a file path in the default log location.
        /// Also this sets the name to what you pass in.
        /// Also sets up the date time stamp.
        /// </summary>
        public static PathGen INDTS(string fileName)
        {
            var instance = new PathGen().TDL().SN(fileName).TDTS();
            return instance;
        }

        /// <summary>
        /// Create a path generator that builds a file path in the default log location.
        /// </summary>
        public static PathGen IDL
        {
            get
            {
                var instance = new PathGen().TDL();
                return instance;
            }
        }
    
        /// <summary>
        /// Create a path generator for building a dir or file path.
        /// </summary>
        public static PathGen I
        {
            get
            {
                var instance = new PathGen();
                return instance;
            }
        }

        #endregion

        #region Variables and Properties
        
        private bool _deleteFirst;
        private bool _useDefaultLog;
        private bool _includeDateTimeStamp;
        private bool _includeTicksStamp;
        private DateTime? _dateTimeToUse;
        private Ext _extType = Ext.Txt;

        public string DirName  { get; private set; }
        public string Tag  { get; private set; }
        public string Name { get; private set; }
        public string RootDir { get; set; }

        public string LastGeneratedPath { get; private set; }

        public const string DEFAULT_LOG = "+export";
        public const string PLUS = "+";
        public const string DOT = ".";
        public const string SLASH = "\\";

        #endregion

        #region Constructor

        public PathGen()
        {
            //LastGeneratedPath = string.Empty;
        }

        #endregion

        #region Set Methods

        /// <summary>
        /// Set directory name NOT THE DRIVE.
        /// Correct: export
        /// Incorrect: c:\export
        /// </summary>
        /// <param name="dirName">The name of the directory that will be generated off the system drive.</param>
        /// <returns></returns>
        public PathGen SDIR(string dirName)
        {
            //logging dirs will always be on the sys drive; don't specify a drive only a name
            //the dir will be created for you off the sys drive if it doesn't exist
            DirName = dirName;
            _useDefaultLog = false;
            return this;
        }

        public PathGen SRootDir(string rootDir)
        {
            RootDir = rootDir;
            return this;
        }

        /// <summary>
        /// Set file name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="et"></param>
        /// <returns></returns>
        public PathGen SN(string name, Ext et)
        {
            Name = name;
            _extType = et;
            return this;
        }

        public PathGen SN(string name)
        {
            Name = name;
            return this;
        }

        /// <summary>
        /// Set the name with a GUID for uniqeness.
        /// Useful for using in threaded situations.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PathGen SNU(string name)
        {
            Name = name + CommonConstants.DASH + Guid.NewGuid();
            return this;
        }

        /// <summary>
        /// Set file name tag. 
        /// This will prefix the file name with a tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public PathGen ST(string tag)
        {
            Tag = tag;
            return this;
        }

        /// <summary>
        /// Set file name tag. 
        /// This will prefix the file name with a tag.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public PathGen SDT(DateTime? dateTime)
        {
            _dateTimeToUse = dateTime;
            return this;
        }

        /// <summary>
        /// Set the extension enum to set the extension of the file that is generated.
        /// </summary>
        /// <param name="et"></param>
        /// <returns></returns>
        public PathGen SEXT(Ext et)
        {
            _extType = et;
            return this;
        }

        /// <summary>
        /// Set the extension enum to set the extension of the file that is generated.
        /// This will be based on the log type passed in. It will default to the appropriate log type extension.
        /// </summary>
        /// <param name="logType">Log type.</param>
        /// <returns></returns>
        public PathGen SEXT(LogType logType)
        {
            _extType = DetermineFileExtensionByLogType(logType);
            return this;
        }

        #endregion

        #region Toggle Methods

        /// <summary>
        /// Toggle default log.
        /// </summary>
        /// <returns></returns>
        public PathGen TDL()
        {
            _useDefaultLog = !_useDefaultLog;
            return this;
        }

        /// <summary>
        /// Toggle delete first.
        /// </summary>
        /// <returns></returns>
        public PathGen TDF()
        {
            _deleteFirst = !_deleteFirst;
            return this;
        }

        /// <summary>
        /// Toggle date and time stamp
        /// </summary>
        /// <returns></returns>
        public PathGen TDTS()
        {
            _includeDateTimeStamp = !_includeDateTimeStamp;
            return this;
        }

        /// <summary>
        /// Toggle date
        /// </summary>
        /// <returns></returns>
        public PathGen TTS()
        {
            _includeTicksStamp = !_includeTicksStamp;
            return this;
        }

        #endregion

        #region Generation Methods

        /// <summary>
        /// Generate the path.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="joinString"></param>
        /// <returns></returns>
        public string Gen(PathGen obj = null, string joinString = SLASH)
        {
            //this will delete the file if it exists
            var path = default(string);

            if(obj != null)
            {
                LastGeneratedPath = 
                    BuildPath() +
                    joinString +
                    obj.BuildPath();
            }
            else
            {
                LastGeneratedPath = BuildPath();
            }
                
            if(_deleteFirst)
                File.Delete(LastGeneratedPath);

            return LastGeneratedPath;
        }

        /// <summary>
        /// Generate the directory path.
        /// </summary>
        public string LogDirectoryPath
        {
            get
            {
                var dirPath =
                    string.IsNullOrEmpty(DirName)
                        ? Path.GetPathRoot(Environment.SystemDirectory) + DEFAULT_LOG
                        : Path.GetPathRoot(Environment.SystemDirectory) + DirName;

                return dirPath;
            }
        }

        /// <summary>
        /// Build a path based on the variables.
        /// </summary>
        /// <returns></returns>
        private string BuildPath()
        {
            //directory:    {drive}\{log dir}\[date stamp]+[date stamp]+[project dir]\
            //file:         [date stamp]+[timestamp]+[prefix]_[tag]_[name].[ext]    

            //tag=0, name=1, ext=2
            var path = string.Format(
                "{0}{1}{2}{3}",
                BuildDateTimeStamp(),
                Tag != null ? Tag+PLUS : string.Empty,
                Name.IsNull(() => string.Empty),
                DOT+_extType.ToString().ToLower());

            if(_useDefaultLog)
                path = LogDirectoryPath.IfNoDirectoryCreateIt()+SLASH+path;

            if(!string.IsNullOrWhiteSpace(DirName))
            {
                var defaultLogDir = Path.GetPathRoot(RootDir ?? Environment.SystemDirectory)+DirName;
                path = LogDirectoryPath.IfNoDirectoryCreateIt()+SLASH+path;
            }

            return path;
        }

        /// <summary>
        /// Build a date time stamp based on the variables.
        /// </summary>
        /// <returns></returns>
        private string BuildDateTimeStamp()
        {
            var dateStamp = string.Empty;
            var timeStamp = string.Empty;
            Thread.Sleep(3);
            var dt = _dateTimeToUse ?? DateTime.Now;

            if (_includeTicksStamp)
            {
                return dt.Ticks+PLUS;
            }

            if(_includeDateTimeStamp)
            {
                dateStamp = dt.ToString("yyyy-MM-dd" + PLUS);
                timeStamp = dt.ToString("HH-mm-ss.ffff"+PLUS);
            }

            return dateStamp + timeStamp;
        }

        /// <summary>
        /// Figure out the file extension type based on the log type.
        /// </summary>
        private Ext DetermineFileExtensionByLogType(LogType logType)
        {
            switch(logType)
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

    }
}
