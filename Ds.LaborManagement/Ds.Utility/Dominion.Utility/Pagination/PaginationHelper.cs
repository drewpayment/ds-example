using System.Linq;

namespace Dominion.Utility.Pagination
{
    public static class PaginationHelper
    {
        public static IQueryable<T> SelectPage<T>(this IQueryable<T> query, PaginationInfo pageInfo)
            where T : class
        {
            if (pageInfo == null)
                return query;

            pageInfo.TotalItemCount = query.Count();

            return query
                .Skip(pageInfo.SkipCount)
                .Take(pageInfo.ItemCountPerPage);
        }
    }
}