using System.Collections.Generic;

namespace Dominion.Utility.OpResult
{
    public interface IGroupedOpResults<TVal>
    {
        IEnumerable<IOpResult<TVal>> ResultsWithoutErrors { get; set; }
        IEnumerable<IOpResult<TVal>> ResultsWithErrors { get; set; }
    }
}