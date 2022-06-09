using Dominion.Core.EF.Abstract;
using Dominion.Core.EF.Interfaces;
using Dominion.Core.EF.Query;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Domain.Interfaces.Repositories;
using Dominion.LaborManagement.EF.Query;
using Dominion.Utility.Query;
using System.Collections.Generic;
using Dominion.Core.Dto.Misc;
using Dominion.Core.Dto.Sprocs;
using Dominion.Core.EF.Query.Sprocs;
using Dominion.Domain.Interfaces;
using Dominion.Domain.Interfaces.Query.Labor;
using Dominion.LaborManagement.Dto.ApplicantTracking;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.LaborManagement.EF.Sprocs;
using Dominion.Core.Dto.TimeCard.Result;
using Dominion.Core.EF.Query.Labor;

namespace Dominion.LaborManagement.EF.Repository
{
    public class LaborManagementRepository : RepositoryBase, ILaborManagementRepository
    {
        #region Constructors

        /// <summary>
        /// Instantiates a new LaborManagementRepository instance.
        /// </summary>
        /// <param name="context">Context the repository will perform data queries on.</param>
        /// <param name="resultFactory">Result factory used to create & execute query results. If null, a 
        /// <see cref="BasicQueryResultFactory"/> will be used.
        /// </param>
        public LaborManagementRepository(IDominionContext context, IQueryResultFactory resultFactory = null)
            : base(context, resultFactory)
        {
        }

        #endregion

        #region ILaborManagementRepository


        IHolidayQuery ILaborManagementRepository.HolidayQuery()
        {
            return new HolidayQuery(_context.Holiday, this.QueryResultFactory);
        }

        IHolidayDateQuery ILaborManagementRepository.HolidayDateQuery()
        {
            return new HolidayDateQuery(_context.HolidayDate, this.QueryResultFactory);
        }

        IClockClientHolidayChangeHistoryQuery ILaborManagementRepository.ClockClientHolidayChangeHistoryQuery()
        {
            return new ClockClientHolidayChangeHistoryQuery(_context.ClockClientHolidayChangeHistories, this.QueryResultFactory);
        }


        /// <summary>
        /// Constructs a new <see cref="GroupSchedule"/> query.
        /// </summary>
        /// <returns></returns>
        IGroupScheduleQuery ILaborManagementRepository.GroupScheduleQuery()
        {
            return new GroupScheduleQuery(_context.GroupSchedules, this.QueryResultFactory);
        }

        /// <summary>
        /// highfix: jay: needs test.
        /// Constructs a new <see cref="GroupScheduleShift"/> query.
        /// </summary>
        /// <returns></returns>
        IGroupScheduleShiftQuery ILaborManagementRepository.GroupScheduleShiftQuery()
        {
            return new GroupScheduleShiftQuery(_context.GroupScheduleShifts, this.QueryResultFactory);
        }

        /// <summary>
        /// Constructs a query for client cost centers.
        /// TABLE: dbo.ClientCostCenter
        /// </summary>
        /// <returns></returns>
        IClientCostCenterQuery ILaborManagementRepository.ClientCostCenterQuery()
        {
            return new ClientCostCenterQuery(_context.ClientCostCenters, this.QueryResultFactory);
        }

        //IClientAccrualQuery ILaborManagementRepository.ClientAccrualQuery()
        //{
        //    return new ClientAccrualQuery(_context.ClientAccruals, this.QueryResultFactory);
        //}

        //IClientAccrualEarningQuery ILaborManagementRepository.ClientAccrualEarningQuery()
        //{
        //    return new ClientAccrualEarningQuery(_context.ClientAccrualEarnings, this.QueryResultFactory);
        //}

        IEmployeeDefaultShiftQuery ILaborManagementRepository.EmployeeDefaultShiftQuery()
        {
            return new EmployeeDefaultShiftQuery(_context.EmployeeDefaultShifts, this.QueryResultFactory);
        }

        IEmployeeSchedulePreviewQuery ILaborManagementRepository.EmployeeSchedulePreviewQuery()
        {
            return new EmployeeSchedulePreviewQuery(_context.EmployeeSchedulePreviews, this.QueryResultFactory);
        }

        /// <summary>
        /// Creates a query for ClockClientSchedule entities
        /// TABLE: ClockClientSchedule
        /// </summary>
        /// <returns></returns>
        IClockClientScheduleQuery ILaborManagementRepository.ClockClientScheduleQuery()
        {
            return new ClockClientScheduleQuery(_context.ClockClientSchedules, this.QueryResultFactory);
        }

