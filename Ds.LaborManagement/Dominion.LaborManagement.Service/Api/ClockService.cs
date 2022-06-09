using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Dominion.Authentication.Intermediate.Util;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Geofence;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.Misc;
using Dominion.Core.Dto.Payroll;
using Dominion.Core.Dto.SftpUpload.CsvTemplates;
using Dominion.Core.Services.Api;
using Dominion.Core.Services.Api.ClockException;
using Dominion.Core.Services.Api.DataServicesInjectors;
using Dominion.Core.Services.Interfaces;
using Dominion.Core.Services.Internal.Providers;
using Dominion.Core.Services.Mapping;
using Dominion.Core.Services.Security.Authorization;
using Dominion.Domain.Entities.Employee;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.LaborManagement.Dto.Clock.Misc;
using Dominion.LaborManagement.Dto.Department;
using Dominion.LaborManagement.Dto.JobCosting;
using Dominion.LaborManagement.Dto.Scheduling;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.LaborManagement.Dto.Sproc.JobCosting;
using Dominion.LaborManagement.Service.Api.DataServicesInjectors;
using Dominion.LaborManagement.Service.Internal;
using Dominion.LaborManagement.Service.Internal.Providers;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.LaborManagement.Service.Mapping.Clock;
using Dominion.Utility.Constants;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.Mapping;
using Dominion.Utility.OpResult;
using GenericMsg = Dominion.Utility.Msg.Specific.GenericMsg;

namespace Dominion.LaborManagement.Service.Api
{
    public class ClockService : IClockService
    {
        #region Variables and Properties

        private readonly IBusinessApiSession _session;
        private readonly IEmployeePunchProvider _punchProvider;
        private readonly IJobCostingProvider _jobCostingProvider;
        private readonly IServiceAuthorizer _authorizer;
        private readonly IUserProvider _userProvider;
        private readonly ISchedulingProvider _schedulingProvider;
        private readonly ILaborManagementProvider _laborManagementProvider;
        private readonly IDsDataServicesClockClientService _objClockClientService;
        private readonly IDsDataServicesClockEmployee _objClockEmployeeService;
        private readonly IDsDataServicesClockEmployeePunchService _objClockEmployeePunchService;
        private readonly IDsDataServicesClientService _objClientService;
        private readonly IDsDataServicesClockService _dsDataServicesClockService;
        private readonly IDsDataServicesEmployeePointsService _objEmployeePointsService;
        private readonly IDsDataServicesEmployeePunchRequestService _objEmployeePunchRequestService;
        private readonly IDsDataServicesClockCalculateActivityService _objClockCalculateActivityService;
        private readonly IDsDataServicesClockEarningDesHistoryService _objClockEarningDesHistoryService;
        private readonly IDsDataServicesClientJobCostingService _objClientJobCostingService;
        private readonly IDsDataServicesEmployeeLeaveManagementService _objLeaveManagementService;
        private readonly IEmployeeLaborManagementService _employeeLaborManagementService;
        private readonly IGeoService _geoService;
        private readonly IClockEmployeeExceptionService _clockEmployeeExceptionService;
		private readonly IPayrollService _payrollService;

        internal IClockService Self { get; set; }

        #endregion

        #region Constructors and Initializers

        public ClockService(
            IBusinessApiSession session, 
            IEmployeePunchProvider punchProvider, 
            IJobCostingProvider jobCostingProvider, 
            IUserProvider userProvider,
            ISchedulingProvider schedulingProvider,
            ILaborManagementProvider laborManagementProvider,
            IDsDataServicesClockClientService objClockClientService,
            IDsDataServicesClockEmployee objClockEmployeeService,
            IDsDataServicesClockEmployeePunchService objClockEmployeePunchService,
            IDsDataServicesClientService objClientService,
            IDsDataServicesEmployeePointsService objEmployeePointsService,
            IDsDataServicesEmployeePunchRequestService objEmployeePunchRequestService,
            IDsDataServicesClockCalculateActivityService objClockCalculateActivityService,
            IDsDataServicesClockEarningDesHistoryService objClockEarningDesHistoryService,
            IDsDataServicesClientJobCostingService objClientJobCostingService,
            IDsDataServicesEmployeeLeaveManagementService objLeaveManagementService,
            IEmployeeLaborManagementService employeeLaborManagementService,
            IDsDataServicesClockService dsDataServicesClockService,
            IGeoService geoService,
            IClockEmployeeExceptionService clockEmployeeExceptionService,
            IPayrollService payrollService			
        )
        {
            Self = this;

            _session = session;
            _punchProvider = punchProvider;
            _jobCostingProvider = jobCostingProvider;
            _userProvider = userProvider;
            _authorizer = new ServiceAuthorizer(session);
            _schedulingProvider = schedulingProvider;
            _laborManagementProvider = laborManagementProvider;
            _objClockClientService = objClockClientService;
            _objClockEmployeeService = objClockEmployeeService;
            _objClockEmployeePunchService = objClockEmployeePunchService;
            _objClientService = objClientService;
            _objEmployeePointsService = objEmployeePointsService;
            _objEmployeePunchRequestService = objEmployeePunchRequestService;
            _objClockCalculateActivityService = objClockCalculateActivityService;
            _objClockEarningDesHistoryService = objClockEarningDesHistoryService;
            _objClientJobCostingService = objClientJobCostingService;
            _objLeaveManagementService = objLeaveManagementService;
            _employeeLaborManagementService = employeeLaborManagementService;
            _dsDataServicesClockService = dsDataServicesClockService;
            _geoService = geoService;
            _clockEmployeeExceptionService = clockEmployeeExceptionService;
            _payrollService = payrollService;			
        }

        #endregion

        #region Methods and Commands 

        /// <summary>
        /// This replaces Sproc : [dbo].[spGetClockClientRulesByEmployeeID]
        /// Returns ClockClientRules and a few supplemental fields that contains additional information.
        /// If an employeeId is not provided, all results for active employees will be returned.
        /// If updating legacy code, ensure this is the best option.  If only needing a list of clockemployees, 
        /// use GetClockEmployees, etc
        /// </summary>
        /// <param name="employeeId">employee Id</param>
        /// <param name="clientId">client Id</param>
        /// <returns></returns>
        public IOpResult<IEnumerable<ClockClientRulesSummaryDto>> GetClockClientRulesSummary(int? employeeId = null, int? clientId = null)
        {
            return _authorizer.Handle(a =>
            {
                return a.CheckClockAdminOrNormalUser(
                        adminCheck: x => x.CheckClientId(clientId ?? 0),
                        normalCheck: x => x.CheckEmployeeId(employeeId ?? 0))
                    .Then(d => _punchProvider.GetClockClientRulesSummary(new ClockClientRulesMaps.ToClockClientRulesSummaryDto(), employeeId, clientId));
            });
        }

        /// <summary>
        /// returns a ClockClientRules object.  
        /// </summary>
        /// <typeparam name="TMapper">Custom mapping object that allows for implementing specific properties</typeparam>
        /// <param name="mapper"></param>
        /// <param name="employeeId"></param>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public IOpResult<IEnumerable<ClockClientRulesSummaryDto>> GetClockClientRulesSummary<TMapper>(TMapper mapper, int? employeeId = null, int? clientId = null) where TMapper : ExpressionMapper<EmployeePay, ClockClientRulesSummaryDto>
        {
            return _authorizer.Handle(a =>
            {
                return a.CheckClockAdminOrNormalUser(
                        adminCheck: x => x.CheckClientId(clientId ?? 0),
                        normalCheck: x => x.CheckEmployeeId(employeeId ?? 0))
                    .Then(d => _punchProvider.GetClockClientRulesSummary(mapper, employeeId, clientId));
            });
        }

