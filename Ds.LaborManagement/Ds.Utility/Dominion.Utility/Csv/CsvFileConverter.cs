using System.Collections.Generic;
using System.IO.Abstractions;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using Dominion.Utility.OpResult;

namespace Dominion.Utility.Csv
{
    /// <summary>
    /// Converts CSV data into the specified type from a file location.
    /// </summary>
    /// <typeparam name="T">Type to convert the CSV data to.</typeparam>
    public class CsvFileConverter<T> : CsvConverter<T> where T : class
    {
        private readonly string _filePath;
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Instantiates a new <see cref="CsvFileConverter&lt;"/>.
        /// </summary>
        /// <param name="filePath">Full path to the file containing the CSV data to convert.</param>
        /// <param name="fileSystem">The file system to use to access the file path. If null, the standard System.IO 
        /// tools will be used. This parameter is mainly for testing.</param>
        public CsvFileConverter(string filePath, IFileSystem fileSystem = null)
        {
            _filePath = filePath;
            _fileSystem = fileSystem ?? CsvUtilities.FileSystem;
        }

        /// <summary>
        /// Converts the CSV data to the specified type using the configuration provided.
        /// </summary>
        /// <param name="config"><see cref="Configuration"/> used to convert the file to the specified type. If null, will use the default configuration.</param>
        /// <returns></returns>
        public override Task<IOpResult<IEnumerable<T>>> Convert(Configuration config = null)
        {
            return CsvUtilities.LoadObjectsFromCsv<T>(_filePath, config, _fileSystem);
        }
    }
}