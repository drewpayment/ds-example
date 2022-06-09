using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Provides ability to query a <see cref="ClientEarningWithholdingOverride"/> dataset. 
    /// </summary>
    public interface IClientEarningWithholdingOverrideQuery : IQuery<ClientEarningWithholdingOverride, IClientEarningWithholdingOverrideQuery>
    {
        /// <summary>
        /// Filters by overrides by those belonging to the specified client earnings.
        /// </summary>
        /// <param name="clientEarningIds">ID(s) of the client earning(s) to filter overrides by.</param>
        /// <returns>Current query to further manipulate fluently.</returns>
        IClientEarningWithholdingOverrideQuery ByClientEarningIds(params int[] clientEarningIds);
    }
}
