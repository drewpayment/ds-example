using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Queries <see cref="CheckStubConfiguration"/> data.
    /// </summary>
    public interface ICheckStubConfigurationQuery : IQuery<CheckStubConfiguration, ICheckStubConfigurationQuery>
    {
        /// <summary>
        /// Filters check stub configurations by client.
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        ICheckStubConfigurationQuery ByClient(int clientId);
    }
}