        public IClockClientScheduleChangeHistoryQuery ClockClientScheduleChangeHistoryQuery()
        {
            return new ClockClientScheduleChangeHistoryQuery(_context.ClockClientScheduleChanges, this.QueryResultFactory);
        }

        /// <summary>
        /// Creates a query for Clock Employee BenefitEntities
        /// TABLE: ClockEmployeeBenefit
        /// </summary>
        /// <returns></returns>
        IClockEmployeeBenefitQuery ILaborManagementRepository.ClockEmployeeBenefitQuery()
        {
            return new ClockEmployeeBenefitQuery(_context.ClockEmployeeBenefits, this.QueryResultFactory);
        }

        /// <summary>
        /// Creates a query for ClockEmployeeCostCenter
        /// TABLE: ClockEmployeeCostCenter
        /// </summary>
        /// <returns></returns>
        IClockEmployeeCostCenterQuery ILaborManagementRepository.ClockEmployeeCostCenterQuery()
        {
            return new ClockEmployeeCostCenterQuery(_context.ClockEmployeeCostCenters, this.QueryResultFactory);
        }

        /// <summary>
        /// Creates a query for clock employee schedules entities
        /// TABLE:  ClockEmployeeSchedule
        /// </summary>
        /// <returns></returns>
        IClockEmployeeScheduleQuery ILaborManagementRepository.ClockEmployeeScheduleQuery()
        {
            return new ClockEmployeeScheduleQuery(_context.ClockEmployeeSchedules, this.QueryResultFactory);
        }

        IClientDepartmentQuery ILaborManagementRepository.ClientDepartmentQuery()
        {
            return new ClientDepartmentQuery(_context.ClientDepartments, this.QueryResultFactory);
        }

        //sync-sept-14th
        //public IApplicantsQuery ApplicantsQuery()
        //{
        //    return new ApplicantsQuery(_context.Applicants, this.QueryResultFactory);
        //}

        public IApplicantPostingsQuery ApplicantPostingsQuery()
        {
            return new ApplicantPostingsQuery(_context.ApplicantPostings, this.QueryResultFactory);
        }

        public IApplicantPostingCategoriesQuery ApplicantPostingCategoriesQuery()
        {
            return new ApplicantPostingCategoriesQuery(_context.ApplicantPostingCategories, this.QueryResultFactory);
        }

        IClientDivisionQuery ILaborManagementRepository.ClientDivisionQuery()
        {
            return new ClientDivisionQuery(_context.ClientDivisions, QueryResultFactory);
        }

        IClientShiftQuery ILaborManagementRepository.ClientShiftQuery()
        {
            return new ClientShiftQuery(_context.ClientShifts, QueryResultFactory);
        }

        /// <summary>
        /// Query <see cref="ClockEmployeeExceptionHistory"/> data.
        /// </summary>
        /// <returns></returns>
        IClockEmployeeExceptionHistoryQuery ILaborManagementRepository.ClockEmployeeExceptionHistoryQuery()
        {
            return new ClockEmployeeExceptionHistoryQuery(_context.ClockEmployeeExceptionHistory, QueryResultFactory);
        }

        /// <summary>
        /// Query <see cref="ClockEmployeeApproveDate"/> data.
        /// </summary>
        /// <returns></returns>
        IClockEmployeeApproveDateQuery ILaborManagementRepository.ClockEmployeeApproveDateQuery()
        {
            return new ClockEmployeeApproveDateQuery(_context.ClockEmployeeApproveDates, QueryResultFactory);
        }

        IClockEarningDesHistoryQuery ILaborManagementRepository.ClockEarningDesHistoryQuery()
        {
            return new ClockEarningDesHistoryQuery(_context.ClockEarningDesHistories, QueryResultFactory);
        }

        IClockClientRulesQuery ILaborManagementRepository.ClockClientRulesQuery()
        {
            return new ClockClientRulesQuery(_context.ClockClientRules, QueryResultFactory);
        }

        IClockClientDailyRulesQuery ILaborManagementRepository.ClockClientDailyRulesQuery()
        {
            return new ClockClientDailyRulesQuery(_context.ClockClientDailyRules, QueryResultFactory);
        }

        IClockClientLunchQuery ILaborManagementRepository.ClockClientLunchQuery()
        {
            return new ClockClientLunchQuery(_context.ClockClientLunches, QueryResultFactory);
        }

//        IClockClientLunchSelectedQuery ILaborManagementRepository.ClockClientLunchSelectedQuery()
//        {
//            return new ClockClientLunchSelectedQuery(_context.ClockClientLunchSelectedList, QueryResultFactory);
//        }

