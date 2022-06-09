using System;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Interfaces.Query;
using System.Collections.Generic;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.Core.Dto.Sprocs;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repo methods for getting data for Applicant Tracking
    /// </summary>
    public interface IApplicantTrackingRepository : IRepository, IDisposable
    {
        IApplicantsQuery ApplicantsQuery();
        IApplicantPostingsQuery ApplicantPostingsQuery();
        IApplicantPostingOwnerQuery ApplicantPostingOwnerQuery();
        IApplicantPostingCategoriesQuery ApplicantPostingCategoriesQuery();
        IApplicantCompanyApplicationQuery ApplicantCompanyApplicationQuery();
        IApplicantJobTypeQuery ApplicantJobTypeQuery();
        IApplicantJobSiteQuery ApplicantJobSiteQuery();
        IClientJobSiteQuery ClientJobSiteQuery();
        IApplicantResumeRequiredQuery ApplicantResumeRequiredQuery();
        IApplicantSchoolTypeQuery ApplicantSchoolTypeQuery();
        IApplicantCorrespondenceTypeQuery ApplicantCorrespondenceTypeQuery();
        IApplicantCompanyCorrespondenceQuery ApplicantCompanyCorrespondenceQuery();
        IApplicantOnBoardingProcessQuery ApplicantOnBoardingProcessQuery();
        IApplicantOnBoardingProcessSetQuery ApplicantOnBoardingProcessSetQuery();
        IApplicantOnboardingProcessTypeQuery ApplicantOnboardingProcessTypeQuery();
        IApplicantApplicationHeaderQuery ApplicantApplicationHeaderQuery();
        IApplicantEmploymentHistoryQuery ApplicantEmploymentHistoryQuery();
        IApplicantStatusTypeQuery ApplicantStatusTypeQuery();
        IApplicantRejectionReasonQuery ApplicantRejectionReasonQuery();
        IApplicantReferenceQuery ApplicantReferenceQuery();
        IApplicantOnBoardingTaskQuery ApplicantOnBoardingTaskQuery();
        IApplicantOnboardingTaskAttachmentQuery ApplicantOnboardingTaskAttachmentQuery();
        IApplicantOnBoardingDetailQuery ApplicantOnBoardingDetailQuery();
        IApplicantOnBoardingDefaultDetailQuery ApplicantOnBoardingDefaultDetailQuery();
        IApplicantOnBoardingHeaderQuery ApplicantOnBoardingHeaderQuery();
        IApplicantApplicationDetailQuery ApplicantApplicationDetailQuery();
        IApplicantApplicationEmailHistoryQuery ApplicantApplicationEmailHistoryQuery();
        IApplicantEducationHistoryQuery ApplicantEducationHistoryQuery();
        IApplicantNoteQuery ApplicantNoteQuery();
        IApplicantQuestionDdlItemQuery ApplicantQuestionDdlItemQuery();
        IApplicantResumeQuery ApplicantResumeQuery();
        IApplicantDocumentQuery ApplicantDocumentQuery();
        IApplicationQuestionSectionQuery ApplicationQuestionSectionQuery();
        IApplicationSectionInstructionQuery ApplicationSectionInstructionQuery();
        IApplicantQuestionControlQuery ApplicantQuestionControlQuery();
        IApplicationViewedQuery ApplicationViewedQuery();
        IApplicantDegreeListQuery ApplicantDegreeListQuery();
        IApplicantClientQuery QueryApplicantClient();
		IApplicantQuestionSetQuery ApplicantQuestionSetQuery();

        IApplicantLicenseQuery ApplicantLicenseQuery();
        IApplicantSkillQuery ApplicantSkillQuery();

        IExternalApplicationIdentityQuery ExternalApplicationIdentityQuery();
        // int? GetNextPostingNumber(int clintId);
        //List<ApplicantPostingCategoryDto> GetApplicantPostingCategories(int clientId, int applicantId, bool isAdmin, int userId, bool includePastPostings, int sortBy, bool sortByAscendingOrder);
    }
}