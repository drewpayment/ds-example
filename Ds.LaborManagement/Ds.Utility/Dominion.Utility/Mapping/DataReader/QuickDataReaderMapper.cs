using System;
using System.Collections.Generic;
using System.Data;

namespace Dominion.Utility.Mapping.DataReader
{
    /// <summary>
    /// Automagically maps a <see cref="IDataReader"/> to a collection of <see cref="TDest"/> objects by matching the 
    /// reader's column names to object property names.
    /// </summary>
    /// <typeparam name="TDest"></typeparam>
    /// <remarks>Uses </remarks>
    public class QuickDataReaderMapper<TDest> : IDataReaderMapper<TDest>
    {
        /// <summary>
        /// Mapping function loaded from cache.
        /// </summary>
        private Func<IDataReader, TDest> mapper; 
 
        public IQuickDataReaderMapProvider<TDest> MapProvider { get; private set; }

        /// <summary>
        /// Used to uniquely identify mappers whose <see cref="IDataReader"/> contains the same column names.  Used
        /// in generating a unique hash code in the mapping cache.
        /// </summary>
        public virtual string Token { get; set; }

        /// <summary>
        /// Instantiates a new mapper.
        /// </summary>
        /// <param name="provider">Provided for testing purposes. If null, defaults to <see cref="QuickDataReaderMapperCache{TDest}.Instance"/>
        /// which provides the static mapping cache.</param>
        public QuickDataReaderMapper(IQuickDataReaderMapProvider<TDest> provider = null)
        {
            MapProvider = provider ?? QuickDataReaderMapperCache<TDest>.Instance;
        } 

        /// <summary>
        /// Maps the records within a <see cref="IDataReader"/> to a collection of <see cref="TDest"/> objects.
        /// </summary>
        /// <param name="reader">Reader to map to objects.</param>
        /// <returns>Collection of mapped objects from the <see cref="IDataReader"/>.</returns>
        public IEnumerable<TDest> Map(IDataReader reader)
        {
            if(this.mapper == null)
                this.mapper = MapProvider.GetObjectCreator(reader, this.Token);

            while(reader.Read())
                yield return this.mapper(reader);
        }
    }
}
