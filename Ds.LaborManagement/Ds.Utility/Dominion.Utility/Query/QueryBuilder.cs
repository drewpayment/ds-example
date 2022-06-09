using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dominion.Utility.Constants;
using Dominion.Utility.Pagination;
using Dominion.Utility.Query.LinqKit;

namespace Dominion.Utility.Query
{
    /// <summary>
    /// Class allowing query parameters such as Filters, Pagination, Sorting and Translation 
    /// to be built and passed dynamically.
    /// </summary>
    /// <typeparam name="TSource">Object type the query will be executed on.</typeparam>
    /// <typeparam name="TDest">Object type the executed query will be translated to.</typeparam>
    public class QueryBuilder<TSource, TDest>
        where TSource : class
        where TDest : class
    {
        #region VARIABLES AND PROPERTIES

        private List<ISorter<TSource>> _sortExpressions;
        private PaginationInfo _pageInfo;
        private Expression<Func<TSource, TDest>> _selector;

        private Expression<Func<TSource, bool>> _predicate;

        /// <summary>
        /// Set of "OrderBy"-clause objects the specified source object will be sorted by 
        /// when the query is executed
        /// </summary>
        public List<ISorter<TSource>> SortExpressions
        {
            get { return _sortExpressions; }
        }

        /// <summary>
        /// "Select"-clause lamba expression used to translate the source object to the
        /// destination object when the query is executed
        /// </summary>
        public virtual Expression<Func<TSource, TDest>> Selector
        {
            get { return _selector; }
        }

        /// <summary>
        /// Pagination object containing the individual page information the query should 
        /// return when executed
        /// </summary>
        public PaginationInfo PageInfo
        {
            get { return _pageInfo; }
            set { _pageInfo = value; }
        }

        #endregion // region VARIABLES AND PROPERTIES

        #region CONSTRUCTORS AND INITIALIZATION

        protected QueryBuilder()
        {
            _selector = null;
            _pageInfo = null;
            Init();
        }

        public QueryBuilder(Expression<Func<TSource, TDest>> selector)
        {
            _selector = selector;
            _pageInfo = null;

            Init();
        }

        public QueryBuilder(Expression<Func<TSource, TDest>> selector, PaginationInfo pageInfo)
        {
            _selector = selector;
            _pageInfo = pageInfo;

            Init();
        }

        protected void Init()
        {
            _sortExpressions = new List<ISorter<TSource>>();
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Adds the specified expression to the list of SortExpressions the query will sort by.
        /// </summary>
        /// <typeparam name="TKey">Object type which the query will sort by.</typeparam>
        /// <param name="expression">Lambda expression the query will sort by. (Ex: emp => emp.LastName)</param>
        /// <param name="sortDirection">Specifies whether to sort in Ascending or Descending order.</param>
        /// <param name="Name">Provides a way to name the sort expression.</param>
        /// <returns>The QueryBuilder so it can be extended using fluent syntax.</returns>
        public QueryBuilder<TSource, TDest> AddSortBy<TKey>(
            Expression<Func<TSource, TKey>> expression, 
            SortDirection sortDirection = SortDirection.Ascending, 
            string Name = CommonConstants.EMPTY_STRING)
        {
            return AddSortBy(new SortExpression<TSource, TKey>(expression, sortDirection, Name));
        }

        /// <summary>
        /// Adds the specified expression to the list of SortExpressions the query will sort by.
        /// </summary>
        /// <typeparam name="TKey">Object type which the query will sort by.</typeparam>
        /// <param name="expression">Lambda expression the query will sort by. (Ex: emp => emp.LastName)</param>
        /// <param name="sortDirection">Specifies whether to sort in Ascending or Descending order.</param>
        /// <param name="Name">Provides a way to name the sort expression.</param>
        /// <returns>The QueryBuilder so it can be extended using fluent syntax.</returns>
        public QueryBuilder<TSource, TDest> AddSortBy(ISorter<TSource> sorter)
        {
            _sortExpressions.Add(sorter);

            return this;
        }

        /// <summary>
        /// "And"-s the specified predicate expression to the existing query expression tree.
        /// </summary>
        /// <param name="expr">The expression to "And".</param>
        /// <returns>The QueryBuilder so it can be extended using fluent syntax.</returns>
        public QueryBuilder<TSource, TDest> FilterBy(Expression<Func<TSource, bool>> expr)
        {
            if (expr != null)
            {
                if (_predicate == null)
                    _predicate = PredicateBuilder.True<TSource>();

                _predicate = _predicate.And(expr);
            }

            return this;
        }

        /// <summary>
        /// "Or"-s the specified predicate expression to the existing query expression tree.
        /// </summary>
        /// <param name="expr">The expression to "Or".</param>
        /// <returns>The QueryBuilder so it can be extended using fluent syntax.</returns>
        public QueryBuilder<TSource, TDest> OrFilterBy(Expression<Func<TSource, bool>> expr)
        {
            if (expr != null)
            {
                if (_predicate == null)
                    _predicate = PredicateBuilder.False<TSource>();

                _predicate = _predicate.Or(expr);
            }

            return this;
        }

        /// <summary>
        /// Constructs the resulting query based on the query parameters specified in the QueryBuilder. 
        /// </summary>
        /// <param name="sourceQuery">
        /// The initial query object to construct the query from. 
        /// </param>
        /// <returns>A translated query object that is ready to be executed.</returns>
        public IQueryable<TDest> Build(IQueryable<TSource> sourceQuery)
        {
            // Apply Filters
            if (_predicate != null)
                sourceQuery = sourceQuery.Where(_predicate.Expand());

            // Add Sort Expressions 
            if (this.SortExpressions != null)
            {
                bool firstSort = true;
                foreach (ISorter<TSource> sortExpression in this.SortExpressions)
                {
                    sourceQuery = sortExpression.Sort(sourceQuery, firstSort);
                    if (firstSort)
                        firstSort = false;
                }

                // Apply paging if specified 
                // NOTE: Paging can only be performed on an ordered collection/query
                if (this.SortExpressions.Count() > 0)
                    sourceQuery = sourceQuery
                        .SelectPage(this.PageInfo);
                else if (this.PageInfo != null)
                    throw new InvalidOperationException(
                        "At least one sort expression must be specified in order to properly paginate query results.");
            }

            // TRANSLATE TO DESTINATION OBJECT TYPE
            // var destQuery = sourceQuery.Select(this.Selector);
            var destQuery = Selectinator(sourceQuery);

            return destQuery;
        }

        /// <summary>
        /// Handles the selecting of the columns we want from the database.
        /// </summary>
        /// <param name="sourceQuery">Query that is being built by the query builder.</param>
        /// <returns>IQueryable result.</returns>
        protected virtual IQueryable<TDest> Selectinator(IQueryable<TSource> sourceQuery)
        {
            var destQuery = sourceQuery.Select(this.Selector);
            return destQuery;
        }

        /// <summary>
        /// Executes the query on a collection of source objects and returns the resulting collection of destination objects.
        /// </summary>
        /// <param name="source">Collection of objects to execute the query on.</param>
        /// <returns></returns>
        public IEnumerable<TDest> Execute(IEnumerable<TSource> source)
        {
            return this.Build(source.AsQueryable()).AsEnumerable();
        }

        #endregion
    } // class QueryBuilder<TSource, TDest>
}