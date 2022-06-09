using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantOnboardingProcessTypeQuery : Query<ApplicantOnBoardingProcessType, IApplicantOnboardingProcessTypeQuery>, IApplicantOnboardingProcessTypeQuery
    {
        public ApplicantOnboardingProcessTypeQuery(IEnumerable<ApplicantOnBoardingProcessType> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }
        IApplicantOnboardingProcessTypeQuery IApplicantOnboardingProcessTypeQuery.OrderByDescription()
        {
            OrderBy(x => x.Description);
            return this;
        }
    }
}
