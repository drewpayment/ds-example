using System.Collections.Generic;
using Dominion.Domain.Entities.Aca;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IAcaAleMemberQuery  : IQuery<AcaAleMember, IAcaAleMemberQuery>
    {
        /// <summary>
        /// Filters members by a Federal Employer Identification Number (FEIN).
        /// </summary>
        /// <param name="fein">Federal Employer Identification Number to query member info for.</param>
        /// <returns></returns>
        IAcaAleMemberQuery FilterByFein(string fein);


        IAcaAleMemberQuery FilterByFeins(IEnumerable<string> feins);


        /// <summary>
        /// Filters members by the specified ACA reporting/filing year.
        /// </summary>
        /// <param name="year">ACA reporting/filing year to filter by.</param>
        /// <returns></returns>
        IAcaAleMemberQuery FilterByYear(int year);

        /// <summary>
        /// Filters members associated with the specified client.
        /// </summary>
        /// <param name="clientId">Client to get member info for.</param>
        /// <returns></returns>
        IAcaAleMemberQuery FilterByClient(int clientId);

        IAcaAleMemberQuery FilterByClientIds(IEnumerable<int> clientIds);
    }
}
