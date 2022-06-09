using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantOnBoardingProcessSetQuery : Query<ApplicantOnBoardingProcessSet, IApplicantOnBoardingProcessSetQuery>, IApplicantOnBoardingProcessSetQuery
    {
        public ApplicantOnBoardingProcessSetQuery(IEnumerable<ApplicantOnBoardingProcessSet> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        IApplicantOnBoardingProcessSetQuery IApplicantOnBoardingProcessSetQuery.ByApplicantOnboardingProcessId(int applicantOnboardingProcessId)
        {
            FilterBy(x => x.ApplicantOnboardingProcessId == applicantOnboardingProcessId);
            return this;
        }

        IApplicantOnBoardingProcessSetQuery IApplicantOnBoardingProcessSetQuery.ByApplicantOnboardingTaskId(int applicantOnboardingTaskId)
        {
            FilterBy(x => x.ApplicantOnboardingTaskId == applicantOnboardingTaskId);
            return this;
        }

        IApplicantOnBoardingProcessSetQuery IApplicantOnBoardingProcessSetQuery.OrderByOrderId()
        {
            OrderBy(x => x.OrderId);
            return this;
        }
    }
}
