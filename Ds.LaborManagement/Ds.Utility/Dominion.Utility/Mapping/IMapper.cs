using System;
using System.Collections.Generic;
using Dominion.Utility.OpResult;

namespace Dominion.Utility.Mapping
{
    /// <summary>
    /// Base mapper definition.  Defines the source and destination types represented in the mapping.
    /// </summary>
    public interface IMapper
    {
        /// <summary>
        /// Type the mapper is able to map from.
        /// </summary>
        Type SourceType { get; }

        /// <summary>
        /// Type the mapper is able to map to.
        /// </summary>
        Type DestinationType { get; }
    }

    /// <summary>
    /// Basic mapping which maps one object type to another.
    /// </summary>
    /// <typeparam name="TSource">Source type to map FROM.</typeparam>
    /// <typeparam name="TDest">Destination type to map TO.</typeparam>
    public interface IMapper<in TSource, out TDest> : IMapper
    {
        /// <summary>
        /// Maps an instance of the source-type to the destination-type.
        /// </summary>
        /// <param name="obj">Object containing the source information to map from.</param>
        /// <returns>Object containing the mapped information.</returns>
        TDest Map(TSource obj);

        /// <summary>
        /// Maps a collection of source-type objects to a collection of destination-type objects.
        /// </summary>
        /// <param name="source">Object collection containing the information to map from.</param>
        /// <returns></returns>
        IEnumerable<TDest> Map(IEnumerable<TSource> source); 
    }

}
