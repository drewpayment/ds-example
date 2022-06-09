using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Clients
{
    public interface IClientUnemploymentSetupQuery : IQuery<ClientUnemploymentSetup, IClientUnemploymentSetupQuery>
    {
        /// <summary>
        /// Filter by ByClientID
        /// </summary>
        /// <param name="ClientID">ID of Client to query.</param>
        /// <returns></returns>
        IClientUnemploymentSetupQuery ByClientID(int ClientID);

        /// <summary>
        /// Filter by ByClientUnemploymentProviderID
        /// </summary>
        /// <param name="UnemploymentProviderID">ID of unemployment provider to query.</param>
        /// <returns></returns>
        IClientUnemploymentSetupQuery ByClientUnemploymentProviderID(int UnemploymentProviderID);

        /// <summary>
        /// Filter by ByIsActive
        /// </summary>
        /// <param name="IsActive">List of IsActive clients to query.</param>
        /// <returns></returns>
        IClientUnemploymentSetupQuery ByIsActive(bool IsActive);

        /// <summary>
        /// Filter by ByIsActive
        /// </summary>
        /// <param name="IsActive">List of IsActive clients to query.</param>
        /// <returns></returns>
        IClientUnemploymentSetupQuery ByIsWotc(bool IsWOTC);
    }
}
