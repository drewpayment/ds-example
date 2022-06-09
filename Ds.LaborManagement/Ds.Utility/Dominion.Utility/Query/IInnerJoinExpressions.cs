using System;
using System.Linq.Expressions;

namespace Dominion.Utility.Query
{
    public interface IInnerJoinExpressions<TOuter, TInner, TKey, TResult>
    {
        Expression<Func<TOuter, TKey>> OuterKey { get; }
        Expression<Func<TInner, TKey>> InnerKey { get; }
        Expression<Func<TOuter, TInner, TResult>> Select { get; }
    }
}