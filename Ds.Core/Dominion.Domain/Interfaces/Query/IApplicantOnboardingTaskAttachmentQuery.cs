using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IApplicantOnboardingTaskAttachmentQuery : IQuery<ApplicantOnBoardingTaskAttachment, IApplicantOnboardingTaskAttachmentQuery>
    {
        IApplicantOnboardingTaskAttachmentQuery ByApplicantOnBoardingTaskId(int applicantOnBoardingTaskId);
        IApplicantOnboardingTaskAttachmentQuery ByApplicantOnBoardingTaskAttachmentId(int applicantOnBoardingTaskAttachmentId);
        IApplicantOnboardingTaskAttachmentQuery ByClientId(int clientId);
    }
}


