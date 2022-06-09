using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    /// <summary>
    /// Queries salary method information.
    /// </summary>
    public interface ISalaryMethodInfoQuery : IQuery<SalaryMethodTypeInfo, ISalaryMethodInfoQuery>
    {
    }
}
