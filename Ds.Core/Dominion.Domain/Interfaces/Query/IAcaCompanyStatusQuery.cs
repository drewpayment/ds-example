using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Aca.Dto.Forms;
using Dominion.Domain.Entities.Aca;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IAcaCompanyStatusQuery : IQuery<AcaCompanyStatus, IAcaCompanyStatusQuery>
    {

        /// <summary>
        /// Filters configuration data by the specified client.
        /// </summary>
        /// <param name="clientId">ID of client to filter configuration data for.</param>
        /// <param name="statusType">ID of client to filter configuration data for.</param>
        /// <returns></returns>
        IAcaCompanyStatusQuery ByClientIdByStatusType(int clientId, AcaCompanyStatusTypes statusType);

        /// <summary>
        /// Filters configuration data by the specified client and status type.
        /// </summary>
        /// <param name="clientId">ID of client to filter configuration data for.</param>
        /// <param name="statusType">ID of client to filter configuration data for.</param>
        /// <returns></returns>
        IAcaCompanyStatusQuery ByClientIdByYearByStatusType(int clientId, int year, AcaCompanyStatusTypes statusType);
    }
}
