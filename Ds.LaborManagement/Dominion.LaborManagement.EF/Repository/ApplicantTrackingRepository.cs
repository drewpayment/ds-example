using System;
using System.Linq;
using Dominion.Core.EF.Abstract;
using Dominion.Core.EF.Interfaces;
using Dominion.Core.EF.Query;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Interfaces.Query;
using Dominion.Domain.Interfaces.Repositories;
using Dominion.LaborManagement.EF.Query;
using Dominion.Utility.Query;
using System.Collections.Generic;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.LaborManagement.EF.Query.Sproc;
using Dominion.Core.Dto.Sprocs;
using Dominion.LaborManagement.Service.Mapping;

namespace Dominion.LaborManagement.EF.Repository
{
    public class ApplicantTrackingRepository : RepositoryBase, IApplicantTrackingRepository
    {
        #region Constructors

        /// <summary>
        /// Instantiates a new Applicant Tracking Repository instance.
        /// </summary>
        /// <param name="context">Context the repository will perform data queries on.</param>
        /// <param name="resultFactory">Result factory used to create & execute query results. If null, a 
        /// <see cref="BasicQueryResultFactory"/> will be used.
        /// </param>
        public ApplicantTrackingRepository(IDominionContext context, IQueryResultFactory resultFactory = null)
            : base(context, resultFactory)
        {
        }

        #endregion

        #region IApplicantTrackingRepository

        public IApplicantsQuery ApplicantsQuery()
        {
            return new ApplicantsQuery(_context.Applicants, this.QueryResultFactory);
        }

        public IApplicantPostingsQuery ApplicantPostingsQuery()
        {
            return new ApplicantPostingsQuery(_context.ApplicantPostings, this.QueryResultFactory);
        }

        public IApplicantPostingOwnerQuery ApplicantPostingOwnerQuery()
        {
            return new ApplicantPostingOwnerQuery(_context.ApplicantPostingOwners, this.QueryResultFactory);
        }

        public IApplicantPostingCategoriesQuery ApplicantPostingCategoriesQuery()
        {
            return new ApplicantPostingCategoriesQuery(_context.ApplicantPostingCategories, this.QueryResultFactory);
        }

        //public List<ApplicantPostingCategoryDto> GetApplicantPostingCategories(int clientId, int applicantId, bool isAdmin, int userId, bool includePastPostings, int sortBy, bool sortByAscendingOrder)
        //{
        //    var now = DateTime.Now.Date;

        //    var postingCategories = new List<ApplicantPostingCategoryDto>();
        //    if (!includePastPostings)
        //    {
        //        postingCategories = (from category in _context.ApplicantPostingCategories
        //                             from aplicant in _context.Applicants.Where(x => x.ApplicantId == applicantId).DefaultIfEmpty()
        //                             from user in _context.Users.Where(x => x.UserId == userId).DefaultIfEmpty()
        //                             where
        //                             category.ClientId == clientId && category.IsEnabled == true
        //                             select new ApplicantPostingCategoryDto
        //                             {
        //                                 Name = category.Name,
        //                                 PostingCategoryId = category.PostingCategoryId,
        //                                 PostingCount = category.ApplicantPostings
        //                                                        .Count(x => (isAdmin || ((x.PostingTypeId != 2 && aplicant != null && !aplicant.EmployeeId.HasValue) || (x.PostingTypeId != 1 && ((aplicant != null && aplicant.EmployeeId.HasValue) || (user != null && user.EmployeeId.HasValue))))) &&
        //                                                                     !x.FilledDate.HasValue &&
        //                                                                     (x.IsRemoveAfterPostingEnd || (x.PostingEnd >= now && x.PostingStart <= now)))
        //                             }).ToList();
        //    }
        //    else
        //    {
        //        postingCategories = (from category in _context.ApplicantPostingCategories
        //                             from aplicant in _context.Applicants.Where(x => x.ApplicantId == applicantId).DefaultIfEmpty()
        //                             from user in _context.Users.Where(x => x.UserId == userId).DefaultIfEmpty()
        //                             where
        //                             category.ClientId == clientId && category.IsEnabled == true
        //                             select new ApplicantPostingCategoryDto
        //                             {
        //                                 Name = category.Name,
        //                                 PostingCategoryId = category.PostingCategoryId,
        //                                 PostingCount = category.ApplicantPostings
        //                                                        .Count(x => (isAdmin || ((x.PostingTypeId != (int)PostingType.Internal && aplicant != null && !aplicant.EmployeeId.HasValue) || (x.PostingTypeId != 1 && ((aplicant != null && aplicant.EmployeeId.HasValue) || (user != null && user.EmployeeId.HasValue))))) &&
        //                                                                     (x.FilledDate.HasValue || (x.IsRemoveAfterPostingEnd && x.PostedDate <= now)))
        //                             }).ToList();
        //    }

