using System.Collections.Generic;
using System.Data;

namespace Dominion.Utility.Mapping.DataReader
{
    /// <summary>
    /// Represents an object capable of mapping from an <see cref="IDataReader"/> to the target type <see cref="TDest"/>.
    /// </summary>
    /// <typeparam name="TDest">Type to be mapped to.</typeparam>
    public interface IDataReaderMapper<out TDest>
    {
        /// <summary>
        /// Maps the records within a <see cref="IDataReader"/> to a collection of <see cref="TDest"/> objects.
        /// </summary>
        /// <param name="reader">Reader to map to objects.</param>
        /// <returns>Collection of mapped objects from the <see cref="IDataReader"/>.</returns>
        IEnumerable<TDest> Map(IDataReader reader);
    }
}
