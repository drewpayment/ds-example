using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Dominion.Utility.Mapping;

namespace Dominion.Utility.Query
{
    /// <summary>
    /// Represents the result of an <see cref="IQuery"/>.
    /// </summary>
    /// <typeparam name="T">Type the query is built on.</typeparam>
    public interface IQueryResult<T>
    {
        /// <summary>
        /// Maps the current query result to a new result using default mappings (eg: Auto-Mapper configurations). 
        /// Note: The query will not be executed until Execute() is called on the QueryResult.
        /// </summary>
        /// <typeparam name="TDest">Type to map the current query set to.</typeparam>
        /// <returns>New query result of the mapped type.</returns>
        IQueryResult<TDest> MapTo<TDest>();

        /// <summary>
        /// Maps the current query result to a new result type using the given map expression.
        /// Note: The query will not be executed until Execute() is called on the QueryResult.
        /// </summary>
        /// <typeparam name="TDest">Type to map the current query set to.</typeparam>
        /// <param name="mapExpression">Mapping expression used to convert the current result to the new type.</param>
        /// <returns>New query result of the mapped type.</returns>
        IQueryResult<TDest> MapTo<TDest>(Expression<Func<T, TDest>> mapExpression);

        /// <summary>
        /// Maps the current query result to a new result type using the given mapper.
        /// </summary>
        /// <typeparam name="TDest">Type to map the current query set to.</typeparam>
        /// <param name="mapper">The mapper to use to map from the source type to the destination type.</param>
        /// <returns></returns>
        IQueryResult<TDest> MapTo<TDest>(IMapper<T, TDest> mapper); 
            
        /// <summary>
        /// Combines the unique results of two queries.
        /// </summary>
        /// <param name="result">Query result to combine with the current query.</param>
        /// <returns></returns>
        IQueryResult<T> Union(IQueryResult<T> result);

        /// <summary>
        /// Constructs a new query from the current result set.
        /// </summary>
        /// <typeparam name="TQuery">Type of query to construct.</typeparam>
        /// <param name="queryBuilder">Function which constructs the desired query object from the results of the 
        /// active query.</param>
        /// <returns>Newly constructed query on the current result set.</returns>
        TQuery QueryAs<TQuery>(Func<IEnumerable<T>, TQuery> queryBuilder) where TQuery : IQuery<T>;

        /// <summary>
        /// Executes the currently defined query and returns the results.
        /// </summary>
        /// <returns>Result set of the query.</returns>
        IEnumerable<T> Execute();

        ///// <summary>
        ///// This will return pre executed result information.
        ///// A query result is a wrapper for a IEnumerable or IQueryable.
        ///// Interally it's always an IQueryable.
        ///// In some we want to take a result and further define it as a query; this method allows us to do that.
        ///// We can take this and build a query object with it if we want.
        ///// </summary>
        ///// <returns></returns>
        //IEnumerable<T> PreExecutionQryState();

        /// <summary>
        /// Returns the first result of the current query or null if no results are found.
        /// </summary>
        /// <returns>The first result of the current query or null if no results are found.</returns>
        T FirstOrDefault();

        /// <summary>
        /// Returns the total number of results that satisfy the current query.
        /// </summary>
        /// <returns>Total number of results that satisfy the current query.</returns>
        int Count();

        /// <summary>
        /// Returns indication if any records exists matching the current query.
        /// </summary>
        /// <returns>True if records exists; otherwise, false.</returns>
        bool Any();

        /// <summary>
        /// Wraps the take functionality.
        /// Do before any execution.
        /// </summary>
        /// <param name="numberOfResults">The number of results you want.</param>
        /// <returns></returns>
        IQueryResult<T> Take(int numberOfResults);

        /// <summary>
        /// Allows grouping.
        /// </summary>
        /// <typeparam name="TKey">The group key type.</typeparam>
        /// <typeparam name="TResult">The data type that is being returned; as required by the .net groupby call.</typeparam>
        /// <param name="expressions">The class containing the expression definitions for grouping; as required by the .net groupby call.</param>
        /// <returns>The specified result.</returns>
        IQueryResult<TResult> Group<TKey, TResult>(
            IGroupExpressions<T, TKey, TResult> expressions);

        IQueryResult<TResult> GroupBy<TKey, TResult>(
            Expression<Func<T,TKey>> keySelector, Expression<Func<TKey, IEnumerable<T>, TResult>> resultSelector);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInner">The inner query result type.</typeparam>
        /// <typeparam name="TKey">The join key type.</typeparam>
        /// <typeparam name="TResult">The data type that is being returned; as required by the .net join call.</typeparam>
        /// <param name="innerQuery">The inner query result object.</param>
        /// <param name="expressions">The class containing the expression definitions for grouping; as required by the .net join call.</param>
        /// <returns></returns>
        IQueryResult<TResult> InnerJoin<TInner, TKey, TResult>(
            IQueryResult<TInner> innerQuery,
            IInnerJoinExpressions<T, TInner, TKey, TResult> expressions);
    }
}