        //    if (sortBy == 1)
        //    {
        //        return sortByAscendingOrder ? postingCategories.OrderBy(x => x.Name).ToList() : postingCategories.OrderByDescending(x => x.Name).ToList();
        //    }

        //    return sortByAscendingOrder ? postingCategories.OrderBy(x => x.PostingCount).ToList() : postingCategories.OrderByDescending(x => x.PostingCount).ToList();
        //}

        public IApplicantCompanyApplicationQuery ApplicantCompanyApplicationQuery()
        {
            return new ApplicantCompanyApplicationQuery(_context.ApplicantCompanyApplications, this.QueryResultFactory);
        }

        public IApplicantJobTypeQuery ApplicantJobTypeQuery()
        {
            return new ApplicantJobTypeQuery(_context.ApplicantJobTypes, this.QueryResultFactory);
        }

        public IApplicantJobSiteQuery ApplicantJobSiteQuery()
        {
            return new ApplicantJobSiteQuery(_context.ApplicantJobSites, this.QueryResultFactory);
        }

        public IClientJobSiteQuery ClientJobSiteQuery()
        {
            return new ClientJobSiteQuery(_context.ClientJobSites, this.QueryResultFactory);
        }

        public IApplicantResumeRequiredQuery ApplicantResumeRequiredQuery()
        {
            return new ApplicantResumeRequiredQuery(_context.ApplicantResumeRequired, this.QueryResultFactory);
        }

        public IApplicantSchoolTypeQuery ApplicantSchoolTypeQuery()
        {
            return new ApplicantSchoolTypeQuery(_context.ApplicantSchoolTypes, this.QueryResultFactory);
        }

        public IApplicantCorrespondenceTypeQuery ApplicantCorrespondenceTypeQuery()
        {
            return new ApplicantCorrespondenceTypeQuery(_context.ApplicantCorrespondenceTypeInformation, this.QueryResultFactory);
        }

        public IApplicantCompanyCorrespondenceQuery ApplicantCompanyCorrespondenceQuery()
        {
            return new ApplicantCompanyCorrespondenceQuery(_context.ApplicantCompanyCorrespondences, this.QueryResultFactory);
        }

        public IApplicantOnBoardingProcessQuery ApplicantOnBoardingProcessQuery()
        {
            return new ApplicantOnBoardingProcessQuery(_context.ApplicantOnBoardingProcesses, this.QueryResultFactory);
        }

        public IApplicantOnBoardingProcessSetQuery ApplicantOnBoardingProcessSetQuery()
        {
            return new ApplicantOnBoardingProcessSetQuery(_context.ApplicantOnBoardingProcessSet, this.QueryResultFactory);
        }

        public IApplicantOnboardingProcessTypeQuery ApplicantOnboardingProcessTypeQuery()
        {
            return new ApplicantOnboardingProcessTypeQuery(_context.ApplicantOnBoardingProcessTypes, this.QueryResultFactory);
        }

        public IApplicantApplicationHeaderQuery ApplicantApplicationHeaderQuery()
        {
            return new ApplicantApplicationHeaderQuery(_context.ApplicantApplicationHeaders, this.QueryResultFactory);
        }

        public IApplicantEmploymentHistoryQuery ApplicantEmploymentHistoryQuery()
        {
            return new ApplicantEmploymentHistoryQuery(_context.ApplicantEmploymentHistories, this.QueryResultFactory);
        }

        public IApplicantStatusTypeQuery ApplicantStatusTypeQuery()
        {
            return new ApplicantStatusTypeQuery(_context.ApplicantStatusTypes, this.QueryResultFactory);
        }

        public IApplicantRejectionReasonQuery ApplicantRejectionReasonQuery()
        {
            return new ApplicantRejectionReasonQuery(_context.ApplicantRejectionReasons, this.QueryResultFactory);
        }

        public IApplicantReferenceQuery ApplicantReferenceQuery()
        {
            return new ApplicantReferenceQuery(_context.ApplicantReference, this.QueryResultFactory);
        }

        public IApplicantOnBoardingTaskQuery ApplicantOnBoardingTaskQuery()
        {
            return new ApplicantOnBoardingTaskQuery(_context.ApplicantOnBoardingTasks, this.QueryResultFactory);
        }

        public IApplicantOnBoardingDetailQuery ApplicantOnBoardingDetailQuery()
        {
            return new ApplicantOnBoardingDetailQuery(_context.ApplicantOnBoardingDetails, this.QueryResultFactory);
        }

