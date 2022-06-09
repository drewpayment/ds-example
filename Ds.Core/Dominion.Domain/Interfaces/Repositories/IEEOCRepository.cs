using System.Collections.Generic;
using Dominion.Core.Dto.EEOC;
using Dominion.Core.Dto.EEOC.EEO1.Comp1;
using Dominion.Domain.Entities.EEOC;
using Dominion.Domain.Interfaces.Query;

namespace Dominion.Domain.Interfaces.Repositories
{
    public interface IEEOCRepository
    {
        IEEOCLocationQuery EeocLocationQuery();
        IEEOCPayBandQuery EeocPayBandQuery();
        IClientEEOCQuery ClientEeocQuery();
        IEEOCJobCategoriesQuery EEOCJobCategoriesQuery();
        IEnumerable<CompanyEEOCPaybandSubResult> GetSpReportCompanyEEOCPaybandSub_ByVars(int clientID, int locationID, int w2Year, int payrollID);
        IEeocRaceEthnicCategoriesQuery EeocRaceEthnicCategoriesQuery();
        IEEOCOrganizationQuery EeocOrganizationQuery();
        IEnumerable<Eeo1Comp1LocationJobCategoryDemographicsDto> GetEeocComp1Data(List<ClientPayroll> clientPayrolls);
    }
}
