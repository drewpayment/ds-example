using System;
using Dominion.Domain.Entities.EEOC;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Query on <see cref="EEOCPayBand"/>(s).
    /// </summary>
    public interface IEEOCPayBandQuery : IQuery<EEOCPayBand, IEEOCPayBandQuery>
    {
        /// <summary>
        /// Filters by W2 Year.
        /// </summary>
        /// <param name="w2Year">W2 Year to filter by.</param>
        /// <returns></returns>
        IEEOCPayBandQuery ByW2Year(int w2Year);
        /// <summary>
        /// Orders by W2 Year descending.
        /// </summary>
        /// <returns></returns>
        IEEOCPayBandQuery OrderByW2Year();
        /// <summary>
        /// Filters to exact W2 Year.
        /// </summary>
        /// <param name="w2Year">W2 Year to filter by.</param>
        /// <returns></returns>
        IEEOCPayBandQuery ByActualW2Year(int w2Year);
    }
}
