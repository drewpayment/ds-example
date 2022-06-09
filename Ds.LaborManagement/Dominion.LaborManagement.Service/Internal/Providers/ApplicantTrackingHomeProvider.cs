using System;
using System.Collections.Generic;
using System.Linq;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Services.Interfaces;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.LaborManagement.Service.Api;
using Dominion.Utility.OpResult;
using ApplicantStatusType = Dominion.LaborManagement.Dto.ApplicantTracking.ApplicantStatusType;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    /// <summary>
    /// This provider defines the business logic that should be invoked when the user is authorized and is allowed to perform the action specified.
    /// This is done by wrapping a function in an object that has the parameters it needs to invoke the function.
    /// </summary>
    public class ApplicantTrackingHomeProvider: IApplicantTrackingHomeProvider
    {
        
        private readonly IBusinessApiSession _session;

        public ApplicantTrackingHomeProvider(IBusinessApiSession session)
        {
            _session = session;
        }

        /// <summary>
        /// Gets the number of applicants who submitted their application for each day in the supplied date range
        /// </summary>
        /// <param name="clientId">The id to use when retrieving the applicants</param>
        /// <param name="startDate">The date to start looking for appplicants</param>
        /// <param name="endDate">The date to stop looking for applicants</param>
        /// <returns>A function that will return an IEnumerable of NewApplicantDataPointDtos.  Each dto contains the date and the number of applicants who submitted their application that day.</returns>
        public IOpResult<IEnumerable<NewApplicantDataPointDto>> GetNewApplicantsDataPointsByDateSpanFn(int clientId, DateTime startDate, DateTime endDate)
        {
            var opResult = new OpResult<IEnumerable<NewApplicantDataPointDto>>();

            var dataPoints = new List<NewApplicantDataPointDto>();
            var headerQuery = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery();
            var postingQuery = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery();

            var datesSubmitted = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .QueryNewApplicantDataPoints(headerQuery, postingQuery, clientId, startDate, endDate)
                .Execute();

            foreach (var joinResultDto in datesSubmitted)
            {
                var lastDataPoint = dataPoints.Find(x=>x.Date.Date.CompareTo(joinResultDto.DateSubmitted.Date) == 0);
                if (lastDataPoint != null )
                {
                    lastDataPoint.Applicants++;
                }
                else
                {
                    lastDataPoint = new NewApplicantDataPointDto()
                    {
                        Date = joinResultDto.DateSubmitted,
                        Applicants = 1,
                        ExternalApplicants = new Dictionary<string, int>()
                    };
                    dataPoints.Add(lastDataPoint);
                }

                // count external applicants based on jobsite name respectively
                if (joinResultDto.IsExternalApplicant)
                {
                    if (lastDataPoint.ExternalApplicants.ContainsKey(joinResultDto.JobSiteName))
                        lastDataPoint.ExternalApplicants[joinResultDto.JobSiteName]++;
                    else
                        lastDataPoint.ExternalApplicants.Add(joinResultDto.JobSiteName, 1);
                }
            }
            opResult.Data = dataPoints;

            return opResult;

        }

        private int GetDaysToFill(DateTime? publishStart,DateTime? postStartDate, DateTime? filledDate)
        {
            DateTime? startDate = publishStart ?? postStartDate;

            if (!startDate.HasValue)
            {
                // In case if the startdate is unavailable, pick the startdate of the quarter
                // during which the position is filled.
                DateTime tempDate = new DateTime(filledDate.Value.Year, 1, 1);
                if (filledDate.Value.Month >= 4 & filledDate.Value.Month <= 6)
                    tempDate = new DateTime(filledDate.Value.Year, 4, 1);
                else if (filledDate.Value.Month >= 7 & filledDate.Value.Month <= 9)
                    tempDate = new DateTime(filledDate.Value.Year, 7, 1);
                else if (filledDate.Value.Month >= 10 & filledDate.Value.Month <= 12)
                    tempDate = new DateTime(filledDate.Value.Year, 10, 1);

                return (filledDate.Value - tempDate).Days;
            }

            return (filledDate.Value - startDate.Value).Days;
        }

        /// <summary>
        /// Gets the average days to fill positions for the current quarter and year.  
        /// This is the method used to get the data for the 'Average Days To Hire' widget on the ApplicantTrackingHome page.
        /// </summary>
        /// <param name="clientId">The id of the client to get the postings for</param>
        /// <returns>A function that will return a dto containing the average days to fill positions for the current quarter and year.</returns>
        public IOpResult<AverageDaysToFillDto> GetAverageDaysToFillPositionsFn(int clientId)
        {
            var opResult = new OpResult<AverageDaysToFillDto>();

            var yearly = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery().WhereFilledDateNotNull()
                .ByClientId(clientId)
                .ByIncludeGeneralApplication(false)
                .ByNoOfDays(DateTime.Now, 365).ExecuteQuery();
            double yearlyAverage = 0;
            var applicantPostings = yearly as ApplicantPosting[] ?? yearly.ToArray();
            if (applicantPostings.Any())
            {
                yearlyAverage =
                    applicantPostings.Average(x =>
                        GetDaysToFill(x.PublishStart, x.StartDate, x.FilledDate)); // entries with null FilledDate values are filtered out in db query so we should be fine here
            }

            var quarterly = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery().WhereFilledDateNotNull()
                .ByClientId(clientId)
                .ByIncludeGeneralApplication(false)
                .ByNoOfDays(DateTime.Now, 90).ExecuteQuery();

            double quarterlyAverage = 0;
            var quarterlyPostings = quarterly as ApplicantPosting[] ?? quarterly.ToArray();
            if (quarterlyPostings.Any())
            {
                quarterlyAverage =
                    quarterlyPostings.Average(x =>
                        GetDaysToFill(x.PublishStart, x.StartDate, x.FilledDate)); // entries with null FilledDate values are filtered out in db query so we should be fine here
            }

            opResult.Data = new AverageDaysToFillDto()
            {
                Quarterly = quarterlyAverage,
                Yearly = yearlyAverage
            };
            return opResult;
        }

        /// <summary>
        /// Gets all of the needed details of the filled postings in the past year for the 'Average Days To Hire' modal.
        /// </summary>
        /// <param name="clientId">The id of the client to get the postings for</param>
        /// <returns>A function that will return an IEnumerable of the grouped posting details to be displayed</returns>
        public IOpResult<IEnumerable<IEnumerable<ApplicantDaysToHireDetailDto>>> GetAverageDaysToFillPositionsDetailFn(int clientId)
        {
            var result = new OpResult<IEnumerable<IEnumerable<ApplicantDaysToHireDetailDto>>>();
            var postingCategoriesQuery =
                _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingCategoriesQuery();
            var postingsQuery = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery();
            var query = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .QueryApplicantDaysToHireDetail(postingCategoriesQuery, postingsQuery, clientId).Execute();

            foreach(IEnumerable<ApplicantDaysToHireDetailDto> dtoList in query)
                foreach (ApplicantDaysToHireDetailDto dto in dtoList)
                    dto.AverageDaysToHire = GetDaysToFill(dto.PublishStart, dto.StartDate, dto.FilledDate);

            result.Data = query;
            return result;
        }

        public IOpResult<IEnumerable<PostingApplicationsDetailDto>> GetPostingApplicationsDetailFn(int clientId)
        {

            var result = new OpResult<IEnumerable<PostingApplicationsDetailDto>>();

            IEnumerable<PostingApplicationsDetailDto> query = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                    .ByClientId(clientId)
                    .ByIsClosed(false)
                    .ByIsActive(true)
                    .ExecuteQueryAs(x => new PostingApplicationsDetailDto()
                    {
                        PostingId = x.PostingId,
                        PostingNumber = x.PostingNumber,
                        Description = x.Description,
                        Category = x.ApplicantPostingCategory.Name,
                        DepartmentName = x.ClientDepartment.Name,
                        PublishStart = x.PublishStart,
                        StartDate = x.StartDate,
                        FilledDate = x.FilledDate,
                        Location = (x.ClientDivision != null) ?
                                    (x.ClientDivision.City ?? "") + ", " +
                                    (x.ClientDivision.State != null ? x.ClientDivision.State.Abbreviation ?? "" : "") : "",
                        NumOfPositions = (x.NumOfPositions.HasValue && x.NumOfPositions.Value > 0) ? x.NumOfPositions.Value : 1,
                        Applications = x.ApplicantApplicationHeaders.Where(z => z.IsApplicationCompleted).Count(),
                        Applicants = x.ApplicantApplicationHeaders.Where(z => z.IsApplicationCompleted).Count(y => y.ApplicantStatusTypeId == ApplicantStatusType.Applicant),
                        ApplicantsHired = x.ApplicantApplicationHeaders.Count(y => y.ApplicantStatusTypeId == ApplicantStatusType.Hired),
                        Candidates = x.ApplicantApplicationHeaders.Where(z => z.IsApplicationCompleted).Count(y => y.ApplicantStatusTypeId == ApplicantStatusType.Candidate),
                        IsGeneralApplication = x.IsGeneralApplication
                    });

            result.Data = query;
            return result;
        }

        /// <summary>
        /// Gets the data for the 'Turnover Rate' dto on the ApplicantTrackingHome page
        /// </summary>
        /// <param name="clientId">The client id used to retrieve the employees hired</param>
        /// <returns>A function that will return a dto with data to be displayed in the 'Turnover Rate' widget on the ApplicantTrackingHome page</returns>
        public IOpResult<TurnoverRateDto> GetEmployeeTurnoverRateFn(int clientId)
        {
                var result = new OpResult<TurnoverRateDto>();

                //filter entries by clientid so that we avoid making 3 calls to the db (one call for each result set with unique constraints--see for each loop below).
                var employeePay = _session.UnitOfWork.PayrollRepository.QueryEmployeePay().ByClient(clientId);
                var employee = _session.UnitOfWork.EmployeeRepository.QueryEmployees();
                var allData = _session.UnitOfWork.EmployeeRepository.EmployeeStatusQuery().JoinEmployeePayEmployeeStatusAndEmployee(employeePay, employee, clientId).Execute();

                var startCount = 0;
                var endCount = 0;
                var termedCount = 0;

                var endDate = DateTime.Now;
                var startDate = endDate.AddDays(-365);

                //Go through all data returned and extract: 
                //the amount of active employees hired before the start date
                //the amount of active employees hired before the end date
                //and the amount of deactivated employees who were 'separated' between the start date and the end date or who were not 'separated' but whose employeepay record was modified between the start date and end date
                foreach (var datum in allData)
                {
                    if (datum.HireDate < startDate && datum.Active)
                    {
                        startCount++;
                    }

                    if (datum.HireDate < endDate && datum.Active)
                    {
                        endCount++;
                    }

                    if (((datum.SeparationDate >= startDate && datum.SeparationDate <= endDate) ||
                        (!(datum.SeparationDate >= DateTime.Now || datum.SeparationDate <= DateTime.Now) &&
                         datum.Modified >= startDate && datum.Modified <= endDate)) && !datum.Active)
                    {
                        termedCount++;
                    }
                }

                result.Data = new TurnoverRateDto()
                {
                    EndCount = endCount,
                    StartCount = startCount,
                    TermedCount = termedCount
                };
                return result;
        }

        /// <summary>
        /// Gets the amount of deactivated employees who were 'separated' between the 
        /// start date and the end date OR who were not 'separated' but whose employeepay 
        /// record was modified between the start date and end date.
        /// </summary>
        /// <param name="clientId">The client id used to filter out employee pay records</param>
        /// <returns>A function that will return a dto containing the data to be displayed in the 'Turnover Rate' modal</returns>
        public IOpResult<IEnumerable<EmployeePayAndEmployeeStatusAndEmployee>> GetTerminatedEmployeesDetailFn(int clientId)
        {
                var opResult = new OpResult<IEnumerable<EmployeePayAndEmployeeStatusAndEmployee>>();

                var employeePay = _session.UnitOfWork.PayrollRepository.QueryEmployeePay().ByClient(clientId);
                var employee = _session.UnitOfWork.EmployeeRepository.QueryEmployees();
                var allData = _session.UnitOfWork.EmployeeRepository.EmployeeStatusQuery().JoinEmployeePayEmployeeStatusAndEmployee(employeePay, employee, clientId).Execute();

                var resultList = new List<EmployeePayAndEmployeeStatusAndEmployee>();

                var endDate = DateTime.Now;
                var startDate = endDate.AddDays(-365);

                foreach (var datum in allData)
                {
                    if (((datum.SeparationDate >= startDate && datum.SeparationDate <= endDate) ||
                         (!(datum.SeparationDate >= DateTime.Now || datum.SeparationDate <= DateTime.Now) &&
                          datum.Modified >= startDate && datum.Modified <= endDate)) && !datum.Active)
                    {
                        resultList.Add(datum);
                    }
                }

                opResult.Data = resultList;

                return opResult;
        }

        public IOpResult<IEnumerable<EmployeePayAndEmployeeStatusAndEmployee>> GetActiveEmployeesDetailFn(int clientId, DateTime startDate, DateTime endDate)
        {
            var opResult = new OpResult<IEnumerable<EmployeePayAndEmployeeStatusAndEmployee>>();
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(clientId);
            var employeePay = _session.UnitOfWork.PayrollRepository.QueryEmployeePay().ByClient(clientId);
            var employee = _session.UnitOfWork.EmployeeRepository.QueryEmployees();
            var allData = _session.UnitOfWork.EmployeeRepository.EmployeeStatusQuery().JoinEmployeePayEmployeeStatusAndEmployee(employeePay, employee, clientId).Execute();

            var resultList = new List<EmployeePayAndEmployeeStatusAndEmployee>();

            foreach (var datum in allData)
            {
                if (datum.Active == true && datum.HireDate < endDate)
                {
                    resultList.Add(datum);
                }
            }

            opResult.Data = resultList;

            return opResult;
        }
    

    /// <summary>
    /// Gets all of the rejected candidates and groups them by the reason why they were rejected.  
    /// This data is used in the 'Rejection Reasons' widget on the applicant tracking home page.
    /// </summary>
    /// <param name="clientId">The id of the client to get the applicants for</param>
    /// <returns>A function that will return an IEnumerable of dtos containing the rejection reason and the amount of applicants for that reason.</returns>
    public IOpResult<IEnumerable<RejectionReasonsDto>> GetApplicantRejectionReasonCountsFn(int clientId)
        {
                var opResult = new OpResult<IEnumerable<RejectionReasonsDto>>();

                var applicants = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantsQuery().ByClientId(clientId).ExecuteQueryAs(
                    applicant => applicant.ApplicantId);
                var headerQuery = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
                    .ByApplicationCompleted(true).ByApplicantIdIn(applicants);
                var rejectionQuery = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantRejectionReasonQuery();
                var result = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantRejectionReasonQuery()
                    .JoinApplicantApplicationHeaderAndApplicantRejectionReason(rejectionQuery, headerQuery);
                opResult.Data = result.Execute();

                return opResult;
        }


        /// <summary>
        /// Gets the number of posts for a client that do not have a completed application
        /// </summary>
        /// <param name="clientId">The id of the client to get the posts for</param>
        /// <returns></returns>
        public IOpResult<PostsWithoutApplicantsDto> GetTheNumberOfPostsWithoutApplicantsFn(int clientId)
        {
            var opResult = new OpResult<PostsWithoutApplicantsDto>();

            var totalPosts = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                .ByClientId(clientId)
                .ByHasNoCompletedApplications().ByIsActive(true).ByIsClosed(false).ExecuteQuery();

            opResult.Data = new PostsWithoutApplicantsDto()
            {
                TotalPostingsWithoutApplicants = totalPosts.Count()
            };

            return opResult;
        }

        public IOpResult<ApplicantsToReviewDto> GetTheNumberApplicantsToReviewFn(int clientId)
        {
            var opResult = new OpResult<ApplicantsToReviewDto>();

            var totalApplicantsToReview = _session.UnitOfWork.ApplicantTrackingRepository
                .ApplicantApplicationHeaderQuery()
                .ByApplicantPostingClientId(clientId)
                .ByApplicantPostingClosed(false)
                .ByApplicantPostingEnabled(true)
                .ByApplicantStatusTypeId(0)
                .ByApplicationCompleted(true)
                .ExecuteQuery();

            opResult.Data = new ApplicantsToReviewDto()
            {
                TotalApplicantsToReview = totalApplicantsToReview.Count()
            };

            return opResult;
        }

        public IOpResult<CandidateCountDto> GetTheTotalNumberOfCandidatesFn(int clientId)
        {
            var opResult = new OpResult<CandidateCountDto>();

            var totalApplicants = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantApplicationHeaderQuery()
                .ByApplicantPostingClientId(clientId)
                .ByApplicantPostingClosed(false)
                .ByApplicantPostingEnabled(true)
                .ByApplicationCompleted(true)
                .ByApplicantStatusTypeId(ApplicantStatusType.Candidate)
                .ExecuteQuery();

            opResult.Data = new CandidateCountDto()
            {
                TotalNumberOfCandidates = totalApplicants.Count()
            };

            return opResult;
        }

        public IOpResult<ApplicantTrackingHomePageDto> GetAllDataForHomePageFn(int clientId, DateTime startDate, DateTime endDate)
        {
            var result = new OpResult<ApplicantTrackingHomePageDto>
            {
                Data = new ApplicantTrackingHomePageDto
                {
                    NewApplicantDataPoints = GetNewApplicantsDataPointsByDateSpanFn(clientId, startDate, endDate).Data,
                    AverageDaysToFillDto = GetAverageDaysToFillPositionsFn(clientId).Data,
                    AverageDaysToFillDetailDto = GetAverageDaysToFillPositionsDetailFn(clientId).Data,
                    TurnoverRateDto = GetEmployeeTurnoverRateFn(clientId).Data,
                    TerminatedEmployees = GetTerminatedEmployeesDetailFn(clientId).Data,
                    RejectionReasonDtos = GetApplicantRejectionReasonCountsFn(clientId).Data,
                    PostsWithoutApplicantsDto = GetTheNumberOfPostsWithoutApplicantsFn(clientId).Data,
                    ApplicantsToReviewDto = GetTheNumberApplicantsToReviewFn(clientId).Data,
                    CandidateCountDto = GetTheTotalNumberOfCandidatesFn(clientId).Data,
                    OpenPostsToFillDto = GetTheNumberOfOpenPosts(clientId).Data
                }
            };
            return result;
        }

        public IOpResult<OpenPostsToFillDto> GetTheNumberOfOpenPosts(int clientId)
        {
            var result = new OpResult<OpenPostsToFillDto>
            {
                Data = new OpenPostsToFillDto()
                {
                    OpenPosts = _session.UnitOfWork.ApplicantTrackingRepository.ApplicantPostingsQuery()
                        .ByClientId(clientId)
                        .ByIncludeGeneralApplication(false)
                        .ByIsClosed(false)
                        .ByIsActive(true)
                        .ExecuteQueryAs(x => new
                        {
                            Posting = x.PostingId,
                            NumOfPositions = (x.NumOfPositions.HasValue && x.NumOfPositions.Value > 0) ? x.NumOfPositions.Value : 1,
                            ApplicantsHired = x.ApplicantApplicationHeaders.Count(y => y.ApplicantStatusTypeId == ApplicantStatusType.Hired)
                        }).Sum(x => (x.NumOfPositions > x.ApplicantsHired) ? (x.NumOfPositions - x.ApplicantsHired) : 0)
                }
            };

            return result;
        }
    }
}
