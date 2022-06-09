using System.Linq;
using AutoMapper.QueryableExtensions;
using Dominion.Utility.Pagination;

namespace Dominion.Utility.Query
{
    /// <summary>
    /// QueryBuilder which uses AutoMapper to map the source object type to the destination type.
    /// </summary>
    /// <typeparam name="TSource">Source object type.</typeparam>
    /// <typeparam name="TDest">Destination object type.</typeparam>
    public class QueryBuilderAutoMap<TSource, TDest> : QueryBuilder<TSource, TDest>
        where TSource : class
        where TDest : class
    {
        /// <summary>
        /// Instantiates a new QueryBuilderAutoMap object.
        /// </summary>
        /// <param name="pageInfo">Pagination configuration for the resulting query.</param>
        public QueryBuilderAutoMap(PaginationInfo pageInfo = null) :
            base(null, pageInfo)
        {
        }

        /// <summary>
        /// Uses AutoMapper's Project() method to transform the source query to the destination query.
        /// </summary>
        /// <param name="sourceQuery">Query to project to the destination type.</param>
        /// <returns>Destination type query object.</returns>
        protected override IQueryable<TDest> Selectinator(IQueryable<TSource> sourceQuery)
        {
            IQueryable<TDest> destQuery = sourceQuery.Project<TSource>().To<TDest>();
            return destQuery;
        }
    } // class QueryBuilderAutoMap
}