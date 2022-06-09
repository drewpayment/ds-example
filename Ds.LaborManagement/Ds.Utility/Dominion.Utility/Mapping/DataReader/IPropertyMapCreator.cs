using System;
using System.Data;

namespace Dominion.Utility.Mapping.DataReader
{
    /// <summary>
    /// Automagically maps a <see cref="IDataReader"/> to a collection of <see cref="TDest"/> objects by matching the 
    /// reader's column names to object property names.
    /// </summary>
    public interface IQuickDataReaderMapProvider<out TDest>
    {
        /// <summary>
        /// Maps the records within a <see cref="IDataReader"/> to a collection of <see cref="TDest"/> objects.
        /// </summary>
        /// <param name="reader">Reader to map to objects.</param>
        /// <param name="mapperToken">Unique token used to specify what mapper to return. If not provided only the 
        /// combination of column names will be used to determine uniqueness.</param>
        /// <returns>Collection of mapped objects from the <see cref="IDataReader"/>.</returns>
        Func<IDataReader, TDest> GetObjectCreator(IDataReader reader, string mapperToken = null);
    }
}