        public IApplicantOnBoardingDefaultDetailQuery ApplicantOnBoardingDefaultDetailQuery()
        {
            return new ApplicantOnBoardingDefaultDetailQuery(_context.ApplicantOnBoardingDefaultDetails, this.QueryResultFactory);
        }

        public IApplicantOnBoardingHeaderQuery ApplicantOnBoardingHeaderQuery()
        {
            return new ApplicantOnBoardingHeaderQuery(_context.ApplicantOnBoardingHeaders, this.QueryResultFactory);
        }

        public IApplicantApplicationDetailQuery ApplicantApplicationDetailQuery()
        {
            return new ApplicantApplicationDetailQuery(_context.ApplicantApplicationDetails, this.QueryResultFactory);
        }

        public IApplicantApplicationEmailHistoryQuery ApplicantApplicationEmailHistoryQuery()
        {
            return new ApplicantApplicationEmailHistoryQuery(_context.ApplicantApplicationEmailHistories, this.QueryResultFactory);
        }

        public IApplicantEducationHistoryQuery ApplicantEducationHistoryQuery()
        {
            return new ApplicantEducationHistoryQuery(_context.ApplicantEducationHistories, this.QueryResultFactory);
        }

        public IApplicantNoteQuery ApplicantNoteQuery()
        {
            return new ApplicantNoteQuery(_context.ApplicantNotes, this.QueryResultFactory);
        }

        public IApplicantQuestionDdlItemQuery ApplicantQuestionDdlItemQuery()
        {
            return new ApplicantQuestionDdlItemQuery(_context.ApplicantQuestionDdlItems, this.QueryResultFactory);
        }

        public IApplicantResumeQuery ApplicantResumeQuery()
        {
            return new ApplicantResumeQuery(_context.ApplicantResumes, this.QueryResultFactory);
        }

        public IApplicantDocumentQuery ApplicantDocumentQuery()
        {
            return new ApplicantDocumentQuery(_context.ApplicantDocuments, this.QueryResultFactory);
        }

        public IApplicationQuestionSectionQuery ApplicationQuestionSectionQuery()
        {
            return new ApplicationQuestionSectionQuery(_context.ApplicationQuestionSections, this.QueryResultFactory);
        }

        public IApplicationSectionInstructionQuery ApplicationSectionInstructionQuery()
        {
            return new ApplicationSectionInstructionQuery(_context.ApplicationSectionInstructions, this.QueryResultFactory);
        }

        public IApplicantQuestionControlQuery ApplicantQuestionControlQuery()
        {
            return new ApplicantQuestionControlQuery(_context.ApplicantQuestionControls, this.QueryResultFactory);
        }

        public IApplicationViewedQuery ApplicationViewedQuery()
        {
            return new ApplicationViewedQuery(_context.ApplicationViewed, this.QueryResultFactory);
        }

        public IApplicantDegreeListQuery ApplicantDegreeListQuery()
        {
            return new ApplicantDegreeListQuery(_context.ApplicantDegreeList, this.QueryResultFactory);
        }

        public IApplicantOnboardingTaskAttachmentQuery ApplicantOnboardingTaskAttachmentQuery()
        {
            return new ApplicantOnboardingTaskAttachmentQuery(_context.ApplicantOnBoardingTaskAttachments, this.QueryResultFactory);
        }

        public IApplicantClientQuery QueryApplicantClient()
        {
            return new ApplicantClientQuery(_context.ApplicantClients, QueryResultFactory);
        }

		public IApplicantQuestionSetQuery ApplicantQuestionSetQuery()
        {
            return new ApplicantQuestionSetQuery(_context.ApplicantQuestionSets, this.QueryResultFactory);
        }

        public IApplicantLicenseQuery ApplicantLicenseQuery()
        {
            return new ApplicantLicenseQuery(_context.ApplicantLicenses, QueryResultFactory);
        }

        public IApplicantSkillQuery ApplicantSkillQuery()
        {
            return new ApplicantSkillQuery(_context.ApplicantSkills, QueryResultFactory);
        }

        public IExternalApplicationIdentityQuery ExternalApplicationIdentityQuery()
        {
            return new ExternalApplicationIdentityQuery(_context.ExternalApplicationIdentities, QueryResultFactory);
        }

        //public int? GetNextPostingNumber(int clientId)
        //{
        //    return _context.ApplicantPostings.Where(x => x.ClientId == clientId).DefaultIfEmpty().Max(x => x.PostingNumber);
        //}

        #endregion
            
    }
}