        IClockClientLunchPaidOptionQuery ILaborManagementRepository.ClockClientLunchPaidOptionQuery()
        {
            return new ClockClientLunchPaidOptionQuery(_context.ClockClientLunchPaidOption, QueryResultFactory);
        }

        IClockClientLunchPaidOptionRulesQuery ILaborManagementRepository.ClockClientLunchPaidOptionRulesQuery()
        {
            return new ClockClientLunchPaidOptionRulesQuery(_context.ClockClientLunchPaidOptionRulesList, QueryResultFactory);
        }

        IClockClientAddHoursQuery ILaborManagementRepository.ClockClientAddHoursQuery()
        {
            return new ClockClientAddHoursQuery(_context.ClockClientAddHours, QueryResultFactory);
        }

        IClockClientHolidayQuery ILaborManagementRepository.ClockClientHolidayQuery()
        {
            return new ClockClientHolidayQuery(_context.ClockClientHolidays, QueryResultFactory);
        }

        IClockClientHolidayDetailQuery ILaborManagementRepository.ClockClientHolidayDetailQuery()
        {
            return new ClockClientHolidayDetailQuery(_context.ClockClientHolidayDetails, QueryResultFactory);
        }

        IClockClientTimePolicyQuery ILaborManagementRepository.ClockClientTimePolicyQuery()
        {
            return new ClockClientTimePolicyQuery(_context.ClockClientTimePolicies, QueryResultFactory);
        }

        IClockEmployeeAllocateHoursDetailQuery ILaborManagementRepository.ClockEmployeeAllocateHoursDetailQuery()
        {
            return new ClockEmployeeAllocateHoursDetailQuery(_context.ClockEmployeeAllocateHoursDetails, QueryResultFactory);
        }

        IClientAccrualEarningQuery ILaborManagementRepository.ClientAccrualEarningQuery()
        {
            return new ClientAccrualEarningQuery(_context.ClientAccrualEarnings, this.QueryResultFactory);

        }
        IClientAccrualQuery ILaborManagementRepository.ClientAccrualQuery()
        { 
            return new ClientAccrualQuery(_context.ClientAccruals, this.QueryResultFactory);
        }
    //sync-sept-14th
    //public List<ApplicantPostingCategoryDto> GetApplicantPostingCategories(int clientId, int applicantId, bool isAdmin, int userId, bool includePastPostings, int sortBy, bool sortByAscendingOrder)
    //{
    //    var now = DateTime.Now.Date;
    //    return (from category in _context.ApplicantPostingCategories
    //                 from posting in category.ApplicantPosting.Where(x => x.ClientId == clientId).DefaultIfEmpty()
    //                 from aplicant in _context.Applicants.Where(x => x.ApplicantId == applicantId).DefaultIfEmpty()
    //                 from user in _context.Users.Where(x => x.UserId == userId).DefaultIfEmpty()
    //                 where
    //                 category.ClientId == clientId &&
    //                 ((posting.PostingTypeId != 2 && aplicant == null) || (posting.PostingTypeId != 1 && (aplicant != null || user != null)) || isAdmin) &&
    //                 (posting.FilledDate.HasValue || (posting.IsRemoveAfterPostingEnd && posting.PostingEnd <= now))
    //                 select new ApplicantPostingCategoryDto { Name = category.Name, PostingCategoryId = category.PostingCategoryId }).ToList();

    //}
        ITimeOffRequestQuery ILaborManagementRepository.TimeOffRequestQuery()
        {
            return new TimeOffRequestQuery(_context.TimeOffRequests, this.QueryResultFactory);
        }

    #endregion

        #region Company

        #region Company ---> Notes

        IClockClientNotesQuery ILaborManagementRepository.ClockClientNotesQuery()
        {
            return new ClockClientNotesQuery(_context.ClockClientNote, QueryResultFactory);
        }

        #endregion

        #region Company ---> Overtime

        IClockClientOvertimeQuery ILaborManagementRepository.ClockClientOvertimeQuery()
        {
            return new ClockClientOvertimeQuery(_context.ClockClientOvertime, QueryResultFactory);
        }

        IClockClientOvertimeSelectedQuery ILaborManagementRepository.ClockClientOvertimeSelectedQuery()
        {
            return new ClockClientOvertimeSelectedQuery(_context.ClockClientOvertimeSelected, QueryResultFactory);
        }

        IClockOvertimeFrequencyQuery ILaborManagementRepository.ClockOvertimeFrequencyQuery()
        {
            return new ClockOvertimeFrequencyQuery(_context.ClockOvertimeFrequencies, QueryResultFactory);
        }

        #endregion

        #region Company ---> Exceptions

