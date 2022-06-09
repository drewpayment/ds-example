using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantOnBoardingProcessQuery : Query<ApplicantOnBoardingProcess, IApplicantOnBoardingProcessQuery>, IApplicantOnBoardingProcessQuery
    {
        #region Constructor

        public ApplicantOnBoardingProcessQuery(IEnumerable<ApplicantOnBoardingProcess> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        #endregion

        IApplicantOnBoardingProcessQuery IApplicantOnBoardingProcessQuery.ByClientId(int clientId)
        {
            FilterBy(x => x.ClientId == clientId);
            return this;
        }

        IApplicantOnBoardingProcessQuery IApplicantOnBoardingProcessQuery.ByCustomToPostingId(int customToPostingId)
        {
            FilterBy(x => x.CustomToPostingId == customToPostingId || x.CustomToPostingId == null);
            return this;
        }
        IApplicantOnBoardingProcessQuery IApplicantOnBoardingProcessQuery.ByApplicantOnboardingProcessId(int applicantOnboardingProcessId)
        {
            FilterBy(x => x.ApplicantOnboardingProcessId == applicantOnboardingProcessId);
            return this;
        }

        IApplicantOnBoardingProcessQuery IApplicantOnBoardingProcessQuery.ByIsEnabled(bool isEnabled)
        {
            FilterBy(x => x.IsEnabled == isEnabled);
            return this;
        }

        IApplicantOnBoardingProcessQuery IApplicantOnBoardingProcessQuery.OrderByApplicantOnBoardingProcessId()
        {
            OrderBy(x => x.ApplicantOnboardingProcessId);
            return this;
        }

        IApplicantOnBoardingProcessQuery IApplicantOnBoardingProcessQuery.OrderByDescription()
        {
            OrderBy(x => x.Description);
            return this;
        }
    }
}