        /// <summary>
        /// Looks at the ClientAccountOptions to determine if the rule for whether the client requires their
        /// employees to select / enter a cost center when submitting a punch.  
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        public IOpResult<int> GetCostCenterRequiredValue(int clientId)
        {
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.User)
                    .CheckClientId(clientId)
                    .Then(d =>
                    {
                        var opResult = new OpResult<int>();

                        var accountOption = _session.UnitOfWork.ClientRepository.GetClient(clientId)
                                   .AccountOptions.FirstOrDefault(
                                   o => o.AccountOption == AccountOption.TimeClock_RequireEmployeeToPickCostCenter);

                        var accountOptionItems = accountOption?.AccountOptionInfo.AccountOptionItems;

                        var accountOptionItem = accountOptionItems?
                                                    .FirstOrDefault(i => i.AccountOptionItemId.ToString().Equals(accountOption?.Value));

                        //See lines 64 - 66 of [spGetClockClientRulesByEmployeeID], if db value is null, 1 is the default value
                        var value = accountOptionItem != null ? accountOptionItem.Value : 1;

                        int returnValue;
                        int.TryParse(value.ToString(), out returnValue);

                        opResult.Data = returnValue;

                        return opResult;
                    });
            });
        }

        IOpResult<ClockEmployeePunchDto> IClockService.GetPunchById(int employeeId, int punchId)
        {
            var result = new OpResult<ClockEmployeePunchDto>();
            var clientId = _session.LoggedInUserInformation.ClientId ?? 0;

            _session.CanPerformAction(ClockActionType.ReadUser).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(clientId).MergeInto(result);

            if (result.HasError) return result;

            return result.TrySetData(() => _session.UnitOfWork.TimeClockRepository
                .GetClockEmployeePunchQuery()
                //.ByClientId(clientId)
                .ByEmployeeId(employeeId)
                .ByClockEmployeePunchId(punchId)
                .ExecuteQueryAs(x => new ClockEmployeePunchDto
                {
                    ClockEmployeePunchId = x.ClockEmployeePunchId
                })
                .FirstOrDefault());
        }

        IOpResult<IEnumerable<ClockEmployeePunchDto>> IClockService.GetPunchesByIdList(int employeeId, int[] punchIdList)
        {
            var result = new OpResult<IEnumerable<ClockEmployeePunchDto>>();
            var clientId = _session.LoggedInUserInformation.ClientId ?? 0;

            _session.CanPerformAction(ClockActionType.ReadUser).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(clientId).MergeInto(result);
            var data = _punchProvider.GetEmployeePunchesByIdList(employeeId, punchIdList).MergeInto(result).Data;

            if (result.HasError) return result;
            return result.SetDataOnSuccess(data);
        }

        public IOpResult<IEnumerable<ClockEmployeePunchDto>> GetEmployeePunches(int employeeId)
        {
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.User)
                    .CheckEmployeeId(employeeId)
                    .Then(d => _punchProvider.GetEmployeePunches(employeeId));
            });
        }

        public IOpResult<bool> HasEmployeeClockActivity(int employeeId) {
            var result = new OpResult<bool>();
            var clientId = _session.LoggedInUserInformation.ClientId ?? 0;

            _session.CanPerformAction(ClockActionType.User).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(clientId).MergeInto(result);

            if (result.HasError) return result;

            var hasBenefits = _session.UnitOfWork.LaborManagementRepository.ClockEmployeeBenefitQuery().ByEmployeeId(employeeId).Result.Any();
            var hasPunches = _session.UnitOfWork.TimeClockRepository.GetClockEmployeePunchQuery().ByEmployeeId(employeeId).Result.Any();

            result.Data = hasPunches || hasBenefits;

            return result;
        }

        public IOpResult<IEnumerable<ClockEmployeePunchDto>> GetEmployeePunches(int employeeId, DateTime startDate,
            DateTime endDate)
        {
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.User)
                    .CheckEmployeeId(employeeId)
                    .Then(d => _punchProvider.GetEmployeePunches(employeeId, startDate, endDate));
            });
        }

        public IOpResult<IEnumerable<ClockEmployeePunchDto>> GetEmployeePunches(int employeeId, DateTime shiftDate)
        {
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.User)
                    .CheckEmployeeId(employeeId)
                    .Then(d => _punchProvider.GetEmployeePunches(employeeId, shiftDate));
            });
        }

        IOpResult<ScheduledHoursWorkedResult> IClockService.GetInputHoursWorked(int clientId, int employeeId, DateTime startDate, DateTime endDate)
        {
            var result = new OpResult<ScheduledHoursWorkedResult>();

            _session.CanPerformAction(ClockActionType.User).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(clientId).MergeInto(result);

            if (result.HasError) return result;

            var schedules = _laborManagementProvider.GetEmployeeSelectedSchedules(employeeId).MergeInto(result).Data;
            var benefitRecords = _laborManagementProvider
                .GetBenefitRecordsByEmployeeDateRange(employeeId, startDate, endDate).MergeInto(result).Data;

            var days = MergeShiftsWorkedIntoWeeklyScheduleSummary(startDate, endDate, null, benefitRecords, schedules.FirstOrDefault());

            var ces = _session.UnitOfWork.LaborManagementRepository
                .ClockEmployeeScheduleQuery()
                .ByClientId(clientId)
                .ByEmployeeId(employeeId)
                .ByEventDateRange(startDate, endDate)
                .ExecuteQueryAs(new ClockEmployeeScheduleMaps.ToClockEmployeeScheduleListDto());

            foreach (var cs in ces)
            {
                var day = days.FirstOrDefault(s => s.Date.Date == cs.EventDate.Date);

                if (day == null) continue;

                day.StartTime = cs.StartTime;
                day.EndTime = cs.StopTime;
                day.StartTime2 = cs.Schedule2StartTime;
                day.EndTime2 = cs.Schedule2StopTime;
                day.StartTime3 = cs.Schedule3StartTime;
                day.EndTime3 = cs.Schedule3StopTime;
            }

            var resultData = new ScheduledHoursWorkedResult
            {
                Days = days,
                HasSchedule = schedules.Any(),
                TotalHoursScheduled = schedules.Any() ? days.Sum(s => s.ScheduledHours) : 40,
                TotalHoursWorked = days.Sum(s => s.HoursWorked)
            };

            return result.SetDataOnSuccess(resultData);
        }

        IOpResult<ScheduledHoursWorkedResult> IClockService.GetEmployeeWeeklyHoursWorked(int clientId, int employeeId, DateTime startDate, DateTime endDate)
        {
            var result = new OpResult<ScheduledHoursWorkedResult>();

            _session.CanPerformAction(ClockActionType.User).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(clientId).MergeInto(result);

            if (result.HasError) return result;

            var clockSetup = _employeeLaborManagementService.GetClockEmployeeSetup(employeeId).MergeInto(result).Data;
            var hasSchedule = clockSetup.SelectedSchedules.Any(s => s.IsActive);
            var shiftsWorked = _schedulingProvider.GetClockEmployeeSchedules(startDate, endDate, employeeId).MergeInto(result).Data;
            var rawPunchInfo = _session.UnitOfWork.LaborManagementRepository.ClockEarningDesHistoryQuery()
                .ByEmployee(employeeId)
                .ByEventDateRange(startDate, endDate)
                .ExecuteQueryAs(e => new OnShiftPunchesLayout()
                {
                    punchDate = e.EventDate,
                    inPunch = e.ClockEmployeePunchIn.ModifiedPunch,
                    outPunch = e.ClockEmployeePunchOut.ModifiedPunch,
                    totalHoursWorked = e.Hours
                });

            var sched = clockSetup.SelectedSchedules.FirstOrDefault(s => s.IsActive)?.ScheduleDetails;
            var days = MergeShiftsWorkedIntoWeeklyScheduleSummary(startDate, endDate, shiftsWorked, rawPunchInfo, sched);

            var ces = _session.UnitOfWork.LaborManagementRepository
                .ClockEmployeeScheduleQuery()
                .ByClientId(clientId)
                .ByEmployeeId(employeeId)
                .ByEventDateRange(startDate, endDate)
                .ExecuteQueryAs(new ClockEmployeeScheduleMaps.ToClockEmployeeScheduleListDto());

            foreach (var cs in ces)
            {
                var day = days.FirstOrDefault(s => s.Date.Date == cs.EventDate.Date);

                if (day == null) continue;

                day.StartTime = cs.StartTime;
                day.EndTime = cs.StopTime;
                day.StartTime2 = cs.Schedule2StartTime;
                day.EndTime2 = cs.Schedule2StopTime;
                day.StartTime3 = cs.Schedule3StartTime;
                day.EndTime3 = cs.Schedule3StopTime;
            }

            var resultData = new ScheduledHoursWorkedResult
            {
                Days = days,
                HasSchedule = hasSchedule,
                TotalHoursScheduled = hasSchedule ? days.Sum(d => d.ScheduledHours) : 40,
                TotalHoursWorked = days.Sum(d => d.HoursWorked)
            };

            return result.SetDataOnSuccess(resultData);
        }

        public IOpResult<IEnumerable<ClockEmployeeOvertimeInformationDto>> GetOvertimeInformationByEmployeeIds(int clientId, DateTime startDate, DateTime endDate, int[] employeeIds)
        {
            var result = new OpResult<IEnumerable<ClockEmployeeOvertimeInformationDto>>();

            var overTimeEarning = _session.UnitOfWork.PayrollRepository.QueryClientEarnings()
                .ByClientId(clientId)
                .ByEarningCategoryId(ClientEarningCategory.Overtime)
                .ExecuteQuery().ToList();

            var ClientEarningIDs = overTimeEarning.Select(x => x.ClientEarningId).ToList();



            var employees = _session.UnitOfWork.LaborManagementRepository.ClockEarningDesHistoryQuery()
                .ByEmployeeIds(employeeIds)
                .ByEventDateRange(startDate, endDate)
                .ExecuteQueryAs(e => new ClockEmployeeOvertimeInformationDto
                {
                    employeeName = e.Employee.LastName + ", " + e.Employee.FirstName,
                    employeeId = e.EmployeeId,
                    employeeSupervisor = e.Employee.DirectSupervisor.LastName.Length > 0 ? e.Employee.DirectSupervisor.LastName + ", " + e.Employee.DirectSupervisor.FirstName : "",
                    employeeDepartment = e.Employee.Department.Name,
                    clientEarningID = e.ClientEarningId,
                    Hours = e.Hours,
                    clockedInPunch = e.ClockEmployeePunchIn.RawPunch,
                    clockedOutPunch = e.ClockEmployeePunchOut.RawPunch
                });

            var otEmployees = employees.Where(x => ClientEarningIDs.Contains(x.clientEarningID)).ToList();
            var otEmployeeList = new List<ClockEmployeeOvertimeInformationDto>();
            var uniqueEmployees = new List<int>();

            foreach (var employee in otEmployees)
            {
                if (!uniqueEmployees.Contains(employee.employeeId))
                {
                    uniqueEmployees.Add(employee.employeeId);
                    otEmployeeList.Add(employee);
                }
                else
                {
                    int index = otEmployeeList.FindIndex(a => a.employeeId == employee.employeeId);
                    otEmployeeList[index].Hours += employee.Hours;
                }
            }

            result.Data = otEmployeeList;
            return result;
        }

        public IOpResult<CheckPunchTypeResultDto> GetNextPunchTypeDetail(int employeeId, DateTime? punchTime,
            string ipAddress, bool includeEmployeeClockConfig, bool isHwClockPunch)
        {
            var result = new OpResult<CheckPunchTypeResultDto>();

            _session.CanPerformAction(ClockActionType.User).MergeInto(result);
            if (result.HasError)
                return result;

            var data = _punchProvider.GetNextPunchTypeDetail(employeeId, punchTime, ipAddress, includeEmployeeClockConfig, isHwClockPunch).MergeInto(result).Data;

            return result.SetDataOnSuccess(data);
        }

        IOpResult<InputHoursPunchRequestResult> IClockService.ProcessInputHoursPunch(ClockEmployeeBenefitDto request, string ipAddess)
        {
            var result = new OpResult<InputHoursPunchRequestResult>();

            _session.CanPerformAction(ClockActionType.User).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(request.ClientId).MergeInto(result);

            if (result.HasError) return result;

            var canPunch = _punchProvider.CanEmployeePunchFromIp(request.EmployeeId, ipAddess).Data?.CanPunch ?? false;

            if (!canPunch) return result.SetToFail(() => new GenericMsg("Cannot punch from this IP address."));

            _punchProvider.ProcessInputHoursPunchRequest(request).MergeAll(result);

            _session.UnitOfWork.Commit().MergeInto(result);

            if (result.HasError) return result;

            _dsDataServicesClockService.CalculateWorkedHours(request.ClientId, request.EmployeeId, (DateTime)request.EventDate);

            return result;
        }

        public IOpResult<RealTimePunchResultDto> ProcessRealTimePunch(RealTimePunchRequest request, string ipAddress)
        {
            return _authorizer.Handle(a =>
            {
                return a.CheckClockAdminOrNormalUser(
                    normalCheck: x => x.CheckCanPunchFromIp(_punchProvider, request.EmployeeId, ipAddress)
                        .Then(d => default(object))
                ).Then(d => _punchProvider.ProcessRealTimePunch(request))
                 .Then(d => d.ToDto());
            });
        }

        public IOpResult<RealTimePunchResultDto> ProcessRealTimePunchFromClock(RealTimePunchRequest request)
        {
            return _authorizer.Handle(a =>
            {
                return _punchProvider.ProcessRealTimePunch(request)
                    .Then(d => d.ToDto());
            });
        }

        public IOpResult<IEnumerable<ClockEmployeePayPeriodEndedDto>> ClockEmployeePayPeriodEndedSproc(int clientId, int employeeId)
        {
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.Administrator)
                    .CheckClientId(clientId)
                    .Then(d => _session.UnitOfWork.TimeClockRepository.ClockEmployeePayPeriodEndedSproc(clientId, employeeId));
            });
        }

        public IOpResult<IEnumerable<ClientDepartmentDto>> GetAvailableDepartments(int clientId)
        {
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.User)
                    .CheckClientId(clientId)
                    .Then(d =>
                        _session.UnitOfWork.LaborManagementRepository.ClientDepartmentQuery()
                            .ByClientId(clientId)
                            .ByIsActive(true)
                            .ExecuteQueryAs(new ClientDepartmentMaps.DefaultClientDepartmentMap()));
            });
        }

        IOpResult<IEnumerable<ClientDepartmentDto>> IClockService.GetEmployeeAvailableDepartments(int clientId, int employeeId)
        {
            var result = new OpResult<IEnumerable<ClientDepartmentDto>>();

            _session.CanPerformAction(ClockActionType.User).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(clientId).MergeInto(result);

            if (result.HasError) return result;

            var userTypeId = _session.UnitOfWork.UserRepository
                .GetUser(_session.LoggedInUserInformation.UserId)?
                .UserTypeId ?? null;

            if(userTypeId == null)
            {
                return result.SetToFail(() => new GenericMsg("Could not find user."));
            }

            return result.TrySetData(() => _session.UnitOfWork.LaborManagementRepository
                .ClientDepartmentQuery()
                .ByClientId(clientId)
                .ByUserTypeAndEmployeeId((int)userTypeId, employeeId)
                .ExecuteQueryAs(new ClientDepartmentMaps.DefaultClientDepartmentMap())
                .OrderBy(x => x.Name).ThenBy(x => x.Code));
        }

        IOpResult<CanEmployeePunchDto> IClockService.CanEmployeePunchFromIp(int employeeId, string ipAddress)
        {
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.User)
                    .CheckEmployeeId(employeeId)
                    .Then(x => _punchProvider.CanEmployeePunchFromIp(employeeId, ipAddress));
            });
        }

        public IOpResult<IEnumerable<ClientJobCostingDto>> GetClientJobCostingList(int clientId)
        {
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.User)
                    .CheckClientId(clientId)
                    .Then(d => _jobCostingProvider.GetClientJobCostingList(clientId));
            });
        }

        public IOpResult<IEnumerable<ClientJobCostingAssignmentSprocDto>> GetEmployeeJobCostingAssignmentList(
            int clientId,
            int employeeId,
            int clientJobCostingId,
            string commaSeparatedParentJobCostingIds,
            string commaSeparatedParentJobCostingAssignmentIds,
            string searchText)
        {
            return _authorizer.Handle(a =>
            {
                return a.CheckClockAdminOrNormalUser(
                        adminCheck: x => x.CheckClientId(clientId),
                        normalCheck: x => x.CheckEmployeeId(employeeId))
                    .Then(d => _jobCostingProvider.GetEmployeeJobCostingAssignmentList(
                        clientId,
                        employeeId,
                        clientJobCostingId,
                        commaSeparatedParentJobCostingIds,
                        commaSeparatedParentJobCostingAssignmentIds,
                        searchText));
            });
        }

        public IOpResult<IEnumerable<ClientJobCostingListDto>> GetEmployeeJobCostingAssignments(int clientId, int employeeId, AssociatedClientJobCostingDto[] jobCostings)
        {
            return _authorizer.Handle(a =>
            {
                return a.CheckClockAdminOrNormalUser(
                        adminCheck: x => x.CheckClientId(clientId))
                    .Then(d => _jobCostingProvider.GetEmployeeJobCostingAssignments(clientId, employeeId, jobCostings));
            });
        }

        public IOpResult<ClientJobCostingListDto> GetEmployeeJobCostingAssignmentsLazy(int clientId, int employeeId, ClientJobCostingCustomDto jobCostings)
        {
            var result = new OpResult<ClientJobCostingListDto>();
            var userId = _session.LoggedInUserInformation.UserId;

            var pjc = string.Empty;
            var ajc = string.Empty;
            var cjc = new ClientJobCostingListDto();

            foreach(int id in jobCostings.ClientJobCosting.ParentJobCostingIds)
            {
                var comma = string.IsNullOrEmpty(pjc) ? "" : ",";

                pjc = pjc + comma + id;
                ajc = ajc + comma;

                //var parentJobCost = jobCostingList2.Where(x => x.ClientJobCostingId == id).FirstOrDefault();
                var parentJobCost = jobCostings.ClientJobCostingList.Where(x => x.ClientJobCostingId == id).FirstOrDefault();
                if (parentJobCost != null && parentJobCost.jobCostingAssignmentSelection != null)
                {
                    ajc = ajc + parentJobCost.jobCostingAssignmentSelection.ClientJobCostingAssignmentId;
                }
                else
                {
                    ajc = ajc + "0";
                }
                
            }
            
            var assignments = _objClientJobCostingService.GetSearchableEmployeeJobCostingAssignmentList(clientId, employeeId, jobCostings.ClientJobCosting.ClientJobCostingId, true, pjc, ajc, userId, null);

            if (assignments.Tables[0].Rows.Count > 0)
            {
                
                cjc.ClientJobCostingId = jobCostings.ClientJobCosting.ClientJobCostingId;
                List<ClientJobCostingAssignmentSprocDto> selections = new List<ClientJobCostingAssignmentSprocDto>();

                foreach(DataRow dr in assignments.Tables[0].Rows)
                {
                    var s = new ClientJobCostingAssignmentSprocDto();
                    s.ClientJobCostingAssignmentId = Int32.Parse(dr["ID"].ToString());
                    s.Description                  = dr["Description"].ToString();
                    s.Code                         = dr["Code"].ToString();
                    selections.Add(s);
                }
                cjc.AvailableAssignments = selections.ToArray();
            }
            result.Data = cjc;
            return result;
        }

        /// <summary>
        /// Checks if the given Employee PIN is available and valid
        /// </summary>
        /// <param name="employeeId">The ID of the employee to check PIN for.</param>
        /// <param name="employeePin">The 4 digit number to check availability and validity for.</param>
        /// <param name="clientId">The ID of the client</param>
        /// <returns></returns>
        IOpResult<bool> IClockService.CanAssignEmployeePinToEmployee(int employeeId, string employeePin, int clientId)
        {
            return _authorizer.Handle(a =>
            {
                return a.CheckActionType(ClockActionType.ClockEmployeeAdministrator).Then(
                    e =>
                    {
                        var opResult = new OpResult<bool>();
                        
                        int numericPin;
                        if (!int.TryParse(employeePin, out numericPin))
                        {
                            opResult.AddMessage(new GenericMsg("PIN can only contain numbers."));
                            return opResult.SetDataOnSuccess(false);
                        }
                        if (employeePin.Length != 4)
                        {
                            opResult.AddMessage(new GenericMsg("PIN must be 4 digits."));
                            return opResult.SetDataOnSuccess(false);
                        }
                        
                        var data = _session.UnitOfWork.TimeClockRepository.GetClockEmployeeQuery()
                        .ByClientId(clientId, includeAllClientsInOrganization: true)
                        .ByEmployeePin(employeePin)
                        .ExecuteQueryAs(x => new
                        {
                            x.EmployeeId,
                            x.EmployeePin
                        })
                        .ToList();

                        if (data.Any(x => x.EmployeeId != employeeId))
                        {
                            opResult.AddMessage(new GenericMsg("PIN is already in use on another employee."));
                            return opResult.SetDataOnSuccess(false);
                        }
                        
                        return opResult.SetDataOnSuccess(true);
                    });
            });
        }

        IOpResult<PunchTypeItemResult> IClockService.GetPunchTypeItems(int employeeId)
        {
            var result = new OpResult<PunchTypeItemResult>();
            _session.CanPerformAction(ClockActionType.User).MergeInto(result);
            if (result.HasError)
                return result;

            var punchTypeResult = _punchProvider.GetPunchTypeItems(employeeId).MergeInto(result).Data;
            result.CheckForNull(punchTypeResult);

            if (result.HasError)
                return result;

            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(punchTypeResult.ClientId).MergeInto(result);

            return result.SetDataOnSuccess(punchTypeResult);
        }

        IOpResult<IEnumerable<ClockTimeCardDto>> IClockService.GetEmployeeWeeklyHoursWorkedTimeCard(int clientId, int employeeId)
        {
            int payrollId = -1;
            DateTime datWeeklyActivity = DateTime.MinValue;
            DateTime datEndingPayPeriod = DateTime.MinValue;

            return Self.GetEmployeeHoursWorkedTimeCard(clientId, employeeId, payrollId, datWeeklyActivity, datEndingPayPeriod);
        }

        IOpResult<IEnumerable<ClockTimeCardDto>> IClockService.GetEmployeeHoursWorkedTimeCard(int clientId, int employeeId, int payrollId, DateTime datWeeklyActivity , DateTime datEndingPayPeriod )
        {
            var result = new OpResult<IEnumerable<ClockTimeCardDto>>();
            var user = _session.UnitOfWork.UserRepository.GetUserByEmployeeId(employeeId);
            var userId = (user!=null) ? user.UserId : _session.LoggedInUserInformation.UserId;

            _session.CanPerformAction(ClockActionType.User).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(clientId).MergeInto(result);

            if (result.HasError) return result;

            var employeePointsView = new DataView();
            var employeeApproveDateView = new DataView();
            var requestPunchDataView = new DataView();
            var benefitsView = new DataView();
            var exceptionsView = new DataView();
            var employeePunchesView = new DataView();
            var employeeScheduleView = new DataView();
            var clientScheduleView = new DataView();

            var datStartTime = new DateTime();
            var datStopTime = new DateTime();

            var schedule = new DayScheduleDto();

            var clockClientScheduleId = 0;
            var clockEmployeeScheduleId = 0;
            var clockClientTimePolicyId = 0;

            var weeklyTotalCount = 0;

            bool isNewDate = false;

            var counter = -1;

            List<ClockTimeCardDto> clockTimeCards = new List<ClockTimeCardDto>();

            #region getClientOptions

            var showHoursInHundreths = false;
            int typePremiumHours = 0;
            int approvalOption = CommonConstants.NO_VALUE_SELECTED;

            var clientOptionData = _objClientService.GetClientOptionByClientIDOptionControlID(clientId, 38);

            if (clientOptionData.Tables[0].Rows.Count > 0)
                showHoursInHundreths = (DBNull.Value.Equals(clientOptionData.Tables[0].Rows[0]["Value"])) ? false
                    : (Int32.Parse(clientOptionData.Tables[0].Rows[0]["Value"].ToString()) == 0) ? false : true;

            clientOptionData = _objClientService.GetClientOptionByClientIDOptionControlID(clientId, 54);
            if (clientOptionData.Tables[0].Rows.Count > 0)
            {
                typePremiumHours = (DBNull.Value.Equals(clientOptionData.Tables[0].Rows[0]["ClientOptionItemValue"])
                    ? 0 : Int32.Parse(clientOptionData.Tables[0].Rows[0]["ClientOptionItemValue"].ToString()));

            }
            clientOptionData = _objClientService.GetClientOptionByClientIDOptionControlID(clientId, 11);
            if (clientOptionData.Tables[0].Rows.Count > 0)
            {
                approvalOption = (DBNull.Value.Equals(clientOptionData.Tables[0].Rows[0]["ClientOptionItemValue"])
                    ? 0 : Int32.Parse(clientOptionData.Tables[0].Rows[0]["ClientOptionItemValue"].ToString()));

            }
            var controlItem = _session.UnitOfWork.ClientAccountFeatureRepository.ClientOptionQuery()
                .ByClientId(clientId)
                .ByOption(AccountOption.TimeClock_PunchesToShowToEmployees)
                .ExecuteQueryAs(x => new ClientAccountOptionDto
                {
                    ClientAccountOptionId = x.ClientAccountOptionId,
                    Value = x.Value,

                }).FirstOrDefault();

            var ci = _session.UnitOfWork.ClientAccountFeatureRepository.AccountOptionItemQuery()
                .ByClientOptionItemId(Int32.Parse(controlItem?.Value ?? "0"))
                .ExecuteQueryAs(x => new Core.Dto.Client.AccountOptionItemDto
                {
                    Value = x.Value
                }).FirstOrDefault();

            var punchesToShow = ci.Value;

            #endregion

            var rules = _objClockClientService.GetClockClientRulesByEmployeeId(0, employeeId);

            //int applyHoursOption = Int32.Parse(rules.Tables[0].Rows[0]["ApplyHoursOption"].ToString());

            if (rules.Tables[0].Rows.Count == 0)
            {
                result.Data = clockTimeCards;
                return result;
            }

            var weeklyStartingDay = rules.Tables[0].Rows[0]["WeeklyStartingDayOfWeekID"].ToString();
            DateTime datFirstDayOfWeek, datLastDayOfWeek, datLastDayOfPeriod;            

            if (payrollId > 0) {
                // Payroll Pay Period
                PayrollDto dsData = _payrollService.GetById(payrollId).MergeInto(result).Data;
                if (dsData == null) 
				{
                    return result.SetToFail("The selected Payroll does not exists.");
                }
                else
                {
                    datFirstDayOfWeek = dsData.PeriodStart;
                    datLastDayOfPeriod = dsData.PeriodEnded;
                }
            }
            else if(datWeeklyActivity != DateTime.MinValue && payrollId == -1)
            {
                // Custom date range
                datLastDayOfPeriod = datEndingPayPeriod;
                datFirstDayOfWeek = datWeeklyActivity;
            }
            else
            {
                // Current week
                var weeklySchedule = GetFirstAndLastDayOfWeek(Int32.Parse(weeklyStartingDay));
                datFirstDayOfWeek = weeklySchedule.FirstDayOfWeek;
                datLastDayOfPeriod = weeklySchedule.LastDayOfWeek;

                //var punchdetail = GetNextPunchTypeDetail(employeeId, (DateTime.Now.Date + new TimeSpan(23, 59, 59)), default(string), false, false).MergeInto(result).Data;
                //datLastDayOfPeriod = punchdetail.PayPeriodEnded.PeriodEnded.Value;
            }

            datLastDayOfWeek = GetDateOfLastDayofWeek(datFirstDayOfWeek, Int32.Parse(weeklyStartingDay));

            int punchOption = (DBNull.Value.Equals(rules.Tables[0].Rows[0]["PunchOption"])) ? 1 : Int32.Parse(rules.Tables[0].Rows[0]["PunchOption"].ToString());

            var applyHoursOption = Int32.Parse(rules.Tables[0].Rows[0]["ApplyHoursOption"].ToString());
            var daysToDispaly = Int32.Parse(rules.Tables[0].Rows[0]["NumberOfDaysToShow"].ToString());

            var dayAmount = datLastDayOfPeriod.Subtract(datFirstDayOfWeek).TotalDays;
            var label = "Weekly Activity For " + datFirstDayOfWeek.ToShortDateString();


            var employeePunches = GetEmployeePunches(employeeId, datFirstDayOfWeek, datLastDayOfPeriod.AddHours(23).AddMinutes(59).AddSeconds(59));
            benefitsView.Table = _objClockEmployeeService.GetClockEmployeeBenefitListByDate(clientId, employeeId, datFirstDayOfWeek.ToString(), datLastDayOfPeriod.ToString(), 2).Tables[0];
            exceptionsView.Table = _objClockEmployeeService.GetClockEmployeeExceptionHistoryByEmployeeID(clientId, employeeId, datFirstDayOfWeek.ToString(), datLastDayOfPeriod.ToString()).Tables[0];
            employeePunchesView.Table = _objClockEmployeePunchService.GetClockEmployeePunchListByDate(0, 0, employeeId, datFirstDayOfWeek.ToString(), datLastDayOfPeriod.ToString()).Tables[0];
            employeeScheduleView.Table = _objClockEmployeeService.GetClockEmployeeScheduleListByDate(clientId, employeeId, datFirstDayOfWeek.ToString(), datLastDayOfPeriod.ToString()).Tables[0];
            clientScheduleView.Table = _objClockClientService.GetClockClientScheduleListByEmployeeIDAndDate(clientId, employeeId, datFirstDayOfWeek, datLastDayOfWeek).Tables[0];


            if (_objClientService.GetClientFeaturesByClientIDFeatureOptionID(clientId, 16).Tables[0].Rows.Count > 0)
            {
                var tempTable = _objEmployeePointsService.GetEmployeePointListByDate(clientId, employeeId, datFirstDayOfWeek, datLastDayOfPeriod).Tables[0];
                if (tempTable.Rows.Count > 0)
                {
                    employeePointsView.Table = tempTable;
                    employeePointsView.Sort = "DateOccured,ClockExceptionID,ClockEmployeeExceptionHistoryID";
                }
            }

            if (_objClockClientService.GetClockClientNoteCount(clientId) > 0)
            {
                var tempTable = _objClockEmployeePunchService.GetClockEmployeeApproveDate(clientId, userId, employeeId, datFirstDayOfWeek, datLastDayOfPeriod, false).Tables[0];
                employeeApproveDateView.Table = tempTable;
            }

            requestPunchDataView.Table = _objEmployeePunchRequestService.GetClockEmployeePunchRequestListByDate(clientId, userId, employeeId, datFirstDayOfWeek.ToString(), datLastDayOfPeriod.ToString()).Tables[0];

            DateTime datCurrentFirstDayOfWeek;
            // time ot build
            for (int i = 0; i <= dayAmount; i++)
            {
                var timeCard = new ClockTimeCardDto();
                var incrementalDate = datFirstDayOfWeek.AddDays(i);

                if (incrementalDate > datLastDayOfWeek)
                {
                    datLastDayOfWeek = datLastDayOfWeek.AddDays(7);
                    clientScheduleView.Table = _objClockClientService.GetClockClientScheduleListByEmployeeIDAndDate(clientId, employeeId, datFirstDayOfWeek, datLastDayOfWeek).Tables[0];
                }

                datCurrentFirstDayOfWeek = firstDayOfTheWeekHelper(incrementalDate, Int32.Parse(weeklyStartingDay));
                isNewDate = true;

                timeCard.Day = incrementalDate.DayOfWeek.ToString();
                timeCard.Date = incrementalDate.ToShortDateString();
                timeCard.Notes = "";
                timeCard.RowTypeId = 1;                

                var dsClock = _objClockClientService.GetClockClientRulesByEmployeeId(0, employeeId);

                _objClockCalculateActivityService.GetscheduleCSharp(clientId, employeeId, ref datStartTime, ref datStopTime,
                    ref clockClientScheduleId, ref clockEmployeeScheduleId,
                    incrementalDate, clientScheduleView, employeeScheduleView, benefitsView, false, ref schedule, ref dsClock);

                timeCard.DayScheduleDto = schedule;

                exceptionsView.RowFilter = "EmployeeID=" + employeeId + " AND EventDateString=" + "'" + incrementalDate.ToString("MM/dd/yyyy") + "'" + " AND ClockExceptionID=9";
                employeePunchesView.RowFilter = "ShiftDateString = '" + incrementalDate.ToString("MM/dd/yyyy") + "'";
                requestPunchDataView.RowFilter = "ShiftDateString = '" + incrementalDate.ToString("MM/dd/yyyy") + "'";

                var totalPunchCounter = 0;

                if (employeePunchesView.Count > 0)
                {
                    counter = 0;
                    totalPunchCounter = 0;

                    foreach (DataRowView rv in employeePunchesView)
                    {
                        DateTime modified = DateTime.Parse(rv["RawPunch"].ToString());
                        string t = modified.ToShortTimeString();

                        modified = DateTime.Parse(rv["ModifiedPunch"].ToString());

                        if (punchesToShow == 1)
                            t = modified.ToShortTimeString();

                        // sub row
                        if (counter == 4)
                        {
                            CreateDailyPunchTotals(ref timeCard, exceptionsView, employeePointsView, incrementalDate, employeeId, showHoursInHundreths, isNewDate);
                            clockTimeCards.Add(timeCard);
                            timeCard = new ClockTimeCardDto();
                            timeCard.RowTypeId = 2;
                            //to do assign type as sub header or some shit
                            counter = 0;
                            isNewDate = false;
                        }

                        counter = counter + 1;
                        totalPunchCounter = totalPunchCounter + 1;

                        switch (counter)
                        {
                            case 1:
                                timeCard.In = t;
                                timeCard.InDateTime = modified;
                                timeCard.InEmployeePunchId = rv["ClockEmployeePunchID"].ToString();
                                timeCard.InClockClientLunchId = rv["ClockClientLunchID"].ToString();
                                timeCard.InTimeZoneId = rv["TimeZoneID"].ToString();
                                break;
                            case 2:
                                timeCard.Out = t;
                                timeCard.OutDateTime = modified;
                                timeCard.OutEmployeePunchId = rv["ClockEmployeePunchID"].ToString();
                                timeCard.OutClockClientLunchId = rv["ClockClientLunchID"].ToString();
                                timeCard.OutTimeZoneId = rv["TimeZoneID"].ToString();
                                break;
                            case 3:
                                timeCard.In2 = t;
                                timeCard.In2DateTime = modified;
                                timeCard.In2EmployeePunchId = rv["ClockEmployeePunchID"].ToString();
                                timeCard.In2ClockClientLunchId = rv["ClockClientLunchID"].ToString();
                                timeCard.In2TimeZoneId = rv["TimeZoneID"].ToString();
                                break;
                            case 4:
                                timeCard.Out2 = t;
                                timeCard.Out2DateTime = modified;
                                timeCard.Out2EmployeePunchId = rv["ClockEmployeePunchID"].ToString();
                                timeCard.Out2ClockClientLunchId = rv["ClockClientLunchID"].ToString();
                                timeCard.Out2TimeZoneId = rv["TimeZoneID"].ToString();
                                break;
                            default:
                                break;
                        }

                        timeCard.HasRequestedPunch = false;

                        if (timeCard.Notes == null)
                            timeCard.Notes = "";
                        if (timeCard.Exceptions == null)
                            timeCard.Exceptions = "";

                        if (requestPunchDataView.Count > 0 && (counter == 1 || counter == 3) && totalPunchCounter == employeePunchesView.Count)
                        {
                            SetRequestedPunchData(employeePunchesView.Count, employeeId, requestPunchDataView[0], ref timeCard);
                            timeCard.HasRequestedPunch = true;
                        }

                        if (!DBNull.Value.Equals(rv["EmployeeComment"]))
                            if (rv["EmployeeComment"].ToString() != "")
                                timeCard.Notes += ((timeCard.Notes == "") ? "" : ", ") + rv["EmployeeComment"].ToString();

                        if (timeCard.HasRequestedPunch)
                            if (employeePunchesView.Count == counter)
                                if (!DBNull.Value.Equals(requestPunchDataView[0]["EmployeeComment"]))
                                    if (((string)requestPunchDataView[0]["EmployeeComment"]) != "")
                                        timeCard.Notes += ((timeCard.Notes == "") ? "" : ", ") + (string)requestPunchDataView[0]["EmployeeComment"];


                    }   // end for punches

                    if (exceptionsView.Count > 0)
                    {
                        if ((timeCard.Out == null || timeCard.Out == "") && timeCard.InDateTime.Date.Ticks < DateTime.Now.Date.Ticks)
                        {
                            timeCard.Out = "Missing";
                            timeCard.OutIsMissing = true;
                            timeCard.OutUrl = GetModalUrl(counter, employeeId, incrementalDate.ToShortDateString(), 0);
                            timeCard.OutHasUrl = true;
                        }
                        if (!(timeCard.In2 == null || timeCard.In2 == "") && (timeCard.Out2 == null || timeCard.Out2 == "") && timeCard.In2DateTime.Date.Ticks < DateTime.Now.Date.Ticks)
                        {
                            timeCard.Out2 = "Missing";
                            timeCard.Out2IsMissing = true;
                            timeCard.Out2Url = "";
                            timeCard.Out2Url = GetModalUrl(counter, employeeId, incrementalDate.ToShortDateString(), 0);
                            timeCard.Out2HasUrl = true;
                        }

                        foreach (DataRow dr in exceptionsView.Table.Rows)
                        {
                            var id = dr["ClockEmployeePunchID"].ToString();

                            if (id == timeCard.InEmployeePunchId)
                                timeCard.InHasException = true;
                            if (id == timeCard.In2EmployeePunchId)
                                timeCard.In2HasException = true;
                            if (id == timeCard.OutEmployeePunchId)
                                timeCard.OutHasException = true;
                            if (id == timeCard.Out2EmployeePunchId)
                                timeCard.Out2HasException = true;

                        }


                    }

                }   // end if punches

                // check for notes
                if (employeeApproveDateView != null)
                {
                    employeeApproveDateView.RowFilter = "EmployeeID=" + employeeId + " and EventDate = '" + incrementalDate.ToString("MM/dd/yyyy") + "'";
                    if (employeeApproveDateView.Count > 0 && (string)employeeApproveDateView[0]["Note"] != "")
                        timeCard.Notes += ((timeCard.Notes == "") ? "" : ", ") + (string)employeeApproveDateView[0]["Note"];
                }

                CreateDailyPunchTotals(ref timeCard, exceptionsView, employeePointsView, incrementalDate, employeeId, showHoursInHundreths, isNewDate);
                clockTimeCards.Add(timeCard);
                CreateDailyTotalTimeCard(ref clockTimeCards, incrementalDate, incrementalDate, employeeId, clientId, userId, punchOption, "", 0, showHoursInHundreths, "", typePremiumHours);
                CreateUnApprovedBenefitsTimeCard(ref clockTimeCards, showHoursInHundreths, userId, employeeId, clientId, incrementalDate);

                if ((incrementalDate.AddDays(1).Date.Subtract(datCurrentFirstDayOfWeek.Date).Days >= 7) || ((i == dayAmount) && weeklyTotalCount >= 1))
                {

                    CreateDailyTotalTimeCard(ref clockTimeCards, (datCurrentFirstDayOfWeek < datFirstDayOfWeek ? datFirstDayOfWeek : datCurrentFirstDayOfWeek), incrementalDate, employeeId, clientId, userId, punchOption, "Weekly Totals", 0, showHoursInHundreths, "", typePremiumHours);
                    weeklyTotalCount += 1;
                }

            }   // end of for day amount

            CreateDailyTotalTimeCard(ref clockTimeCards, datFirstDayOfWeek, datLastDayOfPeriod, employeeId, clientId, userId, punchOption, "Total Earnings", weeklyTotalCount, showHoursInHundreths, "", typePremiumHours);
            result.Data = clockTimeCards;
            return result;
        }

        public IOpResult<RealTimePunchResultDto> ProcessRealTimePunchAttempt(ClockEmployeePunchAttemptDto request)
        {
            var result = new OpResult<RealTimePunchResultDto>();

            // end closed payroll check
            return _punchProvider.ProcessRealTimePunchAttempt(request);
        }

        public IOpResult<RealTimePunchPairResultDto> ProcessRealTimePunchPair(RealTimePunchPairRequest request, string ipAddress)
        {
            return _authorizer.Handle(a =>
            {
                return a.CheckClockAdminOrNormalUser(
                    normalCheck: x => x.CheckEmployeeId(request.First.EmployeeId)
                        .CheckCanPunchFromIp(_punchProvider, request.First.EmployeeId, ipAddress)
                        .Then(d => default(object))
                ).Then(d => _punchProvider.ProcessRealTimePunchPair(request.First, request.Second))
                 .Then(d => d.ToDto());
            });
        }

        public IOpResult<RealTimePunchPairResultDto> ProcessRealTimePunchPairWithHours(RealTimePunchPairRequestWithHours request, string ipAddress)
        {
            return _authorizer.Handle(a =>
            {
                return a.CheckClockAdminOrNormalUser(
                    normalCheck: x => x.CheckEmployeeId(request.First.EmployeeId)
                        .CheckCanPunchFromIp(_punchProvider, request.First.EmployeeId, ipAddress)
                        .Then(d => default(object))
                ).Then(d => _punchProvider.ProcessRealTimePunchPair(request.First, request.Second))
                 .Then(d => d.ToDto());
            });
        }

        IOpResult<WeeklyScheduleDto> IClockService.GetWeeklyScheduleByEmployeeId(int employeeId)
        {
            var opResult = new OpResult<WeeklyScheduleDto>();

            var rules = GetClockClientRulesSummary(new ClockClientRulesMaps.ToClockClientRulesSummaryDto(), employeeId, _session.LoggedInUserInformation.ClientId ?? 0);

            if (rules.HasError)
            {
                opResult.SetToFail("No rules found for employee");
                return opResult;
            }
            
            int weeklyStartingDay = -1;
            int.TryParse(rules.Data.FirstOrDefault()?.WeeklyStartingDayOfWeekIdForPayFrequency.ToString(), out weeklyStartingDay);

            if (weeklyStartingDay == -1)
            {
                opResult.SetToFail("Weekly starting day not found for employee");
                return opResult;
            }

            opResult.Data = GetFirstAndLastDayOfWeek(weeklyStartingDay);

            return opResult;
        }

        #endregion

        #region Employee Mobile App-Specific Methods

        /// <summary>
        /// This is a Geolocation-Enabled app ClockService handler for Clock users who are punching
        /// through the mobile app with Time Policys that require Normal Punches.
        /// </summary>
        /// <param name="punchRequest"></param>
        /// <returns></returns>
        IOpResult<RealTimePunchWithHoursResultDto> IClockService.ProcessAppPunchRequest(AppPunchRequest punchRequest)
        {
            var result = new OpResult<RealTimePunchWithHoursResultDto>();

            _geoService.ProcessRealTimePunchLocation(punchRequest.PunchLocation);

            // save punch
            var punchResult = Self.ProcessRealTimePunch(punchRequest, punchRequest.IpAddress);

            var geofenceCheckResult = new OpResult();
            var clientHasGeofence = _geoService.ClientUsesGeofencing().MergeInto(geofenceCheckResult).Data;
            var timePolicyUsesGeofencing = _geoService.TimePolicyUsesGeofencing().MergeInto(geofenceCheckResult).Data;
            var isGeofenceUser = _geoService.UserRequiresGeofence().MergeInto(geofenceCheckResult).Data;

            if (geofenceCheckResult.HasNoError && clientHasGeofence && timePolicyUsesGeofencing && isGeofenceUser)
            {
                var clockExceptionResult = _clockEmployeeExceptionService.AddClockEmployeeException(punchRequest, punchResult.Data);

                if (clockExceptionResult.HasError)
                {
                    // what do we do if there's an error right here
                }
            }

            var punchTypeResult = Self.GetNextPunchTypeDetail(punchRequest.EmployeeId, punchTime: punchRequest.Punch,
                ipAddress: punchRequest.IpAddress, includeEmployeeClockConfig: punchRequest.IncludeConfig)
                .MergeInto(result);

            if (punchRequest.Start == null || punchRequest.End == null)
            {
                punchRequest.Start = DateTime.Now.Date.StartOfDay().StartOfWeek(DayOfWeek.Sunday);
                punchRequest.End = DateTime.Now.EndOfDay().EndOfWeek(DayOfWeek.Saturday);
            }

            // check punch option type and call the right service method from here... 
            var isInputHours = punchRequest.PunchOptionType != null && punchRequest.PunchOptionType == PunchOptionType.InputHours;

            var scheduledHours = isInputHours
                ? Self.GetInputHoursWorked(punchRequest.ClientId, punchRequest.EmployeeId, punchRequest.Start.GetValueOrDefault(),
                    punchRequest.End.GetValueOrDefault()).MergeInto(result).Data
                : Self.GetEmployeeWeeklyHoursWorked(punchRequest.ClientId, punchRequest.EmployeeId, punchRequest.Start.GetValueOrDefault(),
                    punchRequest.End.GetValueOrDefault()).MergeInto(result).Data;

            return result.SetDataOnSuccess(new RealTimePunchWithHoursResultDto
            {
                punchResult = punchResult.Data,
                punchTypeResult = punchTypeResult.Data,
                scheduledHoursResult = scheduledHours
            });
        }

        /// <summary>
        /// This is a Geolocation-Enabled app ClockService handler for Clock users who are punching
        /// through the mobile app with Time Policys that require Input Punches.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        IOpResult<RealTimePunchPairWithHoursResultDto> IClockService.ProcessAppPunchPairRequest(RealTimePunchPairRequestWithHours request, string ipAddress)
        {
            var result = new OpResult<RealTimePunchPairWithHoursResultDto>();

            var geoLocationResults = new OpResult();
            _geoService.ProcessRealTimePunchLocation(request.First.PunchLocation);
            _geoService.ProcessRealTimePunchLocation(request.First.PunchLocation);

            var punchResult = Self.ProcessRealTimePunchPairWithHours(request, ipAddress).MergeInto(result);

            // Check to see if the punch failed to be processed... if so, end here
            if (result.HasError) return result;

            //if (geoLocationResults.HasError)
            //{
            //    // what do we do if these error?
            //}

            var hasGeofenceSettingsResult = new OpResult();
            var isClientUsingGeofencing = _geoService.ClientUsesGeofencing().MergeInto(hasGeofenceSettingsResult).Data;
            var isPolicyUsingGeofencing = _geoService.TimePolicyUsesGeofencing().MergeInto(hasGeofenceSettingsResult).Data;
            var isUserUsingGeofencing = _geoService.UserRequiresGeofence().MergeInto(hasGeofenceSettingsResult).Data;
            var hasValidPunchData = punchResult.Data.First?.PunchId != null && punchResult.Data.Second?.PunchId != null;

            if (hasGeofenceSettingsResult.HasNoError && hasValidPunchData
                && isClientUsingGeofencing && isPolicyUsingGeofencing && isUserUsingGeofencing)
            {
                _clockEmployeeExceptionService.AddClockEmployeeException(request.First, punchResult.Data.First);
                _clockEmployeeExceptionService.AddClockEmployeeException(request.Second, punchResult.Data.Second);

                // should we doing something if these calls fail???
            }

            var punchDetail = Self.GetNextPunchTypeDetail(request.First.EmployeeId,
                punchTime: request.First.Punch, ipAddress: ipAddress,
                includeEmployeeClockConfig: request.First.IncludeConfig).MergeInto(result).Data;

            if (request.First.Start == null || request.First.End == null)
            {
                request.First.Start = DateTime.Now.Date.StartOfDay().StartOfWeek(DayOfWeek.Sunday);
                request.First.End = DateTime.Now.EndOfDay().EndOfWeek(DayOfWeek.Saturday);
            }

            var scheduledHours = Self.GetEmployeeWeeklyHoursWorked(request.First.ClientId, request.First.EmployeeId, 
                request.First.Start.GetValueOrDefault(),
                request.First.End.GetValueOrDefault()).MergeInto(result).Data;

            return result.SetDataOnSuccess(new RealTimePunchPairWithHoursResultDto
            {
                punchResult = punchResult.Data,
                punchTypeResult = punchDetail,
                scheduledHoursResult = scheduledHours
            });
        }

        /// <summary>
        /// This is a Geolocation-Enabled app ClockService handler for Clock users who are punching
        /// through the mobile app with Time Policys that require Input Hours.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        IOpResult<InputHoursPunchResultWithDetail> IClockService.ProcessInputHoursAppPunch(AppPunchRequest request, string ipAddress)
        {
            var result = new OpResult<InputHoursPunchResultWithDetail>();
            var dto = new ClockEmployeeBenefitDto
            {
                ClientId = request.ClientId,
                ClientCostCenterId = request.CostCenterId,
                ClientDepartmentId = request.DepartmentId,
                ClientDivisionId = request.DivisionId,
                ClientJobCostingAssignmentId1 = request.JobCostingAssignmentId1,
                ClientJobCostingAssignmentId2 = request.JobCostingAssignmentId2,
                ClientJobCostingAssignmentId3 = request.JobCostingAssignmentId3,
                ClientJobCostingAssignmentId4 = request.JobCostingAssignmentId4,
                ClientJobCostingAssignmentId5 = request.JobCostingAssignmentId5,
                ClientJobCostingAssignmentId6 = request.JobCostingAssignmentId6,
                EmployeeComment = request.EmployeeComment,
                Hours = (double)request.InputHours,
                EventDate = request.InputHoursDate,
                IsWorkedHours = true,
                EmployeeId = request.EmployeeId,
                ClientEarningId = request.ClientEarningId
            };

            var punchResult = Self.ProcessInputHoursPunch(dto, ipAddress).MergeInto(result).Data;

            if (result.HasError) return result;

            var punchDetail = Self.GetNextPunchTypeDetail(request.EmployeeId, punchTime: request.InputHoursDate,
                ipAddress: ipAddress, includeEmployeeClockConfig: request.IncludeConfig)
                .MergeInto(result).Data;

            if (request.Start == null || request.End == null)
            {
                request.Start = DateTime.Now.Date.StartOfDay().StartOfWeek(DayOfWeek.Sunday);
                request.End = DateTime.Now.Date.EndOfDay().EndOfWeek(DayOfWeek.Saturday);
            }

            var scheduledHours = Self.GetInputHoursWorked(request.ClientId, request.EmployeeId,
                request.Start.GetValueOrDefault(), request.End.GetValueOrDefault()).MergeInto(result).Data;

            return result.SetDataOnSuccess(new InputHoursPunchResultWithDetail
            {
                PunchId = punchResult.PunchId,
                // I know this is weird, but it's the ClockEmployeeBenefitDto
                Data = punchResult.Data,
                Message = punchResult.Message,
                RequestType = punchResult.RequestType,
                Success = punchResult.Success,
                PunchTypeResult = punchDetail,
                ScheduledHoursResult = scheduledHours
            });
        }

        #endregion

        #region Private Methods

        private List<ScheduledHoursWorkedDto> MergeShiftsWorkedIntoWeeklyScheduleSummary(DateTime startDate, DateTime endDate,
            IEnumerable<ClockEmployeeScheduleDto> shiftsWorked, IEnumerable<OnShiftPunchesLayout> rawPunchInfo, ClockClientScheduleDto sched = null, List<ScheduledHoursWorkedDto> existingShifts = null)
        {
            var shifts = existingShifts ?? new List<ScheduledHoursWorkedDto>();

            for (var dt = startDate; dt <= endDate; dt = dt.AddDays(1))
            {
                var existingShift = shifts.FirstOrDefault(sh => sh.Date == dt.Date);
                var s = shiftsWorked?.FirstOrDefault(sch => sch.EventDate.Date == dt.Date);

                if (existingShift != null)
                {
                    var punches = rawPunchInfo.Where(p => p.punchDate.Date == existingShift.Date);
                    existingShift.HoursWorked = punches.Count() > 0 ? punches.Sum(p => p.totalHoursWorked) : 0;

                    if (HasScheduledShift(sched, existingShift.DayOfWeek))
                    {
                        existingShift.StartTime = sched.StartTime.HasValue ? sched.StartTime : null;
                        existingShift.EndTime = sched.EndDate.HasValue ? sched.EndDate : null;

                        if (existingShift.StartTime != null && existingShift.EndTime != null)
                        {
                            var startTime = existingShift.StartTime ?? default(DateTime);
                            var endTime = existingShift.EndTime ?? default(DateTime);
                            existingShift.ScheduledHours = (endTime - startTime).TotalHours;

                            if (existingShift.ScheduledHours < 0)
                            {
                                endTime = endTime.AddDays(1);
                                existingShift.ScheduledHours = (endTime - startTime).TotalHours;
                            }
                        }
                    }
                    else
                    {
                        existingShift.StartTime = null;
                        existingShift.EndTime = null;
                        existingShift.ScheduledHours = 0;
                    }
                }
                else if (s != null && s.StartTime1.HasValue)
                {
                    var punches = rawPunchInfo.Where(p => p.punchDate.Date == s.EventDate.Date);

                    var shift = new ScheduledHoursWorkedDto
                    {
                        Date = dt.Date,
                        DayOfWeek = dt.DayOfWeek,
                        ScheduledHours = s.EndTime1.Value.Subtract(s.StartTime1.Value).TotalHours,
                        HoursWorked = punches.Count() > 0 ? punches.Sum(p => p.totalHoursWorked) : 0
                    };

                    shift.StartTime = s.StartTime1.HasValue ? s.StartTime1 : null;
                    shift.EndTime = s.EndTime1.HasValue ? s.EndTime1 : null;

                    var shiftStart = shift.StartTime ?? default(DateTime);
                    var shiftEnd = shift.EndTime ?? default(DateTime);

                    shift.ScheduledHours = (shiftEnd - shiftStart).TotalHours;

                    if (shift.ScheduledHours < 0)
                    {
                        shiftEnd = shiftEnd.AddDays(1);
                        shift.ScheduledHours = (shiftEnd - shiftStart).TotalHours;
                    }

                    shifts.Add(shift);
                }
                else
                {
                    // hardcoded start/end times based on case EEMA-54
                    var start = new DateTime(dt.Year, dt.Month, dt.Date.Day, 8, 0, 0);
                    var end = new DateTime(dt.Year, dt.Month, dt.Date.Day, 17, 30, 0);
                    var punches = rawPunchInfo.Where(rp => rp.punchDate.Date == dt.Date);
                    var newShift = new ScheduledHoursWorkedDto
                    {
                        Date = dt.Date,
                        DayOfWeek = dt.DayOfWeek,
                        HoursWorked = punches.Count() > 0 ? punches.Sum(p => p.totalHoursWorked) : 0,
                        IsSystemDefault = true
                    };

                    if (HasScheduledShift(sched, dt.DayOfWeek))
                    {
                        newShift.StartTime = sched.StartTime.HasValue ? sched.StartTime : null;
                        newShift.EndTime = sched.StopTime.HasValue ? sched.StopTime : null;
                        var startTime = newShift.StartTime ?? default(DateTime);
                        var endTime = newShift.EndTime ?? default(DateTime);
                        newShift.ScheduledHours = (endTime - startTime).TotalHours;

                        if (newShift.ScheduledHours < 0)
                        {
                            endTime = endTime.AddDays(1);
                            newShift.ScheduledHours = (endTime - startTime).TotalHours;
                        }
                    }
                    else
                    {
                        newShift.StartTime = null;
                        newShift.EndTime = null;
                        newShift.ScheduledHours = 0; // accounting for a half hour break hardcoded EEMA-54
                    }

                    shifts.Add(newShift);
                }

            }

            return shifts;
        }

        private bool HasScheduledShift(ClockClientScheduleDto sch, DayOfWeek dow)
        {
            if (sch == null) return false;

            if (dow == DayOfWeek.Sunday)
                return sch.IsSunday.Value;

            if (dow == DayOfWeek.Monday)
                return sch.IsMonday.Value;

            if (dow == DayOfWeek.Tuesday)
                return sch.IsTuesday.Value;

            if (dow == DayOfWeek.Wednesday)
                return sch.IsWednesday.Value;

            if (dow == DayOfWeek.Thursday)
                return sch.IsThursday.Value;

            if (dow == DayOfWeek.Friday)
                return sch.IsFriday.Value;

            if (dow == DayOfWeek.Saturday)
                return sch.IsSaturday.Value;

            return false;
        }

        private WeeklyScheduleDto GetFirstAndLastDayOfWeek(int weeklyStartingDay)
        {
            var weeklySchedule = new WeeklyScheduleDto();
            var now = DateTime.Now;

            weeklySchedule.FirstDayOfWeek = firstDayOfTheWeekHelper(now, weeklyStartingDay);
            weeklySchedule.LastDayOfWeek = weeklySchedule.FirstDayOfWeek.AddDays(6);

            return weeklySchedule;
        }

        private DateTime GetDateOfFirstDayofWeek(DateTime datCurrentDate , int intStartingDay )
        {
            if (datCurrentDate.DayOfWeek  < (DayOfWeek)(intStartingDay - 1)){
                datCurrentDate = datCurrentDate.AddDays(-7);
            }

            return datCurrentDate.AddDays(-(int)datCurrentDate.DayOfWeek).AddDays(intStartingDay - 1);
        }
        private DateTime GetDateOfLastDayofWeek(DateTime datCurrentDate, int intStartingDay)
        {
            DateTime dt = GetDateOfFirstDayofWeek(datCurrentDate, intStartingDay);
            return dt.AddDays(6);
        }

        private void SetRequestedPunchData(int counter, int employeeId, DataRowView rv, ref ClockTimeCardDto timeCard)
        {
            DateTime modified = new DateTime();
            string time = "";

            modified = DateTime.Parse(rv["ModifiedPunch"].ToString());
            time = modified.ToShortTimeString();
            counter = counter % 4;

            if (counter == 1)
            {
                timeCard.Out = time;
                timeCard.OutDateTime = modified;
                timeCard.OutEmployeePunchId = rv["ClockEmployeePunchRequestID"].ToString();
                timeCard.OutClockClientLunchId = rv["ClockClientLunchID"].ToString();
                timeCard.OutTimeZoneId = rv["TimeZoneID"].ToString();
                timeCard.OutIsPending = true;
                timeCard.OutHasUrl = true;
                timeCard.OutUrl = GetModalUrl(counter, employeeId, timeCard.Date, timeCard.OutEmployeePunchId == "" ? 0 : Int32.Parse(timeCard.OutEmployeePunchId));
            }
            else if (counter == 3)
            {
                timeCard.Out2 = time;
                timeCard.Out2DateTime = modified;
                timeCard.Out2EmployeePunchId = rv["ClockEmployeePunchRequestID"].ToString();
                timeCard.Out2ClockClientLunchId = rv["ClockClientLunchID"].ToString();
                timeCard.Out2TimeZoneId = rv["TimeZoneID"].ToString();
                timeCard.Out2IsPending = true;
                timeCard.Out2HasUrl = true;
                timeCard.Out2Url = GetModalUrl(counter, employeeId, timeCard.Date, timeCard.Out2EmployeePunchId == "" ? 0 : Int32.Parse(timeCard.Out2EmployeePunchId));
            }

        }

        private string GetModalUrl(int counter, int employeeId, string dateOfPunch, int clockEmployeePunchRequestId)
        {
            string url = "";

            url = "ModalContainer.aspx?URL=ClockEmployeePunchRequest.aspx&EmployeeId=" + employeeId + "&PunchDate=" + dateOfPunch + "&PunchRequestID=" + clockEmployeePunchRequestId;

            return url;
        }


        private DateTime firstDayOfTheWeekHelper(DateTime current, int startingDay)
        {
            if ((int)current.DayOfWeek < (startingDay - 1))
            {
                current = current.AddDays(-7);
            }

            current = current.AddDays(-(int)current.DayOfWeek).AddDays(startingDay - 1);

            return current;
        }

        private ClockTimeCardDto CreateDailyTotalTimeCard(ref List<ClockTimeCardDto> timecard, DateTime startDate, DateTime endDate,
            int employeeId, int clientId, int userId, int punchOption, string desc, int weeklyTotalCount, bool showHoursInHundreths,
            string notes, int typePremiumHours)
        {
            var timeCardDT = new ClockTimeCardDto();
            bool firstRecord = true;
            int hours = 0;
            double combinedTotals = 0;
            //var timeCardsToTotal = timecard.Where(x => x.dayAmount == dayAmount);

            var earningDHL = _objClockEarningDesHistoryService
                .GetClockEarningDesHistoryListByDate(clientId, userId, employeeId, startDate.ToString(), endDate.ToString(), (desc == "") ? 0 : 1);

            if (earningDHL.Tables[0].Rows.Count > 0)
            {
                if (weeklyTotalCount != 1)
                {
                    //var dr = earningDHL.Tables[0].Rows[0];
                    foreach (DataRow dr in earningDHL.Tables[0].Rows)
                    {
                        var departmentLabel = "";
                        var costCenterLabel = "";
                        var skillLabel = "";
                        var department = dr["Department"].ToString();
                        var costCenter = dr["CostCenter"].ToString();
                        var skill = dr["Skill"].ToString();
                        var exceptions = "";
                        double parsedHours;
                        if (!double.TryParse(dr["Hours"].ToString(), out parsedHours)) parsedHours = 0.00;

                        if (punchOption == 2 && desc == "")
                        {
                            timeCardDT = new ClockTimeCardDto();
                            timeCardDT.Date = dr["Description"].ToString();

                            if (showHoursInHundreths)
                            {
                                double hoursDouble = _objClockCalculateActivityService
                                   .Round(TimeSpan.FromHours(parsedHours).TotalHours, 2);
                                timeCardDT.Hours = hoursDouble.ToString("0.00");
                                timeCardDT.HoursDouble = hoursDouble;
                            }
                            else
                            {
                                timeCardDT.Hours = FormatHourAsString((decimal)TimeSpan.FromHours(parsedHours).TotalMinutes);
                                timeCardDT.HoursDouble = parsedHours;

                            }
                            if (timeCardDT.Hours.Contains("-"))
                                timeCardDT.Hours = "-" + timeCardDT.Hours.Replace("-", string.Empty).Trim();

                            var jobCostingList = _objClientJobCostingService.GetClientJobCostingList(clientId);

                            if (jobCostingList.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow jobDr in jobCostingList.Tables[0].Rows)
                                {
                                    switch (Int32.Parse(jobDr["JobCostingTypeId"].ToString()))
                                    {
                                        case 3:
                                            departmentLabel = jobDr["Description"].ToString();
                                            break;
                                        case 4:
                                            costCenterLabel = jobDr["Description"].ToString();
                                            break;
                                        case 1:
                                            skillLabel = jobDr["Description"].ToString();
                                            break;

                                        default:
                                            break;
                                    }
                                }

                                if (department != "")
                                    exceptions = departmentLabel + ":" + department;
                                if (costCenter != "")
                                    exceptions += costCenterLabel + ":" + costCenter;
                                if (skill != "")
                                    exceptions += skillLabel + ":" + skill;

                                timeCardDT.Exceptions = exceptions;
                            }

                            if (timeCardDT.Notes == null)
                                timeCardDT.Notes = "";

                            if (Boolean.Parse(dr["IsBenefit"].ToString()) && notes == "")
                            {
                                if (!DBNull.Value.Equals(dr["EmployeeComment"]))
                                    if (!(notes.Contains(dr["EmployeeComment"].ToString())))
                                        notes = dr["EmployeeComment"].ToString();
                            }

                            timeCardDT.IsWorkedHours = Boolean.Parse(dr["IsWorkedHours"].ToString());

                            timeCardDT.Notes = notes;
                            timecard.Add(timeCardDT);
                            notes = "";

                        } // end of punch option = 2 and desc = ""     
                        else if ((desc != "" && Boolean.Parse(dr["IsWorkedHours"].ToString()) && Int32.Parse(dr["ClientEarningCategoryID"].ToString()) == 2)
                                || (Boolean.Parse(dr["IsBenefit"].ToString()) && Int32.Parse(dr["ClientEarningCategoryID"].ToString()) == 2))
                        {
                            if (!DBNull.Value.Equals(dr["EmployeeComment"]))
                                if (!(notes.Contains(dr["EmployeeComment"].ToString())))
                                    notes += ((notes == "") ? "" : ", ") + dr["EmployeeComment"].ToString();
                        }
                        else
                        {
                            timeCardDT = new ClockTimeCardDto();

                            if (firstRecord && desc != "")
                            {
                                timeCardDT = new ClockTimeCardDto();
                                timeCardDT.Day = desc;
                                timeCardDT.RowTypeId = 4;
                                firstRecord = false;
                                timecard.Add(timeCardDT);
                                timeCardDT = new ClockTimeCardDto();
                            }

                            timeCardDT.Date = dr["Description"].ToString();
                            timeCardDT.RowTypeId = 3;

                            if (!firstRecord && desc != "")
                                timeCardDT.RowTypeId = 4;

                            if ((timeCardDT.Date == null))
                                timeCardDT.Date = "";

                            if (showHoursInHundreths)
                            {
                                double hoursDouble = _objClockCalculateActivityService.Round(TimeSpan.FromHours(parsedHours).TotalHours, 2);
                                timeCardDT.Hours = hoursDouble.ToString("0.00");
                                timeCardDT.HoursDouble = hoursDouble;
                            }
                            else
                            {
                                timeCardDT.HoursDouble = parsedHours;
                                timeCardDT.Hours = FormatHourAsString((decimal)TimeSpan.FromHours(parsedHours).TotalMinutes);
                            }
                            if (timeCardDT.Hours.Contains("-"))
                                timeCardDT.Hours = "-" + timeCardDT.Hours.Replace("-", string.Empty).Trim();

                            timeCardDT.Exceptions = dr["Department"].ToString() + ((dr["CostCenter"].ToString() == "" ? "" : (dr["Department"].ToString() == "" ? "" : ", ")) + dr["CostCenter"].ToString());

                            if (((Boolean.Parse(dr["IsBenefit"].ToString())) && notes == ""))
                                if (!DBNull.Value.Equals(dr["EmployeeComment"]))
                                    if (!(notes.Contains(dr["EmployeeComment"].ToString())))
                                        notes += ((notes == "") ? "" : ", ") + dr["EmployeeComment"].ToString();


                            if (!(timeCardDT.Date.ToUpper() == "OVERTIME" && DBNull.Value.Equals(dr["Hours"])))
                            {
                                timecard.Add(timeCardDT);
                            }
                            notes = "";
                        }
                    }
                }

                if (desc == "Total Earnings")
                {
                    desc = "Total Hours";
                    if (timecard.Count > 1)
                    {
                        foreach (DataRow dr in earningDHL.Tables[0].Rows)
                        {
                            if ((Boolean.Parse(dr["IsWorkedHours"].ToString()) && Int32.Parse(dr["ClientEarningCategoryID"].ToString()) == 2))
                            {

                            }
                            else
                            {
                                if (typePremiumHours == 2 && Int32.Parse(dr["ClientEarningCategoryID"].ToString()) != 2)
                                {

                                }
                                else
                                {
                                    combinedTotals += double.Parse(dr["Hours"].ToString());
                                }
                            }
                        }
                        if (combinedTotals > 0)
                        {
                            timeCardDT = new ClockTimeCardDto();
                            timeCardDT.Day = desc;
                            timeCardDT.RowTypeId = 5;
                            if (showHoursInHundreths)
                                if (combinedTotals < 1)
                                {
                                    double hoursDouble = _objClockCalculateActivityService.Round(TimeSpan.FromMinutes(combinedTotals).TotalHours, 3);
                                    timeCardDT.Hours = hoursDouble.ToString("0.00");
                                    timeCardDT.HoursDouble = hoursDouble;
                                }
                                else
                                {
                                    double hoursDouble = _objClockCalculateActivityService.Round(TimeSpan.FromHours(combinedTotals).TotalHours, 3);
                                    timeCardDT.Hours = hoursDouble.ToString("0.00");
                                    timeCardDT.HoursDouble = hoursDouble;
                                }
                            else
                            {
                                timeCardDT.HoursDouble = combinedTotals;
                                timeCardDT.Hours = FormatHourAsString((decimal)TimeSpan.FromHours(combinedTotals).TotalMinutes);
                            }
                            if (timeCardDT.Hours.Contains("-"))
                                timeCardDT.Hours = "-" + timeCardDT.Hours.Replace("-", string.Empty).Trim();
                            timecard.Add(timeCardDT);
                        }

                    }
                }
            }


            return timeCardDT;
        }

        private void CreateUnApprovedBenefitsTimeCard(ref List<ClockTimeCardDto> clockTimeCards, bool showHoursInHundredths, int userId, int employeeId, int clientId, DateTime requestedDate)
        {
            var employeeLML = _objLeaveManagementService.GetEmployeeLeaveManagementListByStatusAndDate(userId, clientId, employeeId, 1, requestedDate, requestedDate);

            if (employeeLML.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in employeeLML.Tables[0].Rows)
                {
                    var timeCard = new ClockTimeCardDto();

                    timeCard.Date = dr["Description"].ToString();
                    timeCard.RowTypeId = 0;

                    if (showHoursInHundredths)
                    {
                        double hoursDouble = _objClockCalculateActivityService.Round(TimeSpan.FromHours(double.Parse(dr["Hours"].ToString())).TotalHours, 2);
                        timeCard.Hours = hoursDouble.ToString("0.00");
                        timeCard.HoursDouble = hoursDouble;
                    }
                    else
                    {
                        double hoursDouble = double.Parse(dr["Hours"].ToString());
                        timeCard.Hours = FormatHourAsString((decimal)TimeSpan.FromHours(double.Parse(dr["Hours"].ToString())).TotalMinutes);
                        timeCard.HoursDouble = hoursDouble;
                    }

                    clockTimeCards.Add(timeCard);

                }
            }
        }

        private void CreateDailyPunchTotals(ref ClockTimeCardDto timeCard, DataView exceptionHistory, DataView employeePoints, DateTime punchDate, int employeeId, bool showHoursInHundredths, bool isNewDate)
        {
            TimeSpan ts = new TimeSpan();
            Decimal nTotalHours = new decimal(0);
            Decimal nTotalMinutes = new decimal(0);

            var datIn = (timeCard.InDateTime != null) ? timeCard.InDateTime : DateTime.Parse(CommonConstants.NO_DATE_SELECTED);
            var datIn2 = (timeCard.In2DateTime != null) ? timeCard.In2DateTime : DateTime.Parse(CommonConstants.NO_DATE_SELECTED);
            var datOut = (timeCard.OutDateTime != null) ? timeCard.OutDateTime : DateTime.Parse(CommonConstants.NO_DATE_SELECTED);
            var datOut2 = (timeCard.Out2DateTime != null) ? timeCard.Out2DateTime : DateTime.Parse(CommonConstants.NO_DATE_SELECTED);

            var inTimeZoneId = (timeCard.InTimeZoneId != null) ? Int32.Parse(timeCard.InTimeZoneId) : 1;
            var in2TimeZoneId = (timeCard.In2TimeZoneId != null) ? Int32.Parse(timeCard.In2TimeZoneId) : 1;
            var outTimeZoneId = (timeCard.OutTimeZoneId != null) ? (timeCard.OutTimeZoneId != "") ? Int32.Parse(timeCard.OutTimeZoneId) : 1 : 1;
            var out2TimeZoneId = (timeCard.Out2TimeZoneId != null) ? (timeCard.Out2TimeZoneId != "") ? Int32.Parse(timeCard.Out2TimeZoneId) : 1 : 1;

            if (datOut != DateTime.Parse(CommonConstants.NO_DATE_SELECTED) && datOut != DateTime.MinValue)
            {
                ts = datOut.Subtract(datIn);
                nTotalHours = (decimal)ts.TotalHours;
                _objClockCalculateActivityService.GetNewHoursFromTimeZoneDifference(inTimeZoneId, outTimeZoneId, null, ref nTotalHours);
                nTotalMinutes = (decimal)TimeSpan.FromHours((double)nTotalHours).TotalMinutes;
            }

            if (datOut2 != DateTime.Parse(CommonConstants.NO_DATE_SELECTED) && datOut2 != DateTime.MinValue)
            {
                ts = datOut2.Subtract(datIn2);
                nTotalHours = (decimal)ts.TotalHours;
                _objClockCalculateActivityService.GetNewHoursFromTimeZoneDifference(inTimeZoneId, outTimeZoneId, null, ref nTotalHours);
                nTotalMinutes += (decimal)TimeSpan.FromHours((double)nTotalHours).TotalMinutes;
            }

            ts = new TimeSpan(0, (int)nTotalMinutes, 0);

            if (nTotalMinutes != 0)
            {
                if (showHoursInHundredths)
                {
                    double hoursDouble = _objClockCalculateActivityService.Round(TimeSpan.FromMinutes((double)nTotalMinutes).TotalHours, 2);
                    timeCard.Hours = hoursDouble.ToString("0.00");
                    timeCard.HoursDouble = hoursDouble;
                }
                else
                {
                    timeCard.HoursDouble = _objClockCalculateActivityService.Round(TimeSpan.FromMinutes((double)nTotalMinutes).TotalHours, 2);
                    timeCard.Hours = FormatHourAsString(nTotalMinutes);
                }
            }

            var timePolicy = _objClockClientService.GetClockClientTimePolicyByEmployeeID(employeeId);
            var exceptions = "";

            if (isNewDate)
            {
                exceptionHistory.RowFilter = "EventDateString=" + "'" + punchDate.ToString("MM/dd/yyyy") + "'";
                if (exceptionHistory.Count > 0)
                {
                    foreach (DataRowView rv in exceptionHistory)
                    {
                        var hours = (!DBNull.Value.Equals(rv["Hours"]) ? decimal.Parse(rv["Hours"].ToString()) : 0);
                        exceptions += ", " + rv["ClockException"].ToString();

                        if (hours > 0)
                            exceptions = exceptions + " " + FormatHourAsString(hours * 60);
                        exceptions += GetEmployeePointsString(employeePoints, DateTime.Parse(rv["EventDate"].ToString()), Int32.Parse(rv["ClockExceptionID"].ToString()), Int32.Parse(rv["ClockEmployeeExceptionHistoryID"].ToString()));

                        var id = rv["ClockEmployeePunchID"].ToString();

                        if (id == timeCard.InEmployeePunchId)
                            timeCard.InHasException = true;
                        if (id == timeCard.In2EmployeePunchId)
                            timeCard.In2HasException = true;
                        if (id == timeCard.OutEmployeePunchId)
                            timeCard.OutHasException = true;
                        if (id == timeCard.Out2EmployeePunchId)
                            timeCard.Out2HasException = true;
                    }
                    timeCard.Exceptions = exceptions.Substring(1);
                }
            }

        }

        private string GetEmployeePointsString(DataView employeePoints, DateTime eventDate, int clockExceptionId, int clockEmployeeExceptionHistoryId)
        {
            string[] searchValuePoints = new string[3];
            int indexPoints = 0;
            double points = 0;

            if (employeePoints != null && employeePoints.Table != null)
            {
                searchValuePoints[0] = eventDate.ToString("MM/dd/yyyy");
                searchValuePoints[1] = clockExceptionId.ToString();
                searchValuePoints[2] = clockEmployeeExceptionHistoryId.ToString();

                indexPoints = employeePoints.Find(searchValuePoints);
                if (indexPoints > 0)
                {
                    points = double.Parse(employeePoints[indexPoints]["Amount"].ToString());
                    if (points != 0)
                        return " (" + points.ToString() + ")";
                }

            }
            return "";
        }

        private string FormatHourAsString(decimal minutes)
        {
            var hours = "";
            var min = (double)minutes;

            hours = Math.Truncate(TimeSpan.FromMinutes(min).TotalHours).ToString() + ":" + TimeSpan.FromMinutes(min).Minutes.ToString().PadLeft(2, '0');
            hours = (hours == "00:00") ? "" : hours;

            return hours;
        }

        public IOpResult<IEnumerable<ClockEmployeeScheduleDto>> GetEmployeeScheduleByEmployeeIdAndDateRange(int clientId, IEnumerable<int> employeeIds, DateTime startDate, DateTime endDate)
        {
            var opResult = new OpResult<IEnumerable<ClockEmployeeScheduleDto>>();
            var endOfDay = startDate.AddDays(0.99);
            var ces = _session.UnitOfWork.LaborManagementRepository
                .ClockEmployeeScheduleQuery()
                .ByClientId(clientId)
                .ByEmployeeIds(employeeIds)
                .ByEventDateRange(startDate, endDate)
                .ExecuteQueryAs(x => new ClockEmployeeScheduleDto
                {
                    EventDate = x.EventDate,
                    EmployeeId = x.EmployeeId,
                    StartTime1 = x.StartTime1,
                    EndTime1 = x.EndTime1
                });
            opResult.Data = ces;
            return opResult;
        }

        #endregion
    }
}