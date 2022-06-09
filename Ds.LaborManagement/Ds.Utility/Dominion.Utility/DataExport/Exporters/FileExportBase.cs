using System.Collections.Generic;

namespace Dominion.Utility.DataExport.Exporters
{
    public class FileExportBase
    {
        #region Variables and Properties
        
        /// <summary>
        /// Local path gen object.
        /// </summary>
        private PathGen _pathGen;

        /// <summary>
        /// Base path generator.
        /// If not set the default path will be used.
        /// </summary>
        public PathGen PathGen
        {
            get
            {
                _pathGen = _pathGen ?? PathGen.IDL;
                return _pathGen;
            }
            set { _pathGen = value; }
        }

        /// <summary>
        /// When a file is written it's path(s) will be recorded; per export generation.
        /// </summary>
        public List<string> GeneratedPaths { get; protected set; }

        #endregion

        #region Constructors and Initializers

        /// <summary>
        /// Constructor.
        /// </summary>
        public FileExportBase()
        {
            GeneratedPaths = new List<string>();
        }

        #endregion

    }
}
