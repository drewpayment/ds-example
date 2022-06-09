using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominion.Utility.Query
{
    public class SortFunc<TSource, TKey> : IEnumerableSorter<TSource>
    {
        #region PROPERTIES

        /// <summary>
        /// Expression used to extract the key type the source object will be sorted by.
        /// </summary>
        public Func<TSource, TKey> Func { get; private set; }

        /// <summary>
        /// Property that can be used to give the specific sort expression a name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Sort direction to order the object by (ascending / descending) 
        /// </summary>
        public SortDirection SortDirection { get; private set; }

        #endregion

        #region CONSTRUCTORS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="sortDirection"></param>
        /// <param name="name"></param>
        public SortFunc(Func<TSource, TKey> func, SortDirection sortDirection, string name = null)
        {
            Func = func;
            SortDirection = sortDirection;
            Name = name;
        }

        #endregion

        #region METHODS

        public IEnumerable<TSource> Sort(IEnumerable<TSource> query, bool firstSort)
        {
            if (firstSort)
            {
                if (SortDirection == SortDirection.Ascending)
                    query = query.OrderBy(Func);
                else
                    query = query.OrderByDescending(Func);
            }
            else
            {
                if (SortDirection == SortDirection.Ascending)
                    query = (query as IOrderedEnumerable<TSource>)?.ThenBy(Func) ?? query.OrderBy(Func);
                else
                    query = (query as IOrderedEnumerable<TSource>)?.ThenByDescending(Func) ?? query.OrderByDescending(Func);
            }

            return query;
        }

        #endregion
    }
}