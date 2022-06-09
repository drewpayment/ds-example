using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using Dominion.Utility.OpResult;

namespace Dominion.Utility.Csv
{
    /// <summary>
    /// Converts CSV data into the specified type from a <see cref="TextReader"/>.
    /// </summary>
    /// <typeparam name="T">Type to convert the CSV data to.</typeparam>
    public class CsvTextReaderConverter<T> : CsvConverter<T> where T : class
    {
        private readonly TextReader _reader;

        /// <summary>
        /// Instantiates a new <see cref="CsvTextReaderConverter&lt;"/>.
        /// </summary>
        /// <param name="reader"><see cref="TextReader"/> containing the CSV data to convert.</param>
        public CsvTextReaderConverter(TextReader reader)
        {
            _reader = reader;
        }

        /// <summary>
        /// Converts the CSV data to the specified type using the configuration provided.
        /// </summary>
        /// <param name="config"><see cref="CsvConfiguration"/> used to convert the file to the specified type. If null, will use the default configuration.</param>
        /// <returns></returns>
        public override Task<IOpResult<IEnumerable<T>>> Convert(Configuration config = null)
        {
            return CsvUtilities.LoadObjectsFromCsv<T>(_reader, config);
        }
    }
}