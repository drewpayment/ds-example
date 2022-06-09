using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Aca;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IAcaAleMemberClientQuery  : IQuery<AcaAleMemberClient, IAcaAleMemberClientQuery>
    {
        IAcaAleMemberClientQuery FilterByYear(int year);

        IAcaAleMemberClientQuery FilterByFein(string fein);
        IAcaAleMemberClientQuery FilterByFeins(IEnumerable<string> feins);

        IAcaAleMemberClientQuery FilterByClientId(int id);
        IAcaAleMemberClientQuery FilterByClientIds(IEnumerable<int> ids);

        /// <summary>
        /// Only if the client has signed off on their aca 1094 data or whose 1094 been transmitted in the past.
        /// </summary>
        /// <returns></returns>
        IAcaAleMemberClientQuery IsAvailableToEFile();

        /// <summary>
        /// Filters by clients whose last transmission was the one specified.
        /// </summary>
        /// <param name="lastTransmissionId">Transmission the client should have been last transmitted in.</param>
        /// <returns></returns>
        IAcaAleMemberClientQuery ByLastTransmissionId(int lastTransmissionId);

        /// <summary>
        /// Filters by clients who were part of the specified transmission (regardless if it was their last transmission 
        /// or not).
        /// </summary>
        /// <param name="transmissionId">Transmission the specified client-1094 should be a part of.</param>
        /// <returns></returns>
        IAcaAleMemberClientQuery ByTransmissionId(int transmissionId);

        /// <summary>
        /// Apply client filter based on ACA enabled feature for the year
        /// </summary>
        /// <param name="year">Specific ACA year</param>
        /// <returns></returns>
        IAcaAleMemberClientQuery ByAcaEnabledForYear(int year);
    }
}
