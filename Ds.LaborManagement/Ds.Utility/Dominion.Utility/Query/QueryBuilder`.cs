using System;
using System.Linq.Expressions;
using Dominion.Utility.Pagination;

namespace Dominion.Utility.Query
{
    /// <summary>
    /// Class allowing query parameters such as Filters, Pagination and Sorting to be built
    /// and passed dynamically.
    /// </summary>
    /// <typeparam name="TSource">Object type the query will be executed on and returned as.</typeparam>
    public class QueryBuilder<TSource> : QueryBuilder<TSource, TSource>
        where TSource : class
    {
        #region VARIABLES AND PROPERTIES

        /// <summary>
        /// Override of the base QueryBuilder&lt;TSource, TDest&gt; which simply "selects" the TSource
        /// object type as itself.
        /// </summary>
        public override Expression<Func<TSource, TSource>> Selector
        {
            get { return x => x; }
        }

        #endregion // region VARIABLES AND PROPERTIES

        #region CONSTRUCTORS AND INTIALIZATION

        public QueryBuilder()
            : base()
        {
        }

        public QueryBuilder(PaginationInfo pageInfo)
            : base(null, pageInfo)
        {
        }

        #endregion // region CONSTRUCTORS AND INITIALIZATION
    } // class QueryBuilder<TSource>
}