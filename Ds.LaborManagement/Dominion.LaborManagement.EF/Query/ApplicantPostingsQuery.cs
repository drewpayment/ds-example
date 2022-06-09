using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.LaborManagement.Dto.Enums;

namespace Dominion.LaborManagement.EF.Query
{
    public class ApplicantPostingsQuery : Query<ApplicantPosting, IApplicantPostingsQuery>, IApplicantPostingsQuery
    {
        public ApplicantPostingsQuery(IEnumerable<ApplicantPosting> data, IQueryResultFactory resultFactory = null)
            : base(data, resultFactory)
        {
        }

        public IApplicantPostingsQuery ByApplicantApplicationHeaderId(int applicantApplicationHeaderId)
        {
            FilterBy(x => x.ApplicantApplicationHeaders.Any(y => y.ApplicationHeaderId == applicantApplicationHeaderId));
            return this;
        }

        public IApplicantPostingsQuery ByExternallyViewable()
        {
            FilterBy(x => x.PostingTypeId == PostingType.Both || x.PostingTypeId == PostingType.External);
            return this;
        }

        IApplicantPostingsQuery IApplicantPostingsQuery.ByClientId(int clientId)
        {
            FilterBy(x => x.ClientId == clientId);
            return this;
        }

        IApplicantPostingsQuery IApplicantPostingsQuery.ByIncludeGeneralApplication(bool flag)
        {
            if (flag == false)
            {
                FilterBy(x => x.IsGeneralApplication == false);
            }
            return this;
        }

        IApplicantPostingsQuery IApplicantPostingsQuery.ByPostingId(int postingId)
        {
            FilterBy(x => x.PostingId == postingId);
            return this;
        }

        IApplicantPostingsQuery IApplicantPostingsQuery.ByPublishDateRange(DateTime startDate, DateTime endDate)
        {
            FilterBy(x => x.PublishStart.Value >= startDate && x.PublishStart.Value < endDate );
            return this;
        }

        IApplicantPostingsQuery IApplicantPostingsQuery.ShowFilledPostings()
        {
            FilterBy(x => x.FilledDate.HasValue &&
                        (x.IsPublished || (x.PublishEnd >= DateTime.Now.Date && x.PublishStart <= DateTime.Now.Date)));
            return this;

        }

        IApplicantPostingsQuery IApplicantPostingsQuery.ShowOpenPostings()
        {
            FilterBy(x => !x.FilledDate.HasValue &&
                          (x.IsPublished ||
                           (x.PublishEnd >= DateTime.Now.Date && x.PublishStart <= DateTime.Now.Date)));
            return this;
        }

        IApplicantPostingsQuery IApplicantPostingsQuery.ByIsActive(bool flag)
        {
            FilterBy(x => x.IsEnabled == flag);
            return this;
        }

        IApplicantPostingsQuery IApplicantPostingsQuery.ByIsClosed(bool flag)
        {
            FilterBy(x => x.IsClosed == flag);
            return this;
        }

        IApplicantPostingsQuery IApplicantPostingsQuery.ByWithinPublishedDates()
        {
            FilterBy(x => (!x.PublishStart.HasValue || x.PublishStart <= DateTime.Now) && (!x.PublishEnd.HasValue || x.PublishEnd >= DateTime.Now) && x.IsPublished);
            return this;
        }

        public IApplicantPostingsQuery WhereFilledDateNotNull()
        {
            FilterBy(x => x.FilledDate >= DateTime.Now || x.FilledDate < DateTime.Now);
            return this;
        }		

        public IApplicantPostingsQuery ByHasNoCompletedApplications()
        {
            FilterBy(x => x.ApplicantApplicationHeaders.Count(y => y.IsApplicationCompleted) == 0);
            return this;
        }

        public IApplicantPostingsQuery ByNoOfDays(DateTime startDate, int days)
        {
            FilterBy(x => SqlFunctions.DateDiff("dd", x.FilledDate, startDate) <= days);
            return this;
        }

        public IApplicantPostingsQuery OrderByDescription()
        {
            OrderBy(x => x.Description);
            return this;
        }

        IApplicantPostingsQuery IApplicantPostingsQuery.ForClientsWithApplicantJobSite(
            ApplicantJobSiteEnum jobSite)
        {
            FilterBy(p => p.Client.ClientJobSites.Any(x => x.ApplicantJobSiteId == jobSite && x.SharePosts == true));
            return this;
        }

        public IApplicantPostingsQuery ByApplicationId(int applicationId)
        {
            FilterBy(x => x.ApplicationId == applicationId);
            return this;
        }

        public IQueryResult<GetNewApplicantsDateByDateSpanInnerJoinResultDto> QueryNewApplicantDataPoints(IApplicantApplicationHeaderQuery headerQuery,
            IApplicantPostingsQuery postingsQuery, int clientId, DateTime startDate, DateTime endDate)
        {
            headerQuery.ByDateSubmitted(startDate, endDate);
            headerQuery.OrderByDateSubmitted();
            headerQuery.ByApplicationCompleted(true);
            postingsQuery.ByClientId(clientId);
            return headerQuery.Result.InnerJoin(postingsQuery.Result, new NewApplicantDataPointsInnerJoin());
        }

