using System.Linq;

namespace Dominion.Utility.Query
{
    /// <summary>
    /// Constructs <see cref="IQueryResult{T}"/>(s) on pre-defined data queries.
    /// </summary>
    public interface IQueryResultFactory
    {
        /// <summary>
        /// Constructs a new <see cref="IQueryResult{T}"/> on the provided data query.
        /// </summary>
        /// <typeparam name="T">Data type being queried.</typeparam>
        /// <param name="dataQuery">Data to build the result on.  Query should be pre-filtered and sorted as desired.</param>
        /// <returns></returns>
        IQueryResult<T> BuildResult<T>(IQueryable<T> dataQuery);
    }
}
