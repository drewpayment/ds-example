using System.Collections.Generic;
using System.Linq;

using Dominion.Utility.Threading;

namespace Dominion.Utility.Query
{
    /// <summary>
    /// Thread-safe extension of the base <see cref="QueryResult{T}"/>.  Locks on a shared <see cref="IPadlock"/> 
    /// object during the query execution phase preventing only a single query to be executed at a time accross threads.
    /// Primarily used when using EF datasources as Entity Framework queries are not thread-safe.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ThreadSafeQueryResult<T> : QueryResult<T>
    {
        /// <summary>
        /// Shared object to lock on during query execution. Prevents multiple queries to 
        /// be executed in parallel.
        /// </summary>
        private readonly IPadlock _padlock;

        /// <summary>
        /// Instantiates a new <see cref="ThreadSafeQueryResult{T}"/>.
        /// </summary>
        /// <param name="sourceQuery"></param>
        /// <param name="resultFactory"></param>
        /// <param name="padlock">Shared object to lock on during query execution. Prevents multiple queries to 
        /// be executed in parallel.</param>
        public ThreadSafeQueryResult(IQueryable<T> sourceQuery, IQueryResultFactory resultFactory, IPadlock padlock)
            : base(sourceQuery, resultFactory)
        {
            _padlock = padlock;
        }

        /// <summary>
        /// Returns the total number of results that satisfy the current query.
        /// </summary>
        /// <returns>Total number of results that satisfy the current query.</returns>
        public override int Count()
        {
            lock (_padlock)
            {
                return base.Count();
            }
        }

        /// <summary>
        /// Returns the first result of the current query or null if no results are found.
        /// </summary>
        /// <returns>The first result of the current query or null if no results are found.</returns>
        public override T FirstOrDefault()
        {
            lock (_padlock)
            {
                return base.FirstOrDefault();
            }
        }

        /// <summary>
        /// Executes the currently defined query and returns the results.
        /// </summary>
        /// <returns>Result set of the query.</returns>
        public override IEnumerable<T> Execute()
        {
            lock (_padlock)
            {
                return base.Execute();
            }
        }
    }
}
