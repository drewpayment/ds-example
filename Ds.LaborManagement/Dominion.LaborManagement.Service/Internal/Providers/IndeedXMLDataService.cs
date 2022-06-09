using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dominion.Core.Services.Interfaces;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Entities.Clients;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.LaborManagement.Dto.Enums;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    /// <inheritdoc cref="IXMLDataService"/>
    public class IndeedXMLDataService : IXMLDataService
    {
        private readonly IBusinessApiSession _session;
        public IndeedXMLDataService(IBusinessApiSession session)
        {
            _session = session;
        }

        /// <inheritdoc cref="IXMLDataService.GetJobPostsForIndeed"/>
        public IEnumerable<IndeedXmlJobPost> GetJobPostsForIndeed()
        {
            var jobSites = _session.UnitOfWork.ApplicantTrackingRepository.ClientJobSiteQuery()
                .BySharePosts(true)
                .ByApplicantJobSiteId(ApplicantJobSiteEnum.Indeed)
                .ExecuteQuery();

            // Make map for job site emails the client associated
            IDictionary<int, string> emails = new Dictionary<int, string>();
            foreach (ClientJobSite site in jobSites) {
                if(!string.IsNullOrWhiteSpace(site.Email))
               emails.Add(site.ClientId, site.Email);
            }

            IEnumerable<IndeedXmlJobPost> result = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                 .ForClientsWithApplicantJobSite(ApplicantJobSiteEnum.Indeed)
                 .ByIsActive(true)
                 .ByIsClosed(false)
                 .ByExternallyViewable()
                 .ByWithinPublishedDates()
                 .ExecuteQueryAs(posting => new IndeedXmlJobPost()
                 {
                     Date = posting.PublishStart ?? posting.PublishedDate,
                     ReferenceNumber = posting.PostingId,
                     Description = posting.JobRequirements,
                     Salary = posting.Salary,
                     Title = posting.Description,
                     Company = posting.Client.ApplicantClient.JobBoardTitle ?? posting.Client.ClientName,
                     City = posting.ClientDivision.City ?? posting.Client.City,
                     Country = posting.ClientDivision.Country.Abbreviation ?? posting.Client.Country.Abbreviation,
                     PostalCode = posting.ClientDivision.Zip ?? posting.Client.PostalCode,
                     State = posting.ClientDivision.State.Abbreviation ?? posting.Client.State.Abbreviation,
                     JobType = posting.EmployeeStatus.Description,
                     Education = posting.ApplicantSchoolType.Description,
                     Category = posting.ApplicantPostingCategory.Name,
                     ClientId = posting.ClientId,
                     ClientCode = posting.Client.ClientCode,
                     ClientEmail = null,
                     SourceName = posting.Client.ApplicantClient.JobBoardTitle ?? posting.Client.ClientName,
                     Experience = posting.ApplicantCompanyApplication.YearsOfEmployment,
                     ResumeRequiredId = posting.ApplicantResumeRequiredId,
                     ApplicationId = posting.ApplicationId,
                     Responsibilities = posting.JobProfile.JobProfileResponsibilities.Select(x => x.JobResponsibilities.Description).ToList(),
                     Skills = posting.JobProfile.JobProfileSkills.Select(x => x.JobSkills.Description).ToList(),
                     WorkingConditions = posting.JobProfile.WorkingConditions,
                     Benefits = posting.JobProfile.Benefits
                 });

            // Update the job site emails to the posts
            foreach(IndeedXmlJobPost post in result)
                post.ClientEmail = emails.Keys.Contains(post.ClientId) ? emails[post.ClientId] : "careers@dominionsystems.com";

            return result;
        }

    }
}
