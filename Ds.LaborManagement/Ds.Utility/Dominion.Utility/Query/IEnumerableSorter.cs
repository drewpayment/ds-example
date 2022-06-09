using System.Collections.Generic;

namespace Dominion.Utility.Query
{
    public interface IEnumerableSorter<TSource>
    {
        string Name { get; }
        SortDirection SortDirection { get; }
        IEnumerable<TSource> Sort(IEnumerable<TSource> query, bool firstSort);
    }
}