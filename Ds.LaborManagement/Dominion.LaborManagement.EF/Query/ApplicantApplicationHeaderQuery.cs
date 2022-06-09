using System.Collections.Generic;
using System.Linq;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using System;
using System.Data.Entity.SqlServer;
using System.Linq.Expressions;
using Dominion.Core.Dto.Location;
using Dominion.Domain.Entities.Misc;
using ApplicantStatusType = Dominion.LaborManagement.Dto.ApplicantTracking.ApplicantStatusType;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantApplicationHeaderQuery : Query<ApplicantApplicationHeader, IApplicantApplicationHeaderQuery>, IApplicantApplicationHeaderQuery
    {
        #region Constructor

        public ApplicantApplicationHeaderQuery(IEnumerable<ApplicantApplicationHeader> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        #endregion

        public IApplicantApplicationHeaderQuery ByApplicationHeaderId(int applicationHeaderId)
        {
            FilterBy(x => x.ApplicationHeaderId == applicationHeaderId);
            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ByApplicantId(int applicantId)
        {
            FilterBy(x => x.ApplicantId == applicantId);
            return this;
        }

        //IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ByPostingId(int postingId)
        //{
        //    FilterBy(x => x.PostingId == postingId);
        //    return this;
        //}

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ByApplicantStatusTypeId(ApplicantStatusType? applicantStatusTypeId)
        {
            FilterBy(x => x.ApplicantStatusTypeId == applicantStatusTypeId);
            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.RejectableApplications()
        {
            FilterBy(x => x.ApplicantStatusTypeId == null || ((x.ApplicantStatusTypeId != Dto.ApplicantTracking.ApplicantStatusType.Rejected) && x.ApplicantStatusTypeId != Dto.ApplicantTracking.ApplicantStatusType.Hired));
            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ByDateSubmitted(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue)
            {
                startDate = DateTime.Now.Date.AddMonths(-6);
            }

            if (!endDate.HasValue)
            {
                endDate = DateTime.Now.Date;
            }

            var startDt = startDate.Value.Date;
            var endDt = endDate.Value.Date;

            FilterBy(x => x.DateSubmitted >= startDt);

            FilterBy(x => x.DateSubmitted <= endDt);

            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ByApplicationCompleted(bool flag)
        {
            FilterBy(x => x.IsApplicationCompleted == flag);
            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ByPostingId(int? postingId)
        {
            if (postingId.HasValue)
            {
                if (postingId > 0)
                {
                    FilterBy(x => x.ApplicantPosting.PostingId == postingId);
                }
                else
                {
                    if (postingId == -1)
                    {
                        FilterBy(x => x.ApplicantPosting.IsClosed == false);
                    }
                }
            }
            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ByApplicantPostingEnabled(bool flag)
        {
            FilterBy(x => x.ApplicantPosting.IsEnabled == flag);
            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ByApplicantPostingClosed(bool flag)
        {
            FilterBy(x => x.ApplicantPosting.IsClosed == flag);
            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ByJobTypeId(int? jobTypeId)
        {
            if (jobTypeId.HasValue && jobTypeId > 0)
            {
                FilterBy(x => (int)x.ApplicantPosting.JobTypeId == jobTypeId);
            }
            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ByJobProfileId(int? jobProfileId)
        {
            if (jobProfileId.HasValue && jobProfileId > 0)
            {
                FilterBy(x => x.ApplicantPosting.JobProfileId == jobProfileId);
            }
            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ByPostingCategoryId(int? postingCategoryId)
        {
            if (postingCategoryId.HasValue && postingCategoryId > 0)
            {
                FilterBy(x => x.ApplicantPosting.PostingCategoryId == postingCategoryId);
            }
            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ByPostingOwnerId(int? postingOwnerId)
        {
            if (postingOwnerId.HasValue && postingOwnerId > 0)
            {
                FilterBy(x => x.ApplicantPosting.ApplicantPostingOwners.Any(y => y.UserId == postingOwnerId.Value));
            }
            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ByDivisionId(int? divisionId)
        {
            if (divisionId.HasValue && divisionId > 0)
            {
                FilterBy(x => x.ApplicantPosting.ClientDivisionId == divisionId);
            }
            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ByDepartmentId(int? departmentId)
        {
            if (departmentId.HasValue && departmentId > 0)
            {
                FilterBy(x => x.ApplicantPosting.ClientDepartmentId == departmentId);
            }
            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ViewDenied(bool flag)
        {
            if (!flag)
            {
                FilterBy(x => x.Applicant.IsDenied == false);
            }
            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ByKeyword(string keyword)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                FilterBy(x => x.Applicant.ApplicantEmploymentHistories.Any(y => y.Responsibilities.Contains(keyword)) ||
                              x.Applicant.ApplicantEmploymentHistories.Any(y => y.Title.Contains(keyword)) ||
                              x.Applicant.ApplicantEmploymentHistories.Any(y => y.Company.Contains(keyword)) ||
                              x.ApplicantApplicationDetail.Any(y => y.Response.Contains(keyword)) ||
                              x.Applicant.ApplicantEducationHistories.Any(y => y.Studied.Contains(keyword)) ||
                              x.Applicant.ApplicantEducationHistories.Any(y => y.Description.Contains(keyword)) ||
                              x.Applicant.City.Contains(keyword) ||
                              x.Applicant.EmailAddress.Contains(keyword) ||
                              x.ApplicantApplicationDetail
                               .Any(y => y.ApplicantQuestionControl
                                          .ApplicantQuestionDdlItem.Any(z => z.Description.Contains(keyword))
                                    )
                        );
            }
            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ByName(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                FilterBy(x => x.Applicant.FirstName.Contains(name) ||
                              x.Applicant.LastName.Contains(name)
                        );
            }
            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ByPostingNumber(int? postingNumber)
        {
            if (postingNumber.HasValue && postingNumber > 0)
            {
                FilterBy(x => x.ApplicantPosting.PostingNumber == postingNumber);
            }
            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ByApplicantPostingClientId(int? clientId)
        {
            if (clientId.HasValue && clientId > 0)
            {
                FilterBy(x => x.ApplicantPosting.ClientId == clientId);
            }
            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ByApplicantClientId(int? clientId)
        {
            if (clientId.HasValue && clientId > 0)
            {
                FilterBy(x => x.Applicant.ClientId == clientId);
            }
            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.IgnoreNullStatus()
        {
            FilterBy(x => x.ApplicantStatusTypeId != null);
            return this;
        }

        public IApplicantApplicationHeaderQuery OrderByDateSubmitted()
        {
            OrderBy(x => x.DateSubmitted);
            return this;
        }

        public IApplicantApplicationHeaderQuery ByApplicantIdIn(IEnumerable<int> ids)
        {
            FilterBy(x => ids.Contains(x.ApplicantId));
            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ByApplicantRejectionReasonId(int? applicantRejectionReasonId)
        {
            FilterBy(x => x.ApplicantRejectionReasonId == applicantRejectionReasonId);
            return this;
        }

        public IApplicantApplicationHeaderQuery ByRatingSelection(List<bool> ratings)
        {
            if (ratings != null && ratings.Count == 6)
            {
                List<int> temp = new List<int>();
                for (int i = 0; i < 6; i++) if (ratings[i]) temp.Add(i);

                if( temp.Count > 0 )
                    FilterBy(x => temp.Contains(x.Score.HasValue ? x.Score.Value : 0) );
            }
            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ByExternalApplicationId(string externalApplicationId)
        {
            FilterBy(x => (x.ExternalApplicationIdentity != null && x.ExternalApplicationIdentity.ExternalApplicationId == externalApplicationId));
            return this;
        }

        IApplicantApplicationHeaderQuery IApplicantApplicationHeaderQuery.ByCoverLetterIsNotNull()
        {
            FilterBy(x => x.CoverLetterId != null);
            return this;
        }
    }
}
