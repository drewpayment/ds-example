using System;
using Dominion.Core.Dto.Client;
using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;
using System.Collections.Generic;
using Dominion.Core.Dto.Misc;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Builds a query on <see cref="Client"/> data.
    /// </summary>
    public interface IClientQuery : IQuery<Client, IClientQuery>
    {
        /// <summary>
        /// Filter by client ID.
        /// </summary>
        /// <param name="clientId">ID of client to query.</param>
        /// <returns></returns>
        IClientQuery ByClientId(int clientId);

        IClientQuery ByClientIds(IList<int> clientIds);
        IClientQuery ByIsActive();

        /// <summary>
        /// returns client based on an array of numbers
        /// </summary>
        /// <param name="clientIds"></param>
        /// <returns></returns>
        IClientQuery ByClientIds(int[] clientIds);

        /// <summary>
        /// Filter by client code
        /// </summary>
        /// <param name="clientCode">client code to query</param>
        /// <returns></returns>
        IClientQuery ByClientCode(string clientCode);

        /// <summary>
        /// Filter by clients containing the specified feature.
        /// </summary>
        /// <param name="feature">Feature to filter by.</param>
        /// <returns></returns>
        IClientQuery ByAccountFeature(AccountFeatureEnum feature);
        /// <summary>
        /// Filter by clients by AcaEnabled for the specific year.
        /// </summary>
        /// <param name="year">Specified year</param>
        /// <returns></returns>
        IClientQuery ByAcaEnabledForYear(int year);
        /// <summary>
        /// Filters by clients who have one of the specified statuses.
        /// </summary>
        /// <param name="statues">List of statuses. Clients containing one of the specified status(es) will be returned.</param>
        /// <returns></returns>
        IClientQuery ByClientStatus(params ClientStatusType[] statues);

        /// <summary>
        /// Filters by clients who are currently <see cref="ClientStatusType.Active"/> or were terminated after
        /// the specfied date.
        /// </summary>
        /// <param name="termDate">Date the client must be terminated after.</param>
        /// <returns></returns>
        IClientQuery ByTerminatedAfter(DateTime termDate);

        /// <summary>
        /// Filters out clients that belong to the specified client relation. This is typically used to filter out
        /// test/demo clients belonging to the 'Demonstration Payroll' relation.
        /// </summary>
        /// <param name="clientRelationIdToExclude">ID of the client relation to exclude.</param>
        /// <returns></returns>
        IClientQuery ExcludeClientRelationMembers(int clientRelationIdToExclude);

        /// <summary>
        /// Filters by clients whose ACA data was approved for the specified ACA reporting year.
        /// </summary>
        /// <param name="year">ACA reporting year the clients must be approved for.</param>
        /// <returns></returns>
        IClientQuery ByAcaSystemApprovedForYear(int year);

        /// <summary>
        /// Filters by clients who have signed off on their 1094 data for the specified ACA reporting year.
        /// </summary>
        /// <param name="year">ACA reporting year the clients must have a signed 1094-C for.</param>
        /// <returns></returns>
        IClientQuery ByAca1094SignatureForYear(int year);

        /// <summary>
        /// Filters by clients whom the given user ID has access to.
        /// </summary>
        /// <param name="userId">User ID that has access to clients.</param>
        /// <returns></returns>
        IClientQuery ByUserId(int userId);
        /// <summary>
        /// Filters by clients who are not considered "terminated". Optionally includes clients that have "Terminated with Access" up until the term date.
        /// </summary>
        /// <param name="includeTerminatedWithAccess">Whether clients with <see cref="ClientStatusType.TerminatedWithAccess"/> status should be included up until their term date.</param>
        /// <returns></returns>
        IClientQuery ByNotTerminated(bool includeTerminatedWithAccess = true);

        /// <summary>
        /// Filters by clients who are considered "terminated".
        /// </summary>
        /// <param name="includeTerminatedWithAccess">Whether clients with <see cref="ClientStatusType.TerminatedWithAccess"/> should be included if their term date is after today.</param>
        /// <returns></returns>
        IClientQuery ByTerminated(bool includeTerminatedWithAccess = true);
        IClientQuery ClientsUnAssignedToAnyOrganization();
        IClientQuery OrderByName();
        IClientQuery ByAllowTurboTax(int allowTurboTax);
    }
}