        public IQueryResult<IEnumerable<ApplicantDaysToHireDetailDto>> QueryApplicantDaysToHireDetail(
            IApplicantPostingCategoriesQuery postingCategoriesQuery, IApplicantPostingsQuery postingsQuery,
            int clientId)
        {
            postingsQuery.ByIsActive(true);
            postingsQuery.ByClientId(clientId);
            postingsQuery.ByNoOfDays(DateTime.Now, 365);
            postingsQuery.WhereFilledDateNotNull();
            return postingCategoriesQuery.Result.InnerJoin(postingsQuery.Result, new ApplicantDaysToHireDetailJoin())
                .Group(new GroupByIdDescriptionLocationNameDepartmentNamePostingNumber());
        }

        private class NewApplicantDataPointsInnerJoin : IInnerJoinExpressions<ApplicantApplicationHeader,
            ApplicantPosting, int, GetNewApplicantsDateByDateSpanInnerJoinResultDto>
        {
            public Expression<Func<ApplicantApplicationHeader, int>> OuterKey
            {
                get { return header => header.PostingId; }
            }

            public Expression<Func<ApplicantPosting, int>> InnerKey
            {
                get { return posting => posting.PostingId; }
            }

            public Expression<Func<ApplicantApplicationHeader, ApplicantPosting, GetNewApplicantsDateByDateSpanInnerJoinResultDto>> Select
            {
                get
                {
                    return (header, control) => new GetNewApplicantsDateByDateSpanInnerJoinResultDto()
                    {
                        DateSubmitted = header.DateSubmitted.Value,
                        IsExternalApplicant = header.ExternalApplicationIdentity != null,
                        JobSiteName = (header.ExternalApplicationIdentity != null) ?
                                header.ExternalApplicationIdentity.ApplicantJobSite.JobSiteDescription : ""
                    };
                }
            }
        }

        private class GroupByIdDescriptionLocationNameDepartmentNamePostingNumber : IGroupExpressions<ApplicantDaysToHireDetailDto, ApplicantDaysToHireGroupBy, IEnumerable<ApplicantDaysToHireDetailDto>>
        {
            public Expression<Func<ApplicantDaysToHireDetailDto, ApplicantDaysToHireGroupBy>> GroupKey
            {
                get
                {
                    return daysToHire => new ApplicantDaysToHireGroupBy()
                    {
                        PostingId = daysToHire.PostingId,
                        Description = daysToHire.Description,
                        Location = daysToHire.Location,
                        Name = daysToHire.Category,
                        DepartmentName = daysToHire.DepartmentName,
                        PostingNumber = daysToHire.PostingNumber

                    };
                }
            }

            public Expression<Func<IGrouping<ApplicantDaysToHireGroupBy, ApplicantDaysToHireDetailDto>, IEnumerable<ApplicantDaysToHireDetailDto>>> Select
            {
                get
                {
                    return grouping => grouping.Select((x) => new ApplicantDaysToHireDetailDto()
                    {
                        PostingId = x.PostingId,
                        PostingNumber = x.PostingNumber,
                        Description = x.Description,
                        Category = x.Category,
                        DepartmentName = x.DepartmentName,
                        PublishStart = x.PublishStart,
                        StartDate = x.StartDate,
                        FilledDate = x.FilledDate,
                        Location = x.Location
                    });
                }
            }
        }

        private class ApplicantDaysToHireGroupBy
        {
            public int PostingId { get; set; }
            public int PostingNumber { get; set; }
            public string Description { get; set; }
            public string Name { get; set; }
            public string Location { get; set; }
            public string DepartmentName { get; set; }
        }

        private class ApplicantDaysToHireDetailJoin : IInnerJoinExpressions<ApplicantPostingCategory, ApplicantPosting, int, ApplicantDaysToHireDetailDto>
        {
            public Expression<Func<ApplicantPostingCategory, int>> OuterKey
            {
                get { return postingCategory => postingCategory.PostingCategoryId; }
            }

            public Expression<Func<ApplicantPosting, int>> InnerKey
            {
                get { return appPosting => appPosting.PostingCategoryId; }
            }

            public Expression<Func<ApplicantPostingCategory, ApplicantPosting, ApplicantDaysToHireDetailDto>> Select
            {
                get
                {
                    return (postingCategory, appPosting) => new ApplicantDaysToHireDetailDto()
                    {
                        PostingId = appPosting.PostingId,
                        PostingNumber = appPosting.PostingNumber,
                        Description = appPosting.Description,
                        Category = postingCategory.Name,
                        DepartmentName = appPosting.ClientDepartment.Name,
                        PublishStart = appPosting.PublishStart,
                        StartDate = appPosting.StartDate,
                        FilledDate = appPosting.FilledDate,
                        Location = (appPosting.ClientDivision != null) ?
                                    (appPosting.ClientDivision.City ?? "") + ", " +
                                    (appPosting.ClientDivision.State != null ? appPosting.ClientDivision.State.Abbreviation ?? "" : "") : ""
                    };
                }
            }
        }
    }
}
