using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Client;
using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Clients
{
    public interface IClientTurboTaxTrackingQuery : IQuery<ClientTurboTaxTracking, IClientTurboTaxTrackingQuery>
    {
        /// <summary>
        /// Filter by ClientTurboTaxTrackingID
        /// </summary>
        /// <param name="clientTurboTaxTrackingId">ID of ClientTurboTaxTracking to query.</param>
        /// <returns></returns>
        IClientTurboTaxTrackingQuery ByClientTurboTaxTrackingId(int clientTurboTaxTrackingId);

        /// <summary>
        /// Filter by CllientTurboTaxID
        /// </summary>
        /// <param name="clientTurboTaxId">ID of ClientTurboTax to query.</param>
        /// <returns></returns>
        IClientTurboTaxTrackingQuery ByClientTurboTaxId(int clientTurboTaxId);

        /// <summary>
        /// Filter by list of ClientTurboTaxIds
        /// </summary>
        /// <param name="clientTurboTaxIds">List of IDs to query.</param>
        /// <returns></returns>
        IClientTurboTaxTrackingQuery ByClientTurboTaxIds(List<int> clientTurboTaxIds);

        /// <summary>
        /// Filter By TrackingID
        /// </summary>
        /// <param name="trackingId">Tracking ID to query.</param>
        /// <returns></returns>
        IClientTurboTaxTrackingQuery ByTrackingId(string trackingId);

        /// <summary>
        /// Filter by tracking status.
        /// </summary>
        /// <param name="status">Status to query for.</param>
        /// <returns></returns>
        IClientTurboTaxTrackingQuery ByTrackingStatus(ClientTurboTaxTrackingStatus status);

        /// <summary>
        /// Filter by client and tax year.
        /// </summary>
        /// <param name="clientId">ClientTurboTax client ID to query for.</param>
        /// /// <param name="taxYear">ClientTurboTax tax year to query for.</param>
        /// <returns></returns>
        IClientTurboTaxTrackingQuery ByClientTaxYearPending(int clientId, int taxYear);
    }
}
