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
    public interface IClientTurboTaxQuery : IQuery<ClientTurboTax, IClientTurboTaxQuery>
    {
        /// <summary>
        /// Filter by ClientTurboTaxID.
        /// </summary>
        /// <param name="clientTurboTaxId">ID of ClientTurboTax to query.</param>
        /// <returns></returns>
        IClientTurboTaxQuery ByClientTurboTaxId(int clientTurboTaxId);

        /// <summary>
        /// Filter by list of ClientTurboTaxIds
        /// </summary>
        /// <param name="clientTurboTaxIds">List of IDs to query.</param>
        /// <returns></returns>
        IClientTurboTaxQuery ByClientTurboTaxIds(List<int> clientTurboTaxIds);

        /// <summary>
        /// Filter by client ID.
        /// </summary>
        /// <param name="clientTurboTaxId">ID of client to query.</param>
        /// <returns></returns>
        IClientTurboTaxQuery ByClientId(int clientId);

        /// <summary>
        /// Filter by tax year.
        /// </summary>
        /// <param name="taxYear">Tax year to query.</param>
        /// <returns></returns>
        IClientTurboTaxQuery ByTaxYear(int taxYear);

        /// <summary>
        /// Filter by ClientTurboTax file status
        /// </summary>
        /// <param name="fileStatus">File status to query by.</param>
        /// <returns></returns>
        IClientTurboTaxQuery ByFileStatus(ClientTurboTaxFileStatus fileStatus);

        /// <summary>
        /// Filter by client and tax year.
        /// </summary>
        /// <param name="clientId">ID of client to query.</param>
        /// <param name="taxYear">Tax year to query.</param>
        /// <returns></returns>
        IClientTurboTaxQuery ByClientAndTaxYear(int clientId, int taxYear);
    }
}
