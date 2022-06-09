using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.Query
{
    public interface IGroupExpressions<TSource, TKey, TResult>
    {
        Expression<Func<TSource, TKey>> GroupKey { get; }
        Expression<Func<IGrouping<TKey, TSource>, TResult>> Select { get; }
    }
}
