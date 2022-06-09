using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Query on <see cref="PayFrequency"/> data.
    /// </summary>
    public interface IPayFrequencyQuery : IQuery<PayFrequency, IPayFrequencyQuery>
    {
        /// <summary>
        /// Filters by the specified pay frequencies.
        /// </summary>
        /// <param name="types">Frequencies to filter by.</param>
        /// <returns></returns>
        IPayFrequencyQuery ByFrequencyTypes(params PayFrequencyType[] types);
    }
}
