using System.Linq;

using Dominion.Utility.Threading;

namespace Dominion.Utility.Query
{
    /// <summary>
    /// Builds thread-safe <see cref="IQueryResult{T}"/>(s) by passing a shared <see cref="IPadlock"/> object to each
    /// result to lock on during query execution.  Used for EF datasources which are not thread-safe.
    /// </summary>
    public class ThreadSafeQueryResultFactory : IQueryResultFactory
    {
        /// <summary>
        /// Shared object to lock on during query execution. Prevents multiple queries to 
        /// be executed in parallel.
        /// </summary>
        private readonly IPadlock _padlock;

        /// <summary>
        /// Instantiates a new <see cref="ThreadSafeQueryResultFactory"/>.
        /// </summary>
        /// <param name="padlock">Shared object to lock on during query execution. Prevents multiple queries to 
        /// be executed in parallel.</param>
        public ThreadSafeQueryResultFactory(IPadlock padlock)
        {
            _padlock = padlock;
        }

        /// <summary>
        /// Constructs a new <see cref="IQueryResult{T}"/> on the provided data query.
        /// </summary>
        /// <typeparam name="T">Data type being queried.</typeparam>
        /// <param name="dataQuery">Data to build the result on.  Query should be pre-filtered and sorted as desired.</param>
        /// <returns></returns>
        IQueryResult<T> IQueryResultFactory.BuildResult<T>(IQueryable<T> dataQuery)
        {
            return new ThreadSafeQueryResult<T>(dataQuery, this, _padlock);
        }
    }
}
