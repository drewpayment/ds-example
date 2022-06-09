using System;
using System.Linq.Expressions;
using Dominion.Utility.Pagination;

namespace Dominion.Utility.Query
{
    /// <summary>
    /// Provides static methods to create various forms of QueryBuilders.
    /// </summary>
    public static class QueryBuilderFactory
    {
        /// <summary>
        /// Create a new QueryBuilder object based on the given mapper.
        /// </summary>
        /// <param name="selector">The selection expression upon which to base the new QueryBuilder.</param>
        /// <returns>A new QueryBuilder object based on the given mapper.</returns>
        /// <remarks>
        /// This class is intended to allow a QueryBuilder to be created using anonymous types. 
        /// For Example:
        ///   var qb = QueryBuilderHelper.GetQueryBuilder( (Employee emp) => new { emp.AddressLine1, emp.AddressLine2} );
        /// </remarks>
        public static QueryBuilder<TSource, TDest> Create<TSource, TDest>(Expression<Func<TSource, TDest>> selector)
            where TSource : class
            where TDest : class
        {
            return new QueryBuilder<TSource, TDest>(selector);
        }

        // Create<TSource, TDest>()


        /// <summary>
        /// Crate a new QueryBuilder object based on the specified type.
        /// </summary>
        /// <typeparam name="TSource">Type the query will be build on and returned as.</typeparam>
        /// <returns>QueryBuilder to construct query criteria on.</returns>
        public static QueryBuilder<TSource> Create<TSource>()
            where TSource : class
        {
            return new QueryBuilder<TSource>();
        }

        // Create<TSource>()

        public static QueryBuilderAutoMap<TSource, TDest> CreateAutoMap<TSource, TDest>(PaginationInfo pageInfo = null)
            where TSource : class
            where TDest : class
        {
            return new QueryBuilderAutoMap<TSource, TDest>(pageInfo);
        }

        // CreateAutoMap<TSource,TDest>()
    } // class QueryBuilderFactory
}