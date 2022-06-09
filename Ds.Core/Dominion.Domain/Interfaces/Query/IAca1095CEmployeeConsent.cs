using System.Collections.Generic;
using Dominion.Domain.Entities.Aca;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IAca1095CEmployeeConsentQuery : IQuery<Aca1095CEmployeeConsent, IAca1095CEmployeeConsentQuery>
    {
        /// <summary>
        /// Filters 1095C data by employee.
        /// </summary>
        /// <param name="employeeId">Employee to filter by.</param>
        /// <returns>Query to be further filtered.</returns>
        IAca1095CEmployeeConsentQuery ByEmployeeId(int employeeId);


        /// <summary>
        /// Get existing line items only matching the employee ids passed in.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IAca1095CEmployeeConsentQuery ByClientId(int clientId);

    }
}
