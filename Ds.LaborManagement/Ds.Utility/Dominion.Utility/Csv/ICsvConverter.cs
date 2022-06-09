using System.Collections.Generic;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using Dominion.Utility.OpResult;

namespace Dominion.Utility.Csv
{
    /// <summary>
    /// Represents an object capable of converting CSV data into the specified type.
    /// </summary>
    /// <typeparam name="T">Type the CSV data is converted to.</typeparam>
    public interface ICsvConverter<T> where T : class
    {
        /// <summary>
        /// Converts CSV data into a collection of the specified type.
        /// </summary>
        /// <param name="config">CSV configuration used to convert the CSV data.</param>
        /// <returns></returns>
        Task<IOpResult<IEnumerable<T>>> Convert(Configuration config = null);
    }
}