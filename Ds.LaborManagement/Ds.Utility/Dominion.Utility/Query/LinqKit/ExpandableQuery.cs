using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Dominion.Utility.Query.LinqKit
{
    /// <summary>
    /// An IQueryable wrapper that allows the query's expression tree to be "visited" just before a query provider (such as LINQ to Entities) evaluates it.
    /// </summary>
    /// <remarks>
    /// This is based on the excellent work of Tomas Petricek: http://tomasp.net/blog/linq-expand.aspx
    /// Original Source From LINQKit: http://www.albahari.com/nutshell/linqkit.aspx
    /// </remarks>
    public class ExpandableQuery<T> : IQueryable<T>, IOrderedQueryable<T>, IOrderedQueryable
    {
        private ExpandableQueryProvider<T> _provider;
        private IQueryable<T> _inner;

        internal IQueryable<T> InnerQuery
        {
            get { return _inner; }
        } // Original query, that we're wrapping

        internal ExpandableQuery(IQueryable<T> inner)
        {
            _inner = inner;
            _provider = new ExpandableQueryProvider<T>(this);
        }

        Expression IQueryable.Expression
        {
            get { return _inner.Expression; }
        }

        Type IQueryable.ElementType
        {
            get { return typeof (T); }
        }

        IQueryProvider IQueryable.Provider
        {
            get { return _provider; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        public override string ToString()
        {
            return _inner.ToString();
        }
    }

    /// <summary>
    /// IQueryProvider allowing a query's expression tree to be "visited" by an ExpressionExpander prior to performing the inner query.
    /// </summary>
    /// <typeparam name="T">Type the query is built on.</typeparam>
    /// <remarks>
    /// Original Source From LINQKit: http://www.albahari.com/nutshell/linqkit.aspx
    /// </remarks>
    internal class ExpandableQueryProvider<T> : IQueryProvider
    {
        private ExpandableQuery<T> _query;

        internal ExpandableQueryProvider(ExpandableQuery<T> query)
        {
            _query = query;
        }

        /*  The following four methods first call ExpressionExpander to visit the expression tree, then call
         *  upon the inner query to do the remaining work. 
         */

        /// <summary>
        /// Calls the ExpressionExpander to visit the query's expression tree and then calls the inner query to perform the remaining work.
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        IQueryable<TElement> IQueryProvider.CreateQuery<TElement>(Expression expression)
        {
            return new ExpandableQuery<TElement>(_query.InnerQuery.Provider.CreateQuery<TElement>(expression.Expand()));
        }

        /// <summary>
        /// Calls the ExpressionExpander to visit the query's expression tree and then calls the inner query to perform the remaining work.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        IQueryable IQueryProvider.CreateQuery(Expression expression)
        {
            return _query.InnerQuery.Provider.CreateQuery(expression.Expand());
        }

        /// <summary>
        /// Calls the ExpressionExpander to visit the query's expression tree and then calls the inner query to perform the remaining work.
        /// </summary>
        /// <typeparam name="TElement"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        TResult IQueryProvider.Execute<TResult>(Expression expression)
        {
            return _query.InnerQuery.Provider.Execute<TResult>(expression.Expand());
        }

        /// <summary>
        /// Calls the ExpressionExpander to visit the query's expression tree and then calls the inner query to perform the remaining work.
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        object IQueryProvider.Execute(Expression expression)
        {
            return _query.InnerQuery.Provider.Execute(expression.Expand());
        }
    }
}