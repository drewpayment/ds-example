using Dominion.Domain.Entities.TimeClock;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientMachineQuery : IQuery<ClientMachine, IClientMachineQuery>
    {
        /// <summary>
        /// Filters entities by ClientMachineId.
        /// </summary>
        /// <param name="clientMachineId"></param>
        /// <returns></returns>
        IClientMachineQuery ByClientMachineId(int clientMachineId);

        /// <summary>
        /// Filters entities by ClientId.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IClientMachineQuery ByClientId(int clientId);

        /// <summary>
        /// Filters entities by PushMachineId.
        /// </summary>
        /// <param name="pushMachineId"></param>
        /// <returns></returns>
        IClientMachineQuery ByPushMachineId(int pushMachineId);

    }
}
