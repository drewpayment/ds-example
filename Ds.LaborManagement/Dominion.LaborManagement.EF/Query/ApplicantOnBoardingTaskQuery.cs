using System.Collections.Generic;
using System.Linq;

using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantOnBoardingTaskQuery : Query<ApplicantOnBoardingTask, IApplicantOnBoardingTaskQuery>, IApplicantOnBoardingTaskQuery
    {
        public ApplicantOnBoardingTaskQuery(IEnumerable<ApplicantOnBoardingTask> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        IApplicantOnBoardingTaskQuery IApplicantOnBoardingTaskQuery.ByApplicantOnBoardingTaskId(int applicantOnBoardingTaskId)
        {
            FilterBy(x => x.ApplicantOnboardingTaskId == applicantOnBoardingTaskId);
            return this;
        }

        IApplicantOnBoardingTaskQuery IApplicantOnBoardingTaskQuery.ByClientId(int clientId)
        {
            FilterBy(x => x.ClientId == clientId);
            return this;
        }

        IApplicantOnBoardingTaskQuery IApplicantOnBoardingTaskQuery.IsActive(bool isActive)
        {
            FilterBy(x => x.IsEnabled == isActive);
            return this;
        }

        IApplicantOnBoardingTaskQuery IApplicantOnBoardingTaskQuery.ExcludeTasksByProcessId(int processId)
        {
            FilterBy(x => !x.ApplicantOnBoardingProcessSet.Any(y => y.ApplicantOnboardingProcessId == processId));
            return this;
        }
    }
}