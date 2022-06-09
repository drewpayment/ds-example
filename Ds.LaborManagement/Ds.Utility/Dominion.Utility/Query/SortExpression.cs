using System;
using System.Linq;
using System.Linq.Expressions;

namespace Dominion.Utility.Query
{
    /// <summary>
    /// Sort directions to order a collection by
    /// </summary>
    public enum SortDirection
    {
        Ascending, 
        Descending
    }

    /// <summary>
    /// Interface which performs a sort action on the specified type when implemented.
    /// </summary>
    /// <typeparam name="TSource">The type of object to be sorted.</typeparam>
    public interface ISorter<TSource>
    {
        string Name { get; }
        SortDirection SortDirection { get; }
        IQueryable<TSource> Sort(IQueryable<TSource> query, bool firstSort);
    }

    /// <summary>
    /// Class used to store and execute an expression that can be used to order an IQueryable Collection.
    /// </summary>
    /// <typeparam name="TSource">The object to be sorted.</typeparam>
    /// <typeparam name="TKey">The resulting type the object will be sorted by.</typeparam>
    public class SortExpression<TSource, TKey> : ISorter<TSource>
    {
        #region PROPERTIES

        /// <summary>
        /// Expression used to extract the key type the source object will be sorted by.
        /// </summary>
        public Expression<Func<TSource, TKey>> Expression { get; private set; }

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
        public SortExpression(Expression<Func<TSource, TKey>> expression, SortDirection sortDirection, string name = null)
        {
            Expression = expression;
            SortDirection = sortDirection;
            Name = name;
        }

        #endregion

        #region METHODS

        public IQueryable<TSource> Sort(IQueryable<TSource> query, bool firstSort)
        {
            if (firstSort)
            {
                if (SortDirection == SortDirection.Ascending)
                    query = query.OrderBy(Expression);
                else
                    query = query.OrderByDescending(Expression);
            }
            else
            {
                if (SortDirection == SortDirection.Ascending)
                    query = (query as IOrderedQueryable<TSource>).ThenBy(Expression);
                else
                    query = (query as IOrderedQueryable<TSource>).ThenByDescending(Expression);
            }

            return query;
        }

        #endregion
    }
}