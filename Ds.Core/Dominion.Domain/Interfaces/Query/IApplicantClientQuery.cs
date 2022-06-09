using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Builds a query on <see cref="Client"/> data.
    /// </summary>
    public interface IApplicantClientQuery : IQuery<ApplicantClient, IApplicantClientQuery>
    {
        /// <summary>
        /// Filter by client ID.
        /// </summary>
        /// <param name="clientId">ID of client to query.</param>
        /// <returns></returns>
        IApplicantClientQuery ByClientId(int clientId);
    }
}
