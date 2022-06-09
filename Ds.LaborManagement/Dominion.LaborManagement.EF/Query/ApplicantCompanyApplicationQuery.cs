using System.Collections.Generic;
using System.Linq;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantCompanyApplicationQuery : Query<ApplicantCompanyApplication, IApplicantCompanyApplicationQuery>, IApplicantCompanyApplicationQuery
    {
        #region Constructor

        public ApplicantCompanyApplicationQuery(IEnumerable<ApplicantCompanyApplication> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        #endregion

        IApplicantCompanyApplicationQuery IApplicantCompanyApplicationQuery.ByClientId(int clientId)
        {
            FilterBy(x => x.ClientId == clientId);
            return this;
        }

        IApplicantCompanyApplicationQuery IApplicantCompanyApplicationQuery.ByApplicationId(int applicationId)
        {
            FilterBy(x => x.CompanyApplicationId == applicationId);
            return this;
        }

        IApplicantCompanyApplicationQuery IApplicantCompanyApplicationQuery.ByIsCurrentEmpApp(bool isCurrentEmpApp)
        {
            FilterBy(x => x.IsCurrentEmpApp == isCurrentEmpApp);
            return this;
        }

        IApplicantCompanyApplicationQuery IApplicantCompanyApplicationQuery.ByIsActive(bool isActive)
        {
            FilterBy(x => x.IsEnabled == isActive);
            return this;
        }

        IApplicantCompanyApplicationQuery IApplicantCompanyApplicationQuery.OrderByCompanyApplicationId()
        {
            OrderBy(x => x.CompanyApplicationId);
            return this;
        }

        IApplicantCompanyApplicationQuery IApplicantCompanyApplicationQuery.OrderByDescription()
        {
            OrderBy(x => x.Description);
            return this;
        }
    }
}
