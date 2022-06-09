using System.Collections.Generic;
using System.Linq;

using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantOnboardingTaskAttachmentQuery : Query<ApplicantOnBoardingTaskAttachment, IApplicantOnboardingTaskAttachmentQuery>, IApplicantOnboardingTaskAttachmentQuery
    {
        public ApplicantOnboardingTaskAttachmentQuery(IEnumerable<ApplicantOnBoardingTaskAttachment> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        IApplicantOnboardingTaskAttachmentQuery IApplicantOnboardingTaskAttachmentQuery.ByApplicantOnBoardingTaskId(int applicantOnBoardingTaskId)
        {
            FilterBy(x => x.ApplicantOnBoardingTaskId == applicantOnBoardingTaskId);
            return this;
        }

        IApplicantOnboardingTaskAttachmentQuery IApplicantOnboardingTaskAttachmentQuery.ByApplicantOnBoardingTaskAttachmentId(int applicantOnBoardingTaskAttachmentId)
        {
            FilterBy(x => x.ApplicantOnBoardingTaskAttachmentId == applicantOnBoardingTaskAttachmentId);
            return this;
        }

        IApplicantOnboardingTaskAttachmentQuery IApplicantOnboardingTaskAttachmentQuery.ByClientId(int clientId)
        {
            FilterBy(x => x.ClientId == clientId);
            return this;
        }
    }
}


