using System.Collections.Generic;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using Dominion.Utility.OpResult;

namespace Dominion.Utility.Csv
{
    /// <summary>
    /// Base class used to convert CSV data into objects.
    /// </summary>
    /// <typeparam name="T">Type of object to convert CSV data into.</typeparam>
    public abstract class CsvConverter<T> : ICsvConverter<T> where T : class
    {
        /// <summary>
        /// Converts CSV data into a collection of the specified type.
        /// </summary>
        /// <param name="config">CSV configuration used to convert the CSV data.</param>
        /// <returns></returns>
        public abstract Task<IOpResult<IEnumerable<T>>> Convert(Configuration config = null);
    }
}