        IClockExceptionQuery ILaborManagementRepository.ClockExceptionQuery()
        {
            return new ClockExceptionQuery(_context.ClockExceptionTypeInformation, QueryResultFactory);
        }

        IClockClientExceptionQuery ILaborManagementRepository.ClockClientExceptionQuery()
        {
            return new ClockClientExceptionQuery(_context.ClockClientExceptions, QueryResultFactory);
        }

        IClockClientExceptionDetailQuery ILaborManagementRepository.ClockClientExceptionDetailQuery()
        {
            return new ClockClientExceptionDetailQuery(_context.ClockClientExceptionDetails, QueryResultFactory);
        }

        #endregion

        #endregion

        #region TimeCardAuthorization
        IEnumerable<GetClockPayrollListByClientIDPayrollRunIDDto> ILaborManagementRepository.GetClockPayrollListByClientIDPayrollRunID(GetClockPayrollListByClientIDPayrollRunIDArgsDto args)
        {
            var sproc = new GetClockPayrollListByClientIDPayrollRunIDSproc(_context.ConnectionString, args);
            return sproc.Execute();
        }

        IEnumerable<GetClockFilterCategoryDto> ILaborManagementRepository.GetClockFilterCategory(GetClockFilterCategoryArgsDto args)
        {
            var sproc = new GetClockFilterCategorySproc(_context.ConnectionString, args);
            return sproc.Execute();
        }

        IEnumerable<GetClockEmployeeApproveHoursSettingsDto> ILaborManagementRepository.GetClockEmployeeApproveHoursSettings(GetClockEmployeeApproveHoursSettingsArgsDto args)
        {
            var sproc = new GetClockEmployeeApproveHoursSettingsSproc(_context.ConnectionString, args);
            return sproc.Execute();
        }

        IEnumerable<GetClockEmployeeApproveHoursOptionsDto> ILaborManagementRepository.GetClockEmployeeApproveHoursOptions(GetClockEmployeeApproveHoursOptionsArgsDto args)
        {
            var sproc = new GetClockEmployeeApproveHoursOptionsSproc(_context.ConnectionString, args);
            return sproc.Execute();
        }

        GetClientJobCostingInfoByClientIDResultsDto ILaborManagementRepository.GetClientJobCostingInfoByClientID(GetClientJobCostingInfoByClientIDArgsDto args)
        {
            var sproc = new GetClientJobCostingInfoByClientIDSproc(_context.ConnectionString, args);
            return sproc.Execute();
        }

        int ILaborManagementRepository.InsertClockEmployeeApproveDate(InsertClockEmployeeApproveDateArgsDto args)
        {
            var sproc = new InsertClockEmployeeApproveDateSproc(_context.ConnectionString, args);
            return sproc.Execute();
        }

        IEnumerable<GetClockEmployeeApproveDateDto> ILaborManagementRepository.GetClockEmployeeApproveDate(GetClockEmployeeApproveDateArgsDto args)
        {
            var sproc = new GetClockEmployeeApproveDateSproc(_context.ConnectionString, args);
            return sproc.Execute();
        }

        public IEnumerable<GetClockEmployeePunchListByDateAndFilterDto> GetClockEmployeePunchListByDateAndFilter(GetClockEmployeePunchListByDateAndFilterArgsDto args)
        {
            var sproc = new GetClockEmployeePunchListByDateAndFilterSproc(_context.ConnectionString, args);
            return sproc.Execute();
        }

        PunchActivitySprocResults ILaborManagementRepository.GetClockEmployeePunchListByDateAndFilterPaginated(ClockEmployeePunchListByDateAndFilterPaginatedCountArgs args)
        {
            return new GetClockEmployeePunchListByDateAndFilterPaginated(_context.ConnectionString, args).Execute();
        }

        EmployeePunchListCountAndResultLengthDto ILaborManagementRepository.GetClockEmployeePunchListByDateAndFilterCount(ClockEmployeePunchListByDateAndFilterPaginatedCountArgs args)
        {
            return new GetClockEmployeePunchListByDateAndFilterPaginatedCount(_context.ConnectionString, args).Execute();
        }

        public IEnumerable<GetClockFilterIdsDto> GetClockFilterIDs(GetClockFilterIdsArgsDto args)
        {
            var sproc = new GetClockFilterIDsSproc(_context.ConnectionString, args);
            return sproc.Execute();
        }

        public GetTimeClockCurrentPeriodDto GetTimeClockCurrentPeriod(GetTimeClockCurrentPeriodArgsDto args)
        {
            var sproc = new GetTimeClockCurrentPeriodSproc(_context.ConnectionString, args);
            return sproc.Execute();
        }

