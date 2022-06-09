using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientSMTPSettingQuery : IQuery<ClientSMTPSetting, IClientSMTPSettingQuery>
    {
        /// <summary>
        /// Filters by a given client
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        IClientSMTPSettingQuery ByClientId(int clientId);


        /// <summary>
        /// Filters by setting id for a given client
        /// </summary>
        /// <param name="clientSMTPSettingId">ID of client to filter by.</param>
        /// <returns></returns>
        IClientSMTPSettingQuery ByClientSMTPSettingId(int clientSMTPSettingId);
    }
}
