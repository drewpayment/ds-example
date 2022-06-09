using System;
using System.Linq;
using System.Linq.Expressions;

namespace Dominion.Utility.Query.LinqKit
{
    /// <summary>
    /// Provides a way to dynamically build predicate expression trees ("where"-clause(s)) that can be executed on a collection of objects.
    /// </summary>
    /// <remarks>
    /// See http://www.albahari.com/expressions for information and examples.
    /// 
    /// Original Source From LINQKit: http://www.albahari.com/nutshell/linqkit.aspx
    /// </remarks>
    public static class PredicateBuilder
    {
        /// <summary>
        /// Returns a base predicate evaluating to true. Use when 'And'-ing predicates.
        /// </summary>
        /// <typeparam name="T">Type of object the expression tree will be built on.</typeparam>
        /// <returns>Base predicate evaluating to true.</returns>
        public static Expression<Func<T, bool>> True<T>()
        {
            return f => true;
        }

        /// <summary>
        /// Returns a base predicate evaluating to false. Use when 'Or'-ing predicates.
        /// </summary>
        /// <typeparam name="T">Type of object the expression tree will be built on.</typeparam>
        /// <returns>Base predicate evaluating to false.</returns>
        public static Expression<Func<T, bool>> False<T>()
        {
            return f => false;
        }

        /// <summary>
        /// Combines two predicate expressions by "Or"-ing them into a single predicate expression.
        /// An "OrElse" is used so the second expression will only be evaluated if the first evaluates to false.
        /// </summary>
        /// <typeparam name="T">Type of object the predicates are based on.</typeparam>
        /// <param name="expr1">First predicate expression to be evaluated.</param>
        /// <param name="expr2">Second predicate expression to be evaluated.</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, 
            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());

            return Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        /// <summary>
        /// Combines two predicate expressions by "And"-ing them into a single predicate expression.
        /// An "AndAlso" is used so the second expression will only be evaluated if the first evaluates to true.
        /// </summary>
        /// <typeparam name="T">Type of object the predicates are based on.</typeparam>
        /// <param name="expr1">First predicate expression to be evaluated.</param>
        /// <param name="expr2">Second predicate expression to be evaluated.</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, 
            Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters.Cast<Expression>());

            return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}