        public IEnumerable<GetClockEmployeeAllocatedHoursDifferenceDto> GetClockEmployeeAllocatedHoursDifference(GetClockEmployeeAllocatedHoursDifferenceArgsDto args)
        {
            var sproc = new GetClockEmployeeAllocatedHoursDifferenceSproc(_context.ConnectionString, args);
            return sproc.Execute();
        }
        #endregion

        #region ClientAccruals
        ILeaveManagementPendingAwardQuery    ILaborManagementRepository.LeaveManagementPendingAwardQuery()     => new LeaveManagementPendingAwardQuery(_context.LeaveManagementPendingAwards,        this.QueryResultFactory);
        IServiceFrequencyQuery               ILaborManagementRepository.ServiceFrequencyQuery()                => new ServiceFrequencyQuery(_context.ServiceFrequencies,                             this.QueryResultFactory);
        IServiceRenewFrequencyQuery          ILaborManagementRepository.ServiceRenewFrequencyQuery()           => new ServiceRenewFrequencyQuery(_context.ServiceRenewFrequencies,                   this.QueryResultFactory);
        IServiceRewardFrequencyQuery         ILaborManagementRepository.ServiceRewardFrequencyQuery()          => new ServiceRewardFrequencyQuery(_context.ServiceRewardFrequencies,                 this.QueryResultFactory);
        IServiceCarryOverFrequencyQuery      ILaborManagementRepository.ServiceCarryOverFrequencyQuery()       => new ServiceCarryOverFrequencyQuery(_context.ServiceCarryOverFrequencies,           this.QueryResultFactory);
        IServiceCarryOverTillFrequencyQuery  ILaborManagementRepository.ServiceCarryOverTillFrequencyQuery()   => new ServiceCarryOverTillFrequencyQuery(_context.ServiceCarryOverTillFrequencies,   this.QueryResultFactory);
        IServiceCarryOverWhenFrequencyQuery  ILaborManagementRepository.ServiceCarryOverWhenFrequencyQuery()   => new ServiceCarryOverWhenFrequencyQuery(_context.ServiceCarryOverWhenFrequencies,   this.QueryResultFactory);
        IServiceReferencePointFrequencyQuery ILaborManagementRepository.ServiceReferencePointFrequencyQuery()  => new ServiceReferencePointFrequencyQuery(_context.ServiceReferencePointFrequencies, this.QueryResultFactory);
        IServiceStartEndFrequencyQuery       ILaborManagementRepository.ServiceStartEndFrequencyQuery()        => new ServiceStartEndFrequencyQuery(_context.ServiceStartEndFrequencies,             this.QueryResultFactory);
        IServicePlanTypeQuery                ILaborManagementRepository.ServicePlanTypeQuery()                 => new ServicePlanTypeQuery(_context.ServicePlanTypes,                                this.QueryResultFactory);
        IServiceTypeQuery                    ILaborManagementRepository.ServiceTypeQuery()                     => new ServiceTypeQuery(_context.ServiceTypes,                                        this.QueryResultFactory);
        IServiceUnitQuery                    ILaborManagementRepository.ServiceUnitQuery()                     => new ServiceUnitQuery(_context.ServiceUnits,                                        this.QueryResultFactory);

        IAccrualBalanceOptionQuery           ILaborManagementRepository.AccrualBalanceOptionQuery()            => new AccrualBalanceOptionQuery(_context.AccrualBalanceOptions,                      this.QueryResultFactory);
        IClientAccrualEmployeeStatusQuery    ILaborManagementRepository.ClientAccrualEmployeeStatusQuery()     => new ClientAccrualEmployeeStatusQuery(_context.ClientAccrualEmployeeStatuses,       this.QueryResultFactory);
        IAccrualCarryOverOptionQuery         ILaborManagementRepository.AccrualCarryOverOptionQuery()          => new AccrualCarryOverOptionQuery(_context.CarryOverOptions,                         this.QueryResultFactory);
        IClientAccrualClearOptionQuery       ILaborManagementRepository.AccrualClearOptionQuery()              => new ClientAccrualClearOptionQuery(_context.AccrualClearOptions,                    this.QueryResultFactory);
        IServiceBeforeAfterQuery             ILaborManagementRepository.ServiceBeforeAfterQuery()              => new ServiceBeforeAfterQuery(_context.ServiceBeforeAfter,                           this.QueryResultFactory);
        IAutoApplyAccrualPolicyQuery         ILaborManagementRepository.AutoApplyAccrualPolicyQuery()          => new AutoApplyAccrualPolicyQuery(_context.AutoApplyAccrualPolicy,                   this.QueryResultFactory);
        #endregion ClientAccruals
    }
}