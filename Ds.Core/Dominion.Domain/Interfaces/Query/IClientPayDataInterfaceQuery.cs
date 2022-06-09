using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Provides a way to query ClientPayDataInterface configurations.
    /// </summary>
    public interface IClientPayDataInterfaceQuery : IQuery<ClientPayDataInterface, IClientPayDataInterfaceQuery>
    {
        /// <summary>
        /// Filters the client pay data interface configurations results by the unique inteface type.
        /// </summary>
        /// <param name="type">The interface (import) type to filter by.</param>
        /// <returns></returns>
        IClientPayDataInterfaceQuery ByPayrollPayDataInterfaceType(PayrollPayDataInterfaceType type);

        /// <summary>
        /// Filters the client pay data interface configurations results for a specific client.
        /// </summary>
        /// <param name="clientId">Unique ID of the client to filter by.</param>
        /// <returns></returns>
        IClientPayDataInterfaceQuery ByClientId(int clientId);
    }
}