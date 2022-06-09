using System;
using System.Linq.Expressions;

namespace Dominion.Utility.Query.LinqKit
{
    /// <summary>
    /// Provides a way to create anonymous methods in lambda expression or function form.
    /// </summary>
    /// <remarks>
    /// Another good idea by Tomas Petricek.
    /// See http://tomasp.net/blog/dynamic-linq-queries.aspx for information on how it's used.
    /// 
    /// Original Source From LINQKit: http://www.albahari.com/nutshell/linqkit.aspx
    /// </remarks>
    public static class Linq
    {
        /// <summary>
        /// Returns the given anonymous method as a lambda expression.
        /// </summary>
        /// <typeparam name="T">Type the expression will be evaluated against.</typeparam>
        /// <typeparam name="TResult">Type the expression will return when evaluated.</typeparam>
        /// <param name="expr">The delegate as a lambda expression.</param>
        /// <returns></returns>
        public static Expression<Func<T, TResult>> Expr<T, TResult>(Expression<Func<T, TResult>> expr)
        {
            return expr;
        }

        /// <summary>
        /// Returns the given anonymous function as a Func delegate.
        /// </summary>
        /// <typeparam name="T">Type the function will be performed on.</typeparam>
        /// <typeparam name="TResult">Type the function will return.</typeparam>
        /// <param name="expr">A delegate function.</param>
        /// <returns></returns>
        public static Func<T, TResult> Func<T, TResult>(Func<T, TResult> expr)
        {
            return expr;
        }
    }
}