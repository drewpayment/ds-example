using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

using AutoMapper.QueryableExtensions;

using Dominion.Utility.Mapping;

namespace Dominion.Utility.Query
{
    /// <summary>
    /// Represents the result of an <see cref="IQuery{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type the query is built on.</typeparam>
    public class QueryResult<T> : IQueryResult<T>
    {
        #region Variables and Properties

        private readonly IQueryable<T> _query;

        private readonly IQueryResultFactory _resultFactory;

        /// <summary>
        /// The underlying query object being constructed.
        /// </summary>
        internal IQueryable<T> Query
        {
            get { return _query; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new <see cref="QueryResult{T}"/>
        /// </summary>
        /// <param name="sourceQuery">The query the results will be obtained from.</param>
        /// <param name="resultFactory">Factory used to build new query results for certain query conversions 
        /// (e.g. mapping, joining, etc).</param>
        public QueryResult(IQueryable<T> sourceQuery, IQueryResultFactory resultFactory)
        {
            _query         = sourceQuery;
            _resultFactory = resultFactory;
        }

        #endregion

        /// <summary>
        /// Maps the current query result to a new result using default mappings (eg: Auto-Mapper configurations). 
        /// Note: The query will not be executed until Execute() is called on the QueryResult.
        /// </summary>
        /// <typeparam name="TDest">Type to map the current query set to.</typeparam>
        /// <returns>New query result of the mapped type.</returns>
        public virtual IQueryResult<TDest> MapTo<TDest>()
        {
            return _resultFactory.BuildResult(_query.Project().To<TDest>());
        }

        /// <summary>
        /// Maps the current query result to a new result type using the given map expression.
        /// Note: The query will not be executed until Execute() is called on the QueryResult.
        /// </summary>
        /// <typeparam name="TDest">Type to map the current query set to.</typeparam>
        /// <param name="mapExpression">Mapping expression used to convert the current result to the new type.</param>
        /// <returns>New query result of the mapped type.</returns>
        public virtual IQueryResult<TDest> MapTo<TDest>(Expression<Func<T, TDest>> mapExpression)
        {
            return _resultFactory.BuildResult(_query.Select(mapExpression));
        }

        /// <summary>
        /// Maps the current query result to a new result type using the given mapper.
        /// </summary>
        /// <typeparam name="TDest">Type to map the current query set to.</typeparam>
        /// <param name="mapper">The mapper to use to map from the source type to the destination type.</param>
        /// <returns></returns>
        public virtual IQueryResult<TDest> MapTo<TDest>(IMapper<T, TDest> mapper)
        {
            return _resultFactory.BuildResult(mapper.Map(this._query).AsQueryable());
        }

        /// <summary>
        /// Combines the unique results of two queries.
        /// </summary>
        /// <param name="result">Query result to combine with the current query.</param>
        /// <returns></returns>
        public virtual IQueryResult<T> Union(IQueryResult<T> result)
        {
            var typedResult = (QueryResult<T>)result;
            return _resultFactory.BuildResult(_query.Union(typedResult.Query));
        }

        /// <summary>
        /// Constructs a new query from the current result set.
        /// </summary>
        /// <typeparam name="TQuery">Type of query to construct.</typeparam>
        /// <param name="queryBuilder">Function which constructs the desired query object from the results of the 
        /// active query.</param>
        /// <returns>Newly constructed query on the current result set.</returns>
        public virtual TQuery QueryAs<TQuery>(Func<IEnumerable<T>, TQuery> queryBuilder) where TQuery : IQuery<T>
        {
            return queryBuilder(_query);
        }

        /// <summary>
        /// Executes the currently defined query and returns the results.
        /// </summary>
        /// <returns>Result set of the query.</returns>
        public virtual IEnumerable<T> Execute()
        {
            return _query.ToList();
        }

        /// <summary>
        /// Returns the first result of the current query or null if no results are found.
        /// </summary>
        /// <returns>The first result of the current query or null if no results are found.</returns>
        public virtual T FirstOrDefault()
        {
            return _query.FirstOrDefault();
        }

        /// <summary>
        /// Returns the total number of results that satisfy the current query.
        /// </summary>
        /// <returns>Total number of results that satisfy the current query.</returns>
        public virtual int Count()
        {
            return _query.Count();
        }

        /// <summary>
        /// Returns indication if any records exists matching the current query.
        /// </summary>
        /// <returns>True if records exists; otherwise, false.</returns>
        public virtual bool Any()
        {
            return _query.Any();
        }

        /// <summary>
        /// Wraps the take fuctionality.
        /// </summary>
        /// <param name="numberOfResults">The number of results you want.</param>
        /// <returns></returns>
        public virtual IQueryResult<T> Take(int numberOfResults)
        {
            var qry = _query.Take(numberOfResults);
            return _resultFactory.BuildResult(qry);
        }

        /// <summary>
        /// Allows grouping.
        /// </summary>
        /// <typeparam name="TKey">The group key type.</typeparam>
        /// <typeparam name="TResult">The data type that is being returned; as required by the .net groupby call.</typeparam>
        /// <param name="expressions">The class containing the expression definitions for grouping; as required by the .net groupby call.</param>
        /// <returns>The specified result.</returns>
        public virtual IQueryResult<TResult> Group<TKey, TResult>(
            IGroupExpressions<T, TKey, TResult> expressions)
        {
            var qry = 
                _query
                .GroupBy(expressions.GroupKey)
                .Select(expressions.Select);

            return _resultFactory.BuildResult(qry);
        }

        public IQueryResult<TResult> GroupBy<TKey, TResult>(Expression<Func<T, TKey>> keySelector, Expression<Func<TKey, IEnumerable<T>, TResult>> resultSelector)
        {
            var qry =
                _query
                .GroupBy(keySelector, resultSelector);

            return _resultFactory.BuildResult(qry);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInner">The inner query result type.</typeparam>
        /// <typeparam name="TKey">The join key type.</typeparam>
        /// <typeparam name="TResult">The data type that is being returned; as required by the .net join call.</typeparam>
        /// <param name="innerQuery">The inner query result object.</param>
        /// <param name="expressions">The class containing the expression definitions for grouping; as required by the .net join call.</param>
        /// <returns></returns>
        public virtual IQueryResult<TResult> InnerJoin<TInner, TKey, TResult>(
            IQueryResult<TInner> innerQuery,
            IInnerJoinExpressions<T, TInner, TKey, TResult> expressions)
        {
            var typedInnerQuery = innerQuery as QueryResult<TInner>;
            if(typedInnerQuery == null)
                throw new InvalidCastException("Could not cast to known query type while joining query results.");

            var qry = 
                _query
                .Join(
                    typedInnerQuery._query,
                    expressions.OuterKey,
                    expressions.InnerKey,
                    expressions.Select);

            return _resultFactory.BuildResult(qry);
        }

    }
}
