using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dominion.Utility.Query.LinqKit
{
    /// <summary>
    /// Provides helper extension methods useful in querying.
    /// </summary>
    /// <remarks>
    /// Refer to http://www.albahari.com/nutshell/linqkit.html and
    /// http://tomasp.net/blog/linq-expand.aspx for more information.
    /// 
    /// Original Source From LINQKit: http://www.albahari.com/nutshell/linqkit.aspx
    /// </remarks>
    public static class QueryExtensions
    {
        /// <summary>
        /// Wraps the specified IQueryable in order allow each expression in the expression tree to be "visited" prior to evaluating to ensure 
        /// it is structured properly for the given data provider (such as LINQ to Entities).
        /// </summary>
        /// <typeparam name="T">Collection type the query will be executed against.</typeparam>
        /// <param name="query">Collection the query will be built and executed against.</param>
        /// <returns></returns>
        public static IQueryable<T> AsExpandable<T>(this IQueryable<T> query)
        {
            if (query is ExpandableQuery<T>)
                return (ExpandableQuery<T>)query;

            return new ExpandableQuery<T>(query);
        }

        /// <summary>
        /// "Visits" the given expression to ensure it is structured properly before being executed by a query provider.
        /// </summary>
        /// <typeparam name="TDelegate">Type the delegate is based on.</typeparam>
        /// <param name="expr">The delegate expression to be "visited".</param>
        /// <returns>A properly structured expression ready to be evaluated by a query provider.</returns>
        public static Expression<TDelegate> Expand<TDelegate>(this Expression<TDelegate> expr)
        {
            return (Expression<TDelegate>)new ExpressionExpander().Visit(expr);
        }

        /// <summary>
        /// "Visits" the given expression to ensure it is structured properly before being executed by a query provider.
        /// </summary>
        /// <param name="expr">The delegate expression to be "visited".</param>
        /// <returns>A properly structured expression ready to be evaluated by a query provider.</returns>
        public static Expression Expand(this Expression expr)
        {
            return new ExpressionExpander().Visit(expr);
        }

        /// <summary>
        /// Executes the given expression.
        /// </summary>
        /// <typeparam name="TResult">Type the expression will return when executed.</typeparam>
        /// <param name="expr">Expression to execute.</param>
        /// <returns>Return value of the expression.</returns>
        public static TResult Invoke<TResult>(this Expression<Func<TResult>> expr)
        {
            return expr.Compile().Invoke();
        }

        /// <summary>
        /// Executes the given expression.
        /// </summary>
        /// <typeparam name="TResult">Type the expression will return when executed.</typeparam>
        /// <param name="expr">Expression to execute.</param>
        /// <returns>Return value of the expression.</returns>
        public static TResult Invoke<T1, TResult>(this Expression<Func<T1, TResult>> expr, T1 arg1)
        {
            return expr.Compile().Invoke(arg1);
        }

        /// <summary>
        /// Executes the given expression.
        /// </summary>
        /// <typeparam name="TResult">Type the expression will return when executed.</typeparam>
        /// <param name="expr">Expression to execute.</param>
        /// <returns>Return value of the expression.</returns>
        public static TResult Invoke<T1, T2, TResult>(this Expression<Func<T1, T2, TResult>> expr, T1 arg1, T2 arg2)
        {
            return expr.Compile().Invoke(arg1, arg2);
        }

        /// <summary>
        /// Executes the given expression.
        /// </summary>
        /// <typeparam name="TResult">Type the expression will return when executed.</typeparam>
        /// <param name="expr">Expression to execute.</param>
        /// <returns>Return value of the expression.</returns>
        public static TResult Invoke<T1, T2, T3, TResult>(this Expression<Func<T1, T2, T3, TResult>> expr, T1 arg1,
            T2 arg2, T3 arg3)
        {
            return expr.Compile().Invoke(arg1, arg2, arg3);
        }

        /// <summary>
        /// Executes the given expression.
        /// </summary>
        /// <typeparam name="TResult">Type the expression will return when executed.</typeparam>
        /// <param name="expr">Expression to execute.</param>
        /// <returns>Return value of the expression.</returns>
        public static TResult Invoke<T1, T2, T3, T4, TResult>(this Expression<Func<T1, T2, T3, T4, TResult>> expr,
            T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return expr.Compile().Invoke(arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Performs the given action on each element in the specified collection.
        /// </summary>
        /// <typeparam name="T">Collection type.</typeparam>
        /// <param name="source">Collection to perform the action on.</param>
        /// <param name="action">The action to perform on each element of the collection.</param>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
                action(element);
        }

        /// <summary>
        /// Lazily performs the given action on each element in the source collection.
        /// </summary>
        /// <typeparam name="T">The type of elements in the collection.</typeparam>
        /// <param name="source">The collection that should be evaluated.</param>
        /// <param name="action">The action to perform on each element in the collection.</param>
        /// <returns>Returns the source collection.</returns>
        public static IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> action)
        {
            return source.Select(
                i =>
                    {
                        action(i);
                        return i;
                    });
        }
    } // class Extensions
}