using Dominion.Authentication.Intermediate.Util;
using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Geofence;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.Misc;
using Dominion.Core.Dto.Payroll;
using Dominion.Core.Dto.User;
using Dominion.Core.Services.Api.ClockException;
using Dominion.Core.Services.Dto.Employee;
using Dominion.Core.Services.Interfaces;
using Dominion.Core.Services.Internal.Providers;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Employee.ClockEmployeeInfo;
using Dominion.Domain.Entities.Labor;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.LaborManagement.Dto.Clock.Misc;
using Dominion.LaborManagement.Dto.DataServiceObjects;
using Dominion.LaborManagement.Dto.JobCosting;
using Dominion.LaborManagement.Dto.Scheduling;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.LaborManagement.Service.Api.DataServicesInjectors;
using Dominion.LaborManagement.Service.Internal.ClockException;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.LaborManagement.Service.Mapping;
using Dominion.LaborManagement.Service.Mapping.Clock;
using Dominion.Utility.DateAndTime;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.Mapping;
using Dominion.Utility.OpResult;
using Dominion.Utility.Query;
using Dominion.Utility.Query.LinqKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Dominion.Authentication.Intermediate.Util;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Misc;
using Dominion.Core.Dto.Payroll;
using Dominion.Core.Dto.User;
using Dominion.Core.Services.Dto.Employee;
using Dominion.Core.Services.Internal.Providers;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.Utility.DateAndTime;
using Serilog;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    public class EmployeePunchProvider : IEmployeePunchProvider
    {
        #region Utility Classes

        #endregion

        /// <summary>
        /// The value that represents a null value for IDs when interfacing with legacy code.
        /// </summary>
        private const int NullId = int.MinValue;
        private readonly IBusinessApiSession _session;
        private readonly IJobCostingProvider _jobCostingProvider;
        private readonly IDsDataServicesClockService _dataServicesClockService;
        private readonly IClientService _clientSerivce;
        private readonly IClientSettingProvider _clientSettingProvider;
        private readonly IContactProvider _contactProvider;
        private readonly IPayrollService _payrollService;
        private readonly IGeoProvider _geoProvider;
        private readonly IClockEmployeeExceptionService _clockEmployeeExceptionService;
        private readonly ISchedulingProvider _schedulingProvider;
        private readonly ILogger _logger;

        internal IEmployeePunchProvider Self { get; set; }

        public EmployeePunchProvider(IBusinessApiSession session, IJobCostingProvider jobCostingProvider,
            IDsDataServicesClockService dataServicesClockService, IClientService clientService, IClientSettingProvider clientSettingProvider,
            IPayrollService payrollService, IContactProvider contactProvider, IGeoProvider geoProvider, 
            IClockEmployeeExceptionService clockEmployeeExceptionService, ISchedulingProvider schedulingProvider,
            ILogger logger)
        {
            Self = this;

            _session = session;
            _jobCostingProvider = jobCostingProvider;
            _dataServicesClockService = dataServicesClockService;
            _clientSerivce = clientService;
            _clientSettingProvider = clientSettingProvider;
            _payrollService = payrollService;
            _contactProvider = contactProvider;
            _geoProvider = geoProvider;
            _clockEmployeeExceptionService = clockEmployeeExceptionService;
            _schedulingProvider = schedulingProvider;
            _logger = logger;
        }

        public IOpResult<IEnumerable<TimeClockClientDto>> GetAccessibleClients(int[] clientIds)
        {
            var opResult = new OpResult<IEnumerable<TimeClockClientDto>>();

            opResult.TrySetData(() => _session.UnitOfWork.ClientRepository.QueryClients()
                .ByClientIds(clientIds)
                .ExecuteQueryAs(new ClientMaps.ToTimeClockClientDto()));

            return opResult;
        }

        public IOpResult<IEnumerable<ClockEmployeePunchDto>> GetEmployeePunches(int employeeId)
        {
            var opResult = new OpResult<IEnumerable<ClockEmployeePunchDto>>();

            var query = _session.UnitOfWork.TimeClockRepository.GetClockEmployeePunchQuery().ByEmployeeId(employeeId);
            opResult.Data = query.ExecuteQueryAs(new ClockEmployeePunchMaps.ClockEmployeePunchMap());

            return opResult;
        }

        public IOpResult<IEnumerable<ClockEmployeePunchDto>> GetEmployeePunches(int employeeId, DateTime startDate, DateTime endDate)
        {
            var opResult = new OpResult<IEnumerable<ClockEmployeePunchDto>>();

            var query = _session.UnitOfWork.TimeClockRepository.GetClockEmployeePunchQuery()
                .ByEmployeeId(employeeId)
                .ByDates(startDate, endDate);

            opResult.Data = query.ExecuteQueryAs(new ClockEmployeePunchMaps.ClockEmployeePunchMap());

            return opResult;
        }

        public IOpResult<IEnumerable<ClockEmployeePunchDto>> GetEmployeePunchesByIdList(int employeeId, int[] punchIdList)
        {
            var opResult = new OpResult<IEnumerable<ClockEmployeePunchDto>>();

            var query = _session.UnitOfWork.TimeClockRepository.GetClockEmployeePunchQuery()
                .ByEmployeeId(employeeId)
                .ByClockEmployeePunchIdList(punchIdList);

            opResult.Data = query.ExecuteQueryAs(new ClockEmployeePunchMaps.ClockEmployeePunchMap());

            return opResult;
        }

        public IOpResult<IEnumerable<ClockEmployeePunchDto>> GetEmployeePunches(int employeeId, DateTime shiftDate)
        {
            var opResult = new OpResult<IEnumerable<ClockEmployeePunchDto>>();

            // FIXME
            var startDate = shiftDate;
            var endDate   = shiftDate;
            //var startDate = shiftDate.AddDays(-1);
            //var endDate   = shiftDate.AddDays(1);

            var query = _session.UnitOfWork.TimeClockRepository.GetClockEmployeePunchQuery()
                .ByEmployeeId(employeeId)
                .ByDates(startDate, endDate, true);

            opResult.Data = query.ExecuteQueryAs(new ClockEmployeePunchMaps.ClockEmployeePunchMap());

            return opResult;
        }

        public IOpResult<IEnumerable<ClockClientHolidayDto>> GetClientHolidays(int clientId)
        {
            var opResult = new OpResult<IEnumerable<ClockClientHolidayDto>>();

            var accesResult = _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(clientId)
                .MergeInto(opResult);

            if (!accesResult.Success) return opResult;

            var query = _session.UnitOfWork.TimeClockRepository.GetClockClientHolidayQuery().ByClient(clientId);

            opResult.TrySetData(() => query.ExecuteQueryAs(new ClockClientHolidayMaps.ToClockClientHolidayDto()));

            return opResult;
        }

        public IOpResult<IEnumerable<ClockClientLunchDto>> GetClientLunches(int clientId)
        {
            var opResult = new OpResult<IEnumerable<ClockClientLunchDto>>();
            var query = _session.UnitOfWork.TimeClockRepository.GetClockClientLunchQuery().ByClient(clientId);

            opResult.TrySetData(() => query.ExecuteQueryAs(new ClockClientLunchMaps.ToClockClientLunchDto()));

            return opResult;
        }

        public IOpResult<IEnumerable<ClockClientLunchSelectedDto>> GetClientLunchesSelected(int clientId)
        {
            var opResult = new OpResult<IEnumerable<ClockClientLunchSelectedDto>>();

            opResult.TrySetData(_session.UnitOfWork.LaborManagementRepository.ClockClientTimePolicyQuery()
                .ByClientId(clientId: clientId)
                .ExecuteQuery().SelectMany(tp => tp.LunchSelected)
                .Select(ccls => new ClockClientLunchSelectedDto()
                {
                    ClockClientTimePolicyId = ccls.ClockClientTimePolicyId,
                    ClockClientLunchId = ccls.ClockClientLunchId
                }).ToList);

            return opResult;
        }

        public IOpResult<IEnumerable<ClockClientRulesDto>> GetClientRules(int clientId)
        {
            var opResult = new OpResult<IEnumerable<ClockClientRulesDto>>();
            var query = _session.UnitOfWork.TimeClockRepository.GetClockClientRules().ByClient(clientId);

            opResult.TrySetData(() => query.ExecuteQueryAs(new ClockClientRulesMaps.ToClockClientRulesDto()));

            return opResult;
        }

        public IOpResult<IEnumerable<ClockClientTimePolicyDtos.ClockClientTimePolicyDto>> GetClientTimePolicies(int clientId)
        {
            var opResult = new OpResult<IEnumerable<ClockClientTimePolicyDtos.ClockClientTimePolicyDto>>();
            var query = _session.UnitOfWork.TimeClockRepository.GetClockClientTimePolicyQuery().ByClientId(clientId);

            opResult.TrySetData(() => query.ExecuteQueryAs(new ClockClientTimePolicyMaps.ToClockClientTimePolicyDto()));

            return opResult;
        }

        public IOpResult<IEnumerable<ClockEmployeeDto>>  GetClockEmployees(int clientId)
        {
            var opResult = new OpResult<IEnumerable<ClockEmployeeDto>>();

            opResult.TrySetData(() =>
            {
                return _session.UnitOfWork.PayrollRepository.QueryEmployeePay().ByEmployeeActivity(true).ByClient(clientId)
                    .ExecuteQueryAs(
                        pay =>
                            new ClockEmployeeDto()
                            {
                                AverageWeeklyHours = pay.ClockEmployee.AverageWeeklyHours,
                                BadgeNumber = pay.ClockEmployee.BadgeNumber,
                                EmployeePin =  pay.ClockEmployee.EmployeePin,
                                ClientId = pay.ClientId,
                                ClockClientTimePolicyId = pay.ClockEmployee.ClockClientTimePolicyId,
                                EmployeeId = pay.EmployeeId,
                                IsDayLightSavingsObserved = pay.ClockEmployee.IsDayLightSavingsObserved,
                                TimeZoneId = pay.ClockEmployee.TimeZoneId
                            });

            });

            return opResult;
        }

        public IOpResult<IEnumerable<ClockTimePolicyEmployeeDto>> GetEmployeesByTimePolicy(IEnumerable<int> timePolicyIds)
        {
            var opResult = new OpResult<IEnumerable<ClockTimePolicyEmployeeDto>>();
            var employees = new OpResult<IEnumerable<EmployeeDto>>();

            var clientId = _session.LoggedInUserInformation.ClientId.Value;

            employees.TrySetData(() =>
            {
                var query = _session.UnitOfWork.PayrollRepository.QueryEmployeePay()
                    .ByEmployeeActivity(true)
                    .ByTimePolicyIds(timePolicyIds)
                    .ByClient(clientId);

                return query.ExecuteQueryAs(pay => new EmployeeDto()
                 {
                    ClientId = pay.ClientId,
                    EmployeeId = pay.EmployeeId,
                    FirstName = pay.Employee.FirstName,
                    LastName = pay.Employee.LastName,
                    EmployeeNumber = pay.Employee.EmployeeNumber,
                    JobTitle = pay.Employee.JobTitle,
                    EeocLocationId = pay.Employee.EeocLocationId,
                    ClientDepartmentId = pay.Employee.ClientDepartmentId,
                    PayFrequency = pay.PayFrequency,
                    Department = pay.Employee.Department == null ? null : new Dto.Department.ClientDepartmentDto()
                    {
                        Name = pay.Employee.Department.Name,
                        ClientDepartmentId = pay.Employee.Department.ClientDepartmentId
                    },
                    ClockEmployee = new ClockEmployeeDto()
                    {
                        TimePolicy = new ClockClientTimePolicyDtos.ClockClientTimePolicyDto()
                        {
                            Name = pay.ClockEmployee.TimePolicy.Name,
                            ClockClientTimePolicyId = pay.ClockEmployee.TimePolicy.ClockClientTimePolicyId
                        },
                        ClockClientTimePolicyId = pay.ClockEmployee.ClockClientTimePolicyId,
                        GeofenceEnabled = pay.ClockEmployee.GeofenceEnabled,
                    },
                    EEOCLocation = pay.Employee.EEOCLocation == null ? null : new Core.Dto.Contact.EEOCLocationDto()
                    {
                        EeocLocationDescription = pay.Employee.EEOCLocation.EeocLocationDescription,
                        EeocLocationId = pay.Employee.EEOCLocation.EeocLocationId
                    },
                    DirectSupervisorId = pay.Employee.DirectSupervisorID,
                    DirectSupervisor = pay.Employee.DirectSupervisor == null ? null : new UserDto()
                    {
                        FirstName = pay.Employee.DirectSupervisor.FirstName,
                        LastName = pay.Employee.DirectSupervisor.LastName,
                        UserId = pay.Employee.DirectSupervisor.UserId
                    }
                }).ToOrNewList();

            });

            _contactProvider.LoadProfileImageForEmployees(clientId, employees.Data).MergeInto(employees);

            var sortedEmployees = employees.Data.Where(x => x.ClockEmployee.ClockClientTimePolicyId != 0).GroupBy(x => x.ClockEmployee.ClockClientTimePolicyId, (k, g) => new ClockTimePolicyEmployeeDto
            {
                TimePolicyId = k,
                TimePolicyName = g.First().ClockEmployee.TimePolicy.Name,
                Employees = g,
            }).OrderBy(y => y.TimePolicyName).ThenBy(z => z.Employees.First().LastName).ToList();


            opResult.Data = sortedEmployees;

            return opResult;
        }

        public IOpResult<IEnumerable<ClockClientRulesSummaryDto>> GetClockClientRulesSummary(int? employeeId = null, int? clientId = null)
        {
            return GetClockClientRulesSummary(new ClockClientRulesMaps.ToClockClientRulesSummaryDto(), employeeId, clientId);
        }

        public IOpResult<IEnumerable<ClockClientRulesSummaryDto>> GetClockClientRulesSummary<TMapper>(TMapper mapper, int? employeeId = null, int? clientId = null)
            where TMapper : ExpressionMapper<EmployeePay, ClockClientRulesSummaryDto>
        {
            var opResult = new OpResult<IEnumerable<ClockClientRulesSummaryDto>>();

            var query = _session.UnitOfWork.PayrollRepository.QueryEmployeePay().ByEmployeeActivity(true);
            if (employeeId != null)
                query.ByEmployeeId(employeeId.Value);
            if (clientId != null)
                query.ByClient(clientId.Value);

            var results = query.ExecuteQuery();

            var dtoList = new List<ClockClientRulesSummaryDto>();

            results.ForEach(r =>
            {
                var dto = mapper.Map(r);
                dtoList.Add(dto);
            });

            opResult.Data = dtoList;
            return opResult;
        }

        IOpResult<CheckPunchTypeResultDto> IEmployeePunchProvider.GetNextPunchTypeDetail(int employeeId, DateTime? timeOfPunch, string ipAddress, bool includeEmployeeClockConfig, bool isHwClockPunch)
        {
            var opResult = new OpResult<CheckPunchTypeResultDto>();
            var punchDetail = new CheckPunchTypeResultDto();

            var punchTime = timeOfPunch ?? DateTime.Now;
            var startDate = punchTime.Date.AddDays(-2);
            var endDate   = punchTime.ToEndOfDay();
            int clockEmployeeScheduleCostCenterId = 0;

            var emp = Self.GetEmployeeClockConfiguration(employeeId, startDate, endDate).MergeInto(opResult).Data;
            if (opResult.HasError)
                return opResult;

            if (emp?.ClockEmployee == null) return opResult;//return employee not found or clock profile not setup

            punchDetail.PayPeriodEnded =
                _session.UnitOfWork.TimeClockRepository.ClockEmployeePayPeriodEndedSproc(emp.ClientId, emp.EmployeeId).FirstOrDefault();

            if (emp.ClockEmployee != null)
            {

                var clientUsesGeo = _geoProvider.ClientUsesGeofencing();
                var tpUsesGeo = _geoProvider.TimePolicyUsesGeofencing();
                var userUsesGeo = _geoProvider.UserRequiresGeofence();                

                PunchOptionType? userPunchOption = emp.ClockEmployee.TimePolicy.Rules.PunchOption;
                
                if (clientUsesGeo.Data && tpUsesGeo.Data && userUsesGeo.Data)
                {
                    userPunchOption = emp.ClockEmployee.TimePolicy.Rules.PunchOption == PunchOptionType.None_ClockOnly ?
                        PunchOptionType.NormalPunch : emp.ClockEmployee.TimePolicy.Rules.PunchOption;
                }

                punchDetail.ShouldHideCostCenter    = emp.ClockEmployee.TimePolicy.Rules.IsHideCostCenter;
                punchDetail.ShouldHideDepartment    = emp.ClockEmployee.TimePolicy.Rules.IsHideDepartment;
                punchDetail.ShouldHideJobCosting    = emp.ClockEmployee.TimePolicy.Rules.IsHideJobCosting;
                punchDetail.ShouldHideEmployeeNotes = emp.ClockEmployee.TimePolicy.Rules.IsHideEmployeeNotes;
                punchDetail.AllowInputPunches       = emp.ClockEmployee.TimePolicy.Rules.AllowInputPunches;
                punchDetail.PunchOption             = userPunchOption;
                punchDetail.HasMobilePunching       = emp.ClockEmployee.TimePolicy.Rules.AllowMobilePunching;
            }

            // WE DON'T WANT TO CHECK THE SCHEDULE COST CENTER IF THEY ARE INPUT HOURS/INPUT PUNCHES BC THE PUNCH TIME THEY ENTER MAY NOT BE THE CURRENT TIME
            if (!(emp.ClockEmployee.TimePolicy.Rules.AllowInputPunches || emp.ClockEmployee.TimePolicy.Rules.PunchOption == PunchOptionType.InputHours))
            {
                var clockEmployeeSchedules = _schedulingProvider.GetClockEmployeeSchedulesByPunchTime(punchTime, employeeId, emp.ClockEmployee.TimePolicy.Rules.InEarlyGraceTime, emp.ClockEmployee.TimePolicy.Rules.OutLateGraceTime).MergeInto(opResult);
                if (opResult.HasError)
                    return opResult;

                if (clockEmployeeSchedules.HasData && clockEmployeeSchedules.Data.Count() > 0)
                {
                    var clockEmployeeSchedule = clockEmployeeSchedules.Data.OrderBy(x => x.ClockEmployeeScheduleId).FirstOrDefault();
                    if (clockEmployeeSchedule.StartTime1 != null && clockEmployeeSchedule.EndTime1 != null && clockEmployeeSchedule.StartTime1.GetValueOrDefault().AddMinutes(emp.ClockEmployee.TimePolicy.Rules.InEarlyGraceTime * -1) <= punchTime && clockEmployeeSchedule.EndTime1.GetValueOrDefault().AddMinutes(emp.ClockEmployee.TimePolicy.Rules.OutLateGraceTime) >= punchTime)  
                    {
                        clockEmployeeScheduleCostCenterId = clockEmployeeSchedule.ClientCostCenterId1 ?? 0;
                    }
                    else if (clockEmployeeSchedule.StartTime2 != null && clockEmployeeSchedule.EndTime2 != null && clockEmployeeSchedule.StartTime2.GetValueOrDefault().AddMinutes(emp.ClockEmployee.TimePolicy.Rules.InEarlyGraceTime * -1) <= punchTime && clockEmployeeSchedule.EndTime2.GetValueOrDefault().AddMinutes(emp.ClockEmployee.TimePolicy.Rules.OutLateGraceTime) >= punchTime)
                    {
                        clockEmployeeScheduleCostCenterId = clockEmployeeSchedule.ClientCostCenterId2 ?? 0;
                    }
                    else if (clockEmployeeSchedule.StartTime3 != null && clockEmployeeSchedule.EndTime3 != null && clockEmployeeSchedule.StartTime3.GetValueOrDefault().AddMinutes(emp.ClockEmployee.TimePolicy.Rules.InEarlyGraceTime * -1) <= punchTime && clockEmployeeSchedule.EndTime3.GetValueOrDefault().AddMinutes(emp.ClockEmployee.TimePolicy.Rules.OutLateGraceTime) >= punchTime)
                    {
                        clockEmployeeScheduleCostCenterId = clockEmployeeSchedule.ClientCostCenterId3 ?? 0;
                    }
                }
            }

            punchDetail.HomeCostCenterId = clockEmployeeScheduleCostCenterId == 0 ? emp.HomeCostCenterId : clockEmployeeScheduleCostCenterId;
            punchDetail.HomeDepartmentId = emp.HomeDepartmentId;
            
            var costCenterOptionType = Self.GetClockCostCenterRequirementClientOption(emp.ClientId).MergeInto(opResult).Data;
            punchDetail.IsCostCenterSelectionRequired = costCenterOptionType == ClockCostCenterRequirementType.Always 
                || (costCenterOptionType == ClockCostCenterRequirementType.WhenNoHomeCostCenter && punchDetail.HomeCostCenterId == null);
            
            var lunches = emp.ClockEmployee.TimePolicy.Lunches.ToList();

            var shiftPunches = GetShiftPunchList(emp.ClockEmployee.TimePolicy.Rules, emp.ClockEmployee.Punches, punchTime).ToOrNewList();

            punchDetail.IsFirstPunchOfDay = !emp.ClockEmployee.Punches.Any();

            if (shiftPunches.Any())
            {
                var punch = shiftPunches.Last();
                punchDetail.LunchCostCenterId             = lunches.FirstOrDefault(l => l.ClockClientLunchId == punch.ClockClientLunchId)?.ClientCostCenterId;
                punchDetail.LastOutCostCenterId           = punch.ClientCostCenterId;
                punchDetail.LastPunchTime                 = punch.ModifiedPunch;
                punchDetail.CostCenterId                  = punch.ClientCostCenterId;
                punchDetail.DepartmentId                  = punch.ClientDepartmentId;
                punchDetail.DivisionId                    = punch.ClientDivisionId;
                punchDetail.ClientJobCostingAssignmentId1 = punch.ClientJobCostingAssignmentId1;
                punchDetail.ClientJobCostingAssignmentId2 = punch.ClientJobCostingAssignmentId2;
                punchDetail.ClientJobCostingAssignmentId3 = punch.ClientJobCostingAssignmentId3;
                punchDetail.ClientJobCostingAssignmentId4 = punch.ClientJobCostingAssignmentId4;
                punchDetail.ClientJobCostingAssignmentId5 = punch.ClientJobCostingAssignmentId5;
                punchDetail.ClientJobCostingAssignmentId6 = punch.ClientJobCostingAssignmentId6;

                var lunchPunches = shiftPunches.Where(p => p.ClockClientLunchId != null);
                if (shiftPunches.Count() % 2 == 0) //last punch was an out punch 
                {
                    punchDetail.IsOutPunch                      = false;

                    if (lunchPunches.Count() % 2 != 0)
                    {
                        punchDetail.PunchTypeId = punch.ClockClientLunchId;
                        punchDetail.ShouldDisablePunchTypeSelection = true;
                    }
                }
                else //Last Punch was In Punch
                {
                    punchDetail.IsOutPunch = true;

                    // If the user has already submitted 2 punches today, let's not pre-populate the "lunch" 
                    // punch type so that they don't accidentally submit another lunch punch without doing so 
                    // purposefully.
                    if (lunchPunches.Count() % 2 != 0)
                    {
                        punchDetail.PunchTypeId = punch.ClockClientLunchId;
                        punchDetail.ShouldDisablePunchTypeSelection = true;
                    }
                }

                if (isHwClockPunch || punchDetail.IsOutPunch)
                {
                    // shiftPunches come back from GetShiftPunchList() ordered newest->oldest
                    var shiftPunchLast = shiftPunches.Last();

                    var effectiveLunches = lunches.Where(lunch =>
                    {
                        var isEffective   = isLunchEffective(lunch, punchTime);
                        var isInTimeFrame = isLunchInTimeFrame(lunch, punchTime);
                        return isEffective && isInTimeFrame;
                    }).ToList();

                    if (isHwClockPunch && !punchDetail.IsOutPunch)
                    {
                        // If this is an in-punch and the last punch was a lunch out-punch with a ClockClientLunchId,
                        // choose the lunch policy associated with that ClockClientLunchId to use as the effectiveLunch.
                        effectiveLunches = effectiveLunches.FindAll(lunch => shiftPunchLast.ClockClientLunchId == lunch.ClockClientLunchId);
                    }
                    EmployeeTimePolicyLunchConfiguration effectiveLunch = effectiveLunches.FirstOrDefault();

                    // Only try to detect "undeclared" in/out lunch punches if this is from a Hardware Clock AND the employee has effectiveLunches. 
                    if (isHwClockPunch && effectiveLunches.Any())
                    {
                        if (effectiveLunch != null)
                        {
                            // Unlike in the !isHwClockPunch version, we don't want to reassign these if effectiveLunch is null.
                            punchDetail.LunchCostCenterId = effectiveLunch.ClientCostCenterId;
                            punchDetail.PunchTypeId       = effectiveLunch.ClockClientLunchId;
                        }
                    }
                    // Otherwise, fall through to the previous iteration of lunch out-punch detection.
                    else if (punchDetail.IsOutPunch) // punch 
                    {
                        // If the user has submitted a lunch punch already, don't send back a lunch id 
                        // because the frontend will pre-populate the form with that punch type
                        if (!shiftPunches.Any(p => p.ClockClientLunchId != null))
                        {
                            punchDetail.LunchCostCenterId = effectiveLunch?.ClientCostCenterId;
                            punchDetail.PunchTypeId       = effectiveLunch?.ClockClientLunchId;
                        }
                    }
                }
            }
            else
            {
                //no punches from last two days, so assume this is an in punch
                punchDetail.IsOutPunch = false;
                punchDetail.ShouldDisablePunchTypeSelection = true;
            }

            //do IP check if an IP was specified
            if (ipAddress.IsNotNullOrEmpty())
            {
                punchDetail.IpAddress      = ipAddress;
                punchDetail.CanPunchFromIp = Self.CanEmployeePunchFromIp(employeeId, ipAddress).Data?.CanPunch ?? false;
            }

            if (includeEmployeeClockConfig)
            {
                punchDetail.EmployeeClockConfiguration = emp;
            }

            return opResult.SetDataOnSuccess(punchDetail);
        }

        private static bool isLunchInTimeFrame(EmployeeTimePolicyLunchConfiguration lunch, DateTime punchTime)
        {
            bool isInTimeFrame;

            if (lunch.StartTime?.TimeOfDay <= lunch.StopTime?.TimeOfDay)
            {
                isInTimeFrame = (punchTime.TimeOfDay >= lunch.StartTime?.TimeOfDay && punchTime.TimeOfDay <= lunch.StopTime?.TimeOfDay);
            }
            else
            {
                isInTimeFrame = (punchTime.TimeOfDay >= lunch.StartTime?.TimeOfDay && punchTime.TimeOfDay <= new TimeSpan(23, 59, 59))
                             || (punchTime.TimeOfDay <= lunch.StopTime?.TimeOfDay);
            }

            return isInTimeFrame;
        }

        private static bool isLunchEffective(EmployeeTimePolicyLunchConfiguration lunch, DateTime punchTime)
        {
            var shiftDate = punchTime.DayOfWeek;
            var isEffective =
                (shiftDate == DayOfWeek.Sunday)    && lunch.IsSunday    ||
                (shiftDate == DayOfWeek.Monday)    && lunch.IsMonday    ||
                (shiftDate == DayOfWeek.Tuesday)   && lunch.IsTuesday   ||
                (shiftDate == DayOfWeek.Wednesday) && lunch.IsWednesday ||
                (shiftDate == DayOfWeek.Thursday)  && lunch.IsThursday  ||
                (shiftDate == DayOfWeek.Friday)    && lunch.IsFriday    ||
                (shiftDate == DayOfWeek.Saturday)  && lunch.IsSaturday;
            return isEffective;
        }

        IOpResult<PunchTypeItemResult> IEmployeePunchProvider.GetPunchTypeItems(int employeeId)
        {
            var result = new OpResult<PunchTypeItemResult>();

            var info = _session.UnitOfWork.TimeClockRepository
                .GetClockEmployeeQuery()
                .ByEmployeeId(employeeId)
                .FirstOrDefaultAs(x => new
                {
                    x.ClientId,
                    x.TimePolicy.Rules.AllowInputPunches,
                    x.TimePolicy.Rules.ClockClientRulesId,
                    x.TimePolicy.ClockClientTimePolicyId,
                    Lunches = x.TimePolicy.LunchSelected.Select(l => new
                    {
                        l.Lunch.ClockClientLunchId,
                        l.Lunch.Name
                    }),
                    PunchOption = x.TimePolicy.Rules.PunchOption ?? PunchOptionType.NormalPunch,
                });

            result.CheckForNotFound(info);
            if (result.HasError)
                return result;

            var punchTypeItems  = new List<PunchTypeItem>();
            var punchTypeResult = new PunchTypeItemResult
            {
                ClientId                = info.ClientId,
                ClockClientTimePolicyId = info.ClockClientTimePolicyId,
                ClockClientRulesId      = info.ClockClientRulesId,
                Items                   = punchTypeItems
            };

            // input punching does not use punch types, so just return an empty result
            if (info.AllowInputPunches)
            {
                punchTypeResult.Source = PunchTypeItemSource.None;
            }
            else
            {
                switch (info.PunchOption)
                {
                    case PunchOptionType.InputHours:
                    {
                        //mimics logic from ClockEmployeePunch.aspx.vb
                        var earnings = Self.GetEarningPunchTypeItems(info.ClientId, hideOtherEarnings:false, inludeOnlyEarningCategory:null).MergeInto(result).Data;
                        punchTypeItems.AddRange(earnings);
                        break;
                    }
                    default:
                    {
                        //for all other punch types, load the lunches/breaks into the list
                        //starting with a 'Normal Punch' option
                        punchTypeItems.Add(new LunchPunchTypeItem
                        {
                            ClockClientLunchId = null,
                            IsDefault          = true,
                            Name               = "Normal Punch"
                        });

                        foreach (var lunch in info.Lunches.OrderBy(l => l.Name))
                        {
                            punchTypeItems.Add(new LunchPunchTypeItem
                            {
                                ClockClientLunchId = lunch.ClockClientLunchId,
                                Name               = lunch.Name
                            });
                        }
                        break;
                    }
                }
            }
            
            return result.SetDataOnSuccess(punchTypeResult);
        }

        IOpResult<IEnumerable<EarningPunchTypeItem>> IEmployeePunchProvider.GetEarningPunchTypeItems(int clientId, bool hideOtherEarnings,
            ClientEarningCategory? inludeOnlyEarningCategory)
        {
            var result = new OpResult<IEnumerable<EarningPunchTypeItem>>();

            //load client earning list
            //this mimics logic found in spGetTimeClockClientEarningListByClientEarningCategoryID
            var earningCategories = new List<ClientEarningCategory>();
            if (inludeOnlyEarningCategory.HasValue)
            {
                earningCategories.Add(inludeOnlyEarningCategory.Value);
            }
            else
            {
                earningCategories.AddRange(new []
                {
                    ClientEarningCategory.Regular,
                    ClientEarningCategory.Overtime,
                    ClientEarningCategory.Double,
                    ClientEarningCategory.HolidayWorked
                });

                if (!hideOtherEarnings)
                    earningCategories.Add(ClientEarningCategory.Other);
            }

            var userTypeOverrides = new List<UserType>();
            var userInfo = _session.UnitOfWork.UserRepository
                .QueryUsers()
                .ByUserId(_session.LoggedInUserInformation.UserId)
                .FirstOrDefaultAs(x => new { x.UserTypeId });
            switch (userInfo.UserTypeId)
            {
                case UserType.Supervisor:
                    userTypeOverrides.Add(UserType.Supervisor);
                    break;
                case UserType.CompanyAdmin:
                    userTypeOverrides.Add(UserType.Supervisor);
                    userTypeOverrides.Add(UserType.CompanyAdmin);
                    break;
            }


            var earnings = _session.UnitOfWork.PayrollRepository
                .QueryClientEarnings()
                .ByClientId(clientId)
                .ByIsBlockedFromTimeClock(isBlockedFromTimeClock: false, allowedUserTypes: userTypeOverrides)
                .ByEarningCategoryIds(earningCategories.ToArray())
                .ExecuteQueryAs(x => new EarningPunchTypeItem
                {
                    ClientEarningId = x.ClientEarningId,
                    Code            = x.Code,
                    Description     = x.Description,
                    EarningCategory = x.EarningCategoryId
                })
                .Select(x =>
                {
                    if (x.EarningCategory == ClientEarningCategory.Regular)
                        x.IsDefault = true;
                    return x;
                })
                .OrderBy(x => x.Name)
                .ToList();

            return result.SetDataOnSuccess(earnings);
        }

        IOpResult<InputHoursPunchRequestResult> IEmployeePunchProvider.ProcessInputHoursPunchRequest(ClockEmployeeBenefitDto request)
        {
            var result = new OpResult<InputHoursPunchRequestResult>();

            // check to make sure that the punch request is for a date/time that falls outside of the last closed payroll
            CheckPunchIsNotClosedPayroll(request).MergeInto(result);
            if (result.HasError) return result;
            CheckPunchIsNotLockedTimecard(request).MergeInto(result);
            if (result.HasError) return result;

            var eventDate = request.EventDate ?? DateTime.Now;
            var employee = Self.GetEmployeeWithPunchesByEmployeeId(request.EmployeeId, eventDate, eventDate)
                .MergeInto(result).Data;

            if (result.HasError) return result;

            var empJobCostingDto = CalculateJobCosting(
                    request.ClientId,
                    request.ClientJobCostingAssignmentId1,
                    request.ClientJobCostingAssignmentId2,
                    request.ClientJobCostingAssignmentId3,
                    request.ClientJobCostingAssignmentId4,
                    request.ClientJobCostingAssignmentId5,
                    request.ClientJobCostingAssignmentId6);

            // Use the supplied cost center/department/division first, then use job costing, then employee default "home".
            var defaultCostCenterId = CalculateDefaultCostCenterId(request.ClientCostCenterId, empJobCostingDto, employee);
            var defaultDivisionId = CalculateDefaultDivisionId(request.ClientDivisionId, empJobCostingDto, employee);
            var defaultDepartmentId = CalculateDefaultDepartmentId(request.ClientDepartmentId, empJobCostingDto, employee);

            var entity = new ClockEmployeeBenefit
            {
                EmployeeId = request.EmployeeId,
                ClientEarningId = request.ClientEarningId,
                Hours = request.Hours,
                EventDate = request.EventDate,
                ClientCostCenterId = defaultCostCenterId,
                ClientDivisionId = defaultDivisionId,
                ClientDepartmentId = defaultDepartmentId,
                ClientShiftId = request.ClientShiftId,
                IsApproved = false,
                IsWorkedHours = true,
                ClientId = request.ClientId,
                ClientJobCostingAssignmentId1 = request.ClientJobCostingAssignmentId1,
                ClientJobCostingAssignmentId2 = request.ClientJobCostingAssignmentId2,
                ClientJobCostingAssignmentId3 = request.ClientJobCostingAssignmentId3,
                ClientJobCostingAssignmentId4 = request.ClientJobCostingAssignmentId4,
                ClientJobCostingAssignmentId5 = request.ClientJobCostingAssignmentId5,
                ClientJobCostingAssignmentId6 = request.ClientJobCostingAssignmentId6,
                Subcheck = request.Subcheck,
                EmployeeComment = request.EmployeeComment,
                EmployeeClientRateId = request.EmployeeClientRateId
            };

            var resultData = new InputHoursPunchRequestResult
            {
                Success = true,
                Data = request,
                Message = "Successfully saved your punch.",
                RequestType = PunchOptionType.InputHours
            };

            _session.SetModifiedProperties(entity);
            _session.UnitOfWork.RegisterPostCommitAction(() =>
            {
                resultData.PunchId = entity.ClockEmployeeBenefitId;
                resultData.Data.ClockEmployeeBenefitId = entity.ClockEmployeeBenefitId;
            });

            _session.UnitOfWork.RegisterNew(entity);

            return result.TrySetData(() => resultData);
        }

        private IOpResult CheckPunchIsNotLockedTimecard(ClockEmployeeBenefitDto request)
        {
            var opResult = new OpResult();
            var punchTime = request.EventDate ?? DateTime.Now;

            var payrollPeriodRangeForEmployee = _payrollService.GetPayrollPeriodRangeForEmployee(request.EmployeeId, request.ClientId);

            if (payrollPeriodRangeForEmployee.Data == null)
            {
                return opResult;
            }

            if (punchTime.Date >= payrollPeriodRangeForEmployee.Data.Start && punchTime.Date <= payrollPeriodRangeForEmployee.Data.End)
            {
                opResult.SetToFail(() => new GenericMsg("Cannot associate this punch with a locked timecard."));
            }

            return opResult;
        }

        IOpResult<IEnumerable<PunchRerunRawDto>> IEmployeePunchProvider.GetRerunPunches(DateTime startDate, DateTime endDate, IEnumerable<int> clientIds = null)
        {
            var result = new OpResult<IEnumerable<PunchRerunRawDto>>();

            var rawPunches = result.TryGetData(() => {

                var idList = clientIds.ToOrNewList();

                if (!idList.Any())
                    return _session.UnitOfWork.TimeClockRepository
                        .GetClockEmployeePunchQuery()
                        .ByDates(startDate, endDate)
                        .ExecuteQueryAs(x => new PunchRerunRawDto
                        {
                            ClockEmployeePunchId = x.ClockEmployeePunchId,
                            EmployeeId           = x.EmployeeId,
                            FirstName            = x.Employee.FirstName,
                            LastName             = x.Employee.LastName,
                            EmployeeNumber       = x.Employee.EmployeeNumber,
                            ClientId             = x.ClientId,
                            ClientName           = x.Employee.Client.ClientName,
                            ClientCode           = x.Employee.Client.ClientCode,
                            RawPunch             = x.RawPunch,
                            ModifiedPunch        = x.ModifiedPunch,
                            ShiftDate            = x.ShiftDate
                        })
                        .ToList();

                var allPunches = new List<PunchRerunRawDto>();
                foreach(var clientId in idList)
                {
                    var clientPunches = _session.UnitOfWork.TimeClockRepository
                        .GetClockEmployeePunchQuery()
                        .ByDates(startDate, endDate)
                        .ByClientId(clientId)
                        .ExecuteQueryAs(x => new PunchRerunRawDto
                        {
                            ClockEmployeePunchId = x.ClockEmployeePunchId,
                            EmployeeId           = x.EmployeeId,
                            FirstName            = x.Employee.FirstName,
                            LastName             = x.Employee.LastName,
                            EmployeeNumber       = x.Employee.EmployeeNumber,
                            ClientId             = x.ClientId,
                            ClientName           = x.Employee.Client.ClientName,
                            ClientCode           = x.Employee.Client.ClientCode,
                            RawPunch             = x.RawPunch,
                            ModifiedPunch        = x.ModifiedPunch,
                            ShiftDate            = x.ShiftDate
                        })
                        .ToList();

                    if (clientPunches.Any())
                        allPunches.AddRange(clientPunches);
                }
                return allPunches;
            });

            // APPROVALS
            var approvals = _session.UnitOfWork.LaborManagementRepository.ClockEmployeeApproveDateQuery()
                .ByEventDateFrom(startDate)
                .ByEventDateTo(endDate)
                .ExecuteQueryAs(x => new
                {
                    x.EmployeeId,
                    x.EventDate,
                    x.IsApproved
                })
                .Where(x => x.IsApproved ?? false)
                .ToOrNewList();

            var approvalLookup = new Dictionary<int, Dictionary<DateTime, bool>>();
            foreach (var approval in approvals)
            {
                if (!approvalLookup.TryGetValue(approval.EmployeeId, out var ee))
                {
                    ee = new Dictionary<DateTime, bool>();
                    approvalLookup.Add(approval.EmployeeId, ee);
                }

                if (!ee.ContainsKey(approval.EventDate.Date))
                    ee.Add(approval.EventDate.Date, true);
            }
            
            foreach(var punch in rawPunches)
            {
                if(approvalLookup.TryGetValue(punch.EmployeeId, out var eeApproval))
                {
                    if (eeApproval.TryGetValue(punch.ModifiedPunch.Date, out var dateApproval))
                        punch.IsApproved = dateApproval;
                }
            }

            return result.SetDataOnSuccess(rawPunches);

            

        }

        IOpResult<IEnumerable<PunchRerunClientDto>> IEmployeePunchProvider.GetRerunPunchesGroupedByClient(DateTime startDate, DateTime endDate, IEnumerable<int> clientIds = null)
        {
            var result = new OpResult<IEnumerable<PunchRerunClientDto>>();

            var rawPunches = Self.GetRerunPunches(startDate, endDate, clientIds).MergeInto(result).Data.ToOrNewList();

            // GROUPING
            var grouped = rawPunches.GroupBy(x => new { x.ClientId, x.ClientCode, x.ClientName }, (k1, g1) => new PunchRerunClientDto
            {
                ClientId   = k1.ClientId,
                ClientCode = k1.ClientCode,
                ClientName = k1.ClientName,
                Employees  = g1.GroupBy(x => new { x.EmployeeId, x.EmployeeNumber, x.FirstName, x.LastName }, (k2, g2) => new PunchRerunEmployeeDto
                {
                    EmployeeId     = k2.EmployeeId,
                    EmployeeNumber = k2.EmployeeNumber,
                    FirstName      = k2.FirstName,
                    LastName       = k2.LastName,
                    Punches        = g2.Select(p => new PunchRerunInfoDto
                    { 
                        ClockEmployeePunchId = p.ClockEmployeePunchId,
                        ClientId             = p.ClientId,
                        EmployeeId           = p.EmployeeId,
                        ShiftDate            = p.ShiftDate,
                        ModifiedPunch        = p.ModifiedPunch,
                        RawPunch             = p.RawPunch,
                        IsApproved           = p.IsApproved ?? false
                    })
                })
            }).ToList();

            return result.SetDataOnSuccess(grouped);
        }

        IOpResult<PunchRerunInfoDto> IEmployeePunchProvider.RerunCalculateWeeklyActivity(PunchRerunInfoDto punch, bool shouldRound)
        {
            var result = new OpResult<PunchRerunInfoDto>(punch);

            result.TryCatch(() => _dataServicesClockService
                .CalculateWeeklyActivity(new CalculateWeeklyActivityRequestArgs
                {
                    ClientId               = punch.ClientId,
                    EmployeeId             = punch.EmployeeId,
                    ClockEmployeePunchId   = punch.ClockEmployeePunchId,
                    DatePunch              = punch.ModifiedPunch,
                    ShouldRoundPunch       = shouldRound,
                    ShouldIncludeAutoLunch = true
                }, _clientSerivce)
            );

            return result;
        }

        IOpResult<IEnumerable<(PunchRerunInfoDto Punch, IOpResult Result)>> IEmployeePunchProvider.RerunCalculateWeeklyActivity(IEnumerable<PunchRerunInfoDto> punches, bool shouldRound)
        {
            var result = new OpResult<IEnumerable<(PunchRerunInfoDto Punch, IOpResult Result)>>();

            var punchResults = new List<(PunchRerunInfoDto Punch, IOpResult Result)>();
            foreach (var punch in punches)
            {
                var punchResult = Self.RerunCalculateWeeklyActivity(punch, shouldRound);
                punchResults.Add((punch, punchResult));
            }

            return result.SetDataOnSuccess(punchResults);
        }

        public IOpResult<RealTimePunchResult> ProcessRealTimePunch(RealTimePunchRequest request)
        {
            var opResult = new OpResult<RealTimePunchResult>();

            CheckPunchIsNotClosedPayroll(request).MergeInto(opResult);
            if (opResult.HasError) return opResult;

            opResult.TryCatch(() =>
            {
                opResult.TrySetData(() => ProcessRealTimePunchInternal(
                    request.ClientId,
                    request.EmployeeId,
                    request.CostCenterId,
                    request.DepartmentId,
                    request.DivisionId,
                    request.IsOutPunch,
                    request.EmployeeComment,
                    request.Comment,
                    request.ClockHardwareId,
                    request.ClockName,
                    request.PunchTypeId,
                    request.OverridePunchTime,
                    request.OverrideLunchBreak,
                    request.JobCostingAssignmentId1,
                    request.JobCostingAssignmentId2,
                    request.JobCostingAssignmentId3,
                    request.JobCostingAssignmentId4,
                    request.JobCostingAssignmentId5,
                    request.JobCostingAssignmentId6,
                    request.PunchLocation?.PunchLocationID,
                    request.ClientMachineId
                    ));
            });
            return opResult;
        }

        public IOpResult<RealTimePunchResultDto> ProcessRealTimePunchAttempt(ClockEmployeePunchAttemptDto request)
        {
            var opResult = new OpResult<RealTimePunchResultDto>();

            if (request == null)
                return opResult;

            var attempt = new ClockEmployeePunchAttempt
            {
                ClientCostCenterID = request.CostCenterId,
                ClientDepartmentID = request.DepartmentId,
                ClientDivisionID = request.DivisionId,
                ClientID = request.ClientId,
                ClientJobCostingAssignmentID1 = request.JobCostingAssignmentId1,
                ClientJobCostingAssignmentID2 = request.JobCostingAssignmentId2,
                ClientJobCostingAssignmentID3 = request.JobCostingAssignmentId3,
                ClientJobCostingAssignmentID4 = request.JobCostingAssignmentId4,
                ClientJobCostingAssignmentID5 = request.JobCostingAssignmentId5,
                ClientJobCostingAssignmentID6 = request.JobCostingAssignmentId6,
                ClockHardwareId = request.ClockHardwareId,
                ClockName = request.ClockName,
                Comment = request.Comment,
                EmployeeComment = request.EmployeeComment,
                EmployeeID = request.EmployeeId,
                InputHours = request.InputHours,
                InputHoursDate = request.InputHoursDate,
                IsOutPunch = request.IsOutPunch,
                Modified = DateTime.Now,
                ModifiedBy = _session.LoggedInUserInformation.UserId,
                OverrideLunchBreak = request.OverrideLunchBreak,
                OverridePunchTime = request.OverridePunchTime,
                PunchTypeID = request.PunchTypeId
            };

            _session.UnitOfWork.RegisterNew(attempt);

            _session.UnitOfWork.Commit().MergeInto(opResult);

            opResult.Data = new RealTimePunchResultDto() { Succeeded = false, Message = "There was an issue with saving your punch." };

            return opResult;
        }

        #region Misc Helper Methods

        private CheckPunchAllowedResult CheckPunchAllowed(int employeeId, DateTime punchTime)
        {
            Debug.WriteLine($"CHECK PUNCH ALLOWED : START GetEmployee {DateTime.Now}");
            var emp = _session.UnitOfWork.EmployeeRepository.GetEmployee(employeeId);
            if (emp == null)
                return new CheckPunchAllowedResult() { IsAllowed = false, Message = "Invalid Employee ID" };
            Debug.WriteLine($"CHECK PUNCH ALLOWED : END GetEmployee {DateTime.Now}");

            Debug.WriteLine($"CHECK PUNCH ALLOWED : START GetRules {DateTime.Now}");
            var rules = GetClockClientRulesSummary(new ClockClientRulesMaps.ToClockClientRulesSummaryPartial(), employeeId).Data.FirstOrDefault();
            //var rules = emp.ClockEmployee.TimePolicy.Rules;
            Debug.WriteLine($"CHECK PUNCH ALLOWED : END GetRules {DateTime.Now}");
            if (rules == null)
                return new CheckPunchAllowedResult() { IsAllowed = false, Message = "Unable to load rules" };//TODO: do I need this?

            if (rules.OutLateAllowPunchTime + rules.InEarlyAllowPunchTime + rules.OutEarlyAllowPunchTime +
                rules.InLateAllowPunchTime == 0)
                return new CheckPunchAllowedResult() { IsAllowed = true };

            /***************************************************************************************
            *   Call into legacy code. C# requires all parameters be provided. Its ugly.
            ***************************************************************************************/
            Debug.WriteLine($"START CALL INTO TO GET SCHEDULE : {DateTime.Now}");

            var scheduleRequestArgs = new DataServicesGetScheduleArgs()
            {
                ClientId = emp.ClientId,
                EmployeeId = emp.EmployeeId,
                ClockClientTimePolicyId = emp.ClockEmployee?.ClockClientTimePolicyId ?? 0,
                ApplyHoursOption = rules.ApplyHoursOption ?? 0,
                ShouldHideMultipleSchedules = rules.HideMultipleSchedules,
                PunchTime = punchTime
            };

            var scheduleResult = _dataServicesClockService.GetSchedule(scheduleRequestArgs);

            Debug.WriteLine($"END CALL INTO TO GET SCHEDULE : {DateTime.Now}");

            var result = new CheckPunchAllowedResult()
            {
                IsAllowed = true,
                StartTime = scheduleResult.StartTime,
                StopTime = scheduleResult.StopTime,
                ClockClientScheduleChangeHistoryId = scheduleResult.ChangeHistoryId
            };

            var roundPunchRquest = new RoundPunchRequestArgs()
            {
                ClientId = emp.ClientId,
                EmployeeId = emp.EmployeeId,
                PunchTime = punchTime,
                StartTime = scheduleResult.StartTime,
                StopTime = scheduleResult.StopTime,
                RulesDataSet = rules.ToDataSet(),
            };

            var scheduleExists = scheduleResult.ScheduleId > 0;
            if (!scheduleExists && !rules.AllowInputPunches)
            {
                //If there are no schedules, we can continue to process the punch
                return result;
            }

            var punches = emp.Punches.Where(p => (p.ShiftDate ?? p.ModifiedPunch) >= punchTime.Date && (p.ShiftDate ?? p.ModifiedPunch) <= punchTime.ToEndOfDay()).ToList();

            // Check how Early & Late the punch is allowed to be
            if (punches.Any())
            {
                Debug.WriteLine($"START PUNCH TYPE LOGIC AND CHECK FOR EXCEPTIONS : {DateTime.Now}");
                if (punches.Count() % 2 == 0)//can in punch
                    return result;

                if (rules.OutEarlyAllowPunchTime != 0 &&
                    !rules.AllowInputPunches &&
                    CheckForException(ClockExceptionType.LeftEarly, punchTime, scheduleResult.StartTime, scheduleResult.StopTime))
                {
                    var roundedLeftEarlyPunch =_dataServicesClockService
                        .RoundPunch(roundPunchRquest, (int) ClockExceptionType.LeftEarly);

                    if (roundedLeftEarlyPunch > DateTime.Now.ToNoDateSelected())
                        return result;

                    result.IsAllowed = false;
                    result.Message = $"Your punch was not accepted, you will be able to punch at " +
                                     $"{scheduleResult.StopTime.AddMinutes(rules.OutEarlyAllowPunchTime * -1).ToShortTimeString()}";
                }
                else if (rules.OutLateAllowPunchTime != 0 &&
                         !rules.AllowInputPunches &&
                         CheckForException(ClockExceptionType.LeftLate, punchTime, scheduleResult.StartTime, scheduleResult.StopTime))
                {
                    var roundedLeftLatePunch = _dataServicesClockService
                        .RoundPunch(roundPunchRquest,(int) ClockExceptionType.LeftLate);

                    if (roundedLeftLatePunch > DateTime.Now.ToNoDateSelected())
                        return result;

                    result.IsAllowed = false;
                    result.Message = "Your Punch Was Not Accepted, Please See Your Supervisor";
                }
                Debug.WriteLine($"END PUNCH TYPE LOGIC AND CHECK FOR EXCEPTIONS : {DateTime.Now}");
            }
            else //no punches for the day, first punch
            {
                // Checking for early does not need to happen when input punches is being used.
                if (rules.InEarlyAllowPunchTime != 0 &&
                    !rules.AllowInputPunches &&
                    CheckForException(ClockExceptionType.ArrivedEarly, punchTime, scheduleResult.StartTime, scheduleResult.StopTime))
                {
                    var roundedInEarlyPunch = _dataServicesClockService
                        .RoundPunch(roundPunchRquest, (int)ClockExceptionType.ArrivedEarly);
                    if (roundedInEarlyPunch <= DateTime.Now.ToNoDateSelected())
                    {
                        result.IsAllowed = false;
                        result.Message = $"Your punch was not accepted, you will be able to punch at " +
                                         $"{scheduleResult.StartTime.AddMinutes(rules.InEarlyAllowPunchTime * -1)}";
                    }
                }
                else if (rules.InLateAllowPunchTime != 0 &&
                         !rules.AllowInputPunches &&
                         CheckForException(ClockExceptionType.Tardy, punchTime, scheduleResult.StartTime, scheduleResult.StopTime))
                {
                    var roundedInLatePunc = _dataServicesClockService
                        .RoundPunch(roundPunchRquest, (int) ClockExceptionType.Tardy); 
                    if (roundedInLatePunc <= DateTime.Now.ToNoDateSelected())
                    {
                        result.IsAllowed = false;
                        result.Message = "Your punch was not accepted, please see your supervisor";
                    }
                }
            }
            return result;
        }

        private bool CheckForException(ClockExceptionType exception, DateTime punchTime, DateTime startTime,
            DateTime stopTime)
        {
            switch (exception)
            {
                case ClockExceptionType.ArrivedEarly:
                    return punchTime < startTime;
                case ClockExceptionType.Tardy:
                    return punchTime > startTime;
                case ClockExceptionType.LeftEarly:
                    return punchTime < stopTime;
                case ClockExceptionType.LeftLate:
                    return punchTime > stopTime;
                default:
                    return false;
            }
        }

        private DateTime GetTimeByEmployeeTimeZone(EmployeeDto emp, DateTime punchTime)
        {
            var empTimeZone = emp.ClockEmployee.TimePolicy.TimeZone;

            var standardUtc = _session.UnitOfWork.TimeClockRepository.GetTimeZones()
                    .FirstOrDefault(t => (TimeZoneType)t.TimeZoneName == TimeZoneType.Eastern)?.Utc ?? 0;
            var employeeUtc = _session.UnitOfWork.TimeClockRepository.GetTimeZones()
                    .FirstOrDefault(t => t.TimeZoneId.Equals(empTimeZone.TimeZoneId))?.Utc ?? 0;

            var utcChange = employeeUtc - standardUtc;

            if (empTimeZone.TimeZoneName != TimeZoneType.Eastern)//Is in different TimeZone
            {
                var dst = _session.UnitOfWork.TimeClockRepository
                                          .DayListSavingsTimeQuery().ForYear(punchTime).ExecuteQuery().First();

                if (standardUtc > employeeUtc) //Behind Dominion Time
                {
                    //First Date of DST
                    if (punchTime.Date == dst.BeginDate.Date && punchTime.AddHours(-1) >= dst.BeginDate)
                    {
                        return punchTime.AddHours(utcChange + 1) >= dst.BeginDate
                            ? punchTime.AddHours((utcChange + 1) - 1)
                            : punchTime.AddHours(utcChange - 1);
                    }
                    //Last Date of DST
                    if (punchTime.Date == dst.EndDate.Date && punchTime.AddHours(1) >= dst.EndDate)
                    {
                        return punchTime.AddHours(utcChange + 1) >= dst.EndDate
                            ? punchTime.AddHours((utcChange + 1) - 1)
                            : punchTime.AddHours(utcChange + 1);
                    }
                }
                else //Ahead of Dominion Time
                {
                    //First Date of DST
                    if (punchTime < dst.BeginDate
                        && punchTime.AddHours(utcChange).Date == dst.BeginDate.Date
                        && punchTime.AddHours(utcChange) >= dst.BeginDate)
                    {
                        return punchTime.AddHours(utcChange + 1);
                    }
                    //Last Date of DST
                    if (punchTime < dst.EndDate
                        && punchTime.AddHours(utcChange).Date == dst.EndDate.Date
                        && punchTime.AddHours(utcChange) >= dst.BeginDate)
                    {
                        return punchTime.AddHours(utcChange - 1);
                    }
                }
            }
            return punchTime.AddHours(utcChange);//Not during DST
        }

        /// <summary>
        ///  Replaces SPROC's GetClockEmployeePunchListByDateAndTime and GetClockEmployeePunchListByDate
        ///  Uses the employee rules to locate any punches that apply to their current shift
        /// </summary>
        /// <param name="rules"></param>
        /// <param name="punches"></param>
        /// <param name="punchTime"></param>
        /// <returns></returns>
        private IEnumerable<TPunch> GetShiftPunchList<TPunch>(IHasTimePolicyRuleShiftSettings rules, IEnumerable<TPunch> punches, DateTime punchTime) where TPunch : IHasPunchShiftDateInfo
        {
            var startSearchTime = punchTime.Date;
            var endSearchTime = punchTime.Date;
            var punchList = punches.ToOrNewList();

            IEnumerable<TPunch> result = new List<TPunch>();

            // 4 => Split by Midnight Hours
            // I think we will need some major changes to make this 100% accurate and able to find shift starts and ends.
            // For now, I am leaving Split by midnight alone and letting it function as it did
            // before since it was right most of the time. 
            if (rules.ApplyHoursOption == 4)
            {
                startSearchTime = punchTime.AddDays(-1).Date;

                result = punchList.RemoveWhere(p =>
                {
                    DateTime date = p.ShiftDate?.Date ?? p.ModifiedPunch;

                    return date < startSearchTime.Date || p.ModifiedPunch > punchTime;
                });
            } 

            // All other apply hours options and max shift is valid
            else if (rules.MaxShift != null)
            {

                if (punchList.Count > 0)
                {
                    //  Our punches don't have any data on them to tell us which ones are tied together by a shift.
                    // So we have to figure it out. We start by getting all the punches applied to today and yesterday.
                    // "Applied to" means the shift date is today or yesterday, or the shift date is null and the modified
                    // punch is today or yesterday. This should prevent us from starting our search in the middle of a shift.
                    var filteredPunches = punchList.Where(p =>
                        (p.ShiftDate ?? p.ModifiedPunch).Date >= punchTime.AddDays(-1).Date &&
                        (p.ShiftDate ?? p.ModifiedPunch).Date <= punchTime.Date
                    ).OrderBy(p => p.ModifiedPunch).ThenBy(p => p.ShiftDate).ToList();

                    if (filteredPunches.Count > 0)
                    {
                        // We know that our first punch is the start of a shift because of how we created filtered punches.
                        // And we know that shift will end after the maxshift.
                        startSearchTime = filteredPunches.First().ModifiedPunch;
                        endSearchTime = startSearchTime.AddHours((double)rules.MaxShift.Value);

                        // Now we go through each punch in order and when we find one that is after the current endSearchTIme, it should 
                        // be the start of a new shift. We can reset the start and end times and keep looping.
                        for (int i = 0; i < filteredPunches.Count; i++)
                        {
                            var punch = filteredPunches[i];

                            if (punch.ModifiedPunch > endSearchTime)
                            {
                                startSearchTime = punch.ModifiedPunch;
                                endSearchTime = startSearchTime.AddHours((double)rules.MaxShift.Value);
                            }
                        }
                    }
                }

                result = punchList.RemoveWhere(p =>
                {
                    return p.ModifiedPunch < startSearchTime || p.ModifiedPunch > endSearchTime || (DateTime.Now > endSearchTime && punchTime > endSearchTime);
                });

            } 

            return result.OrderBy(p => p.ModifiedPunch).ThenBy(p => p.TransferOption).ToList();
        }

        #endregion

        public RealTimePunchResult ProcessRealTimePunchInternal(
            int clientId,
            int employeeId,
            int? costCenterId,
            int? departmentId,
            int? divisionId,
            bool isOutPunch,
            string employeeComment,
            string comment = null,
            int? clockHardwareId = null,
            string clockName = null,
            int? clockClientLunchID = null,
            DateTimeOffset? overridePunchTime = null,
            int? overrideLunchBreak = null,
            int? jobCostingAssignmentId1 = null,
            int? jobCostingAssignmentId2 = null,
            int? jobCostingAssignmentId3 = null,
            int? jobCostingAssignmentId4 = null,
            int? jobCostingAssignmentId5 = null,
            int? jobCostingAssignmentId6 = null,
            int? punchLocationID = null,
            int? clientMachineId = null)
        {
            var result = new RealTimePunchResult();

            clockClientLunchID = CheckForNull(clockClientLunchID);
            costCenterId = CheckForNull(costCenterId);
            departmentId = CheckForNull(departmentId);
            divisionId = CheckForNull(divisionId);
            jobCostingAssignmentId1 = CheckForNull(jobCostingAssignmentId1);
            jobCostingAssignmentId2 = CheckForNull(jobCostingAssignmentId2);
            jobCostingAssignmentId3 = CheckForNull(jobCostingAssignmentId3);
            jobCostingAssignmentId4 = CheckForNull(jobCostingAssignmentId4);
            jobCostingAssignmentId5 = CheckForNull(jobCostingAssignmentId5);
            jobCostingAssignmentId6 = CheckForNull(jobCostingAssignmentId6);
            punchLocationID = CheckForNull(punchLocationID);

            var startDate = DateTime.Today.Date.AddDays(-2);
            var endDate = DateTime.Today.ToEndOfDay();

            if (overridePunchTime != null)
            {
                startDate = overridePunchTime.Value.Date.AddDays(-2);
                endDate = overridePunchTime.Value.Date.ToEndOfDay();
            }

            //var employee = _session.UnitOfWork.EmployeeRepository.GetEmployee(id: employeeId);
            var employeeResult = Self.GetEmployeeWithPunchesByEmployeeId(employeeId, startDate, endDate);

            if (employeeResult.HasError)
                return new RealTimePunchResult() { Succeeded = false, Message = "Unable to locate Employee." };

            var employee = employeeResult.Data;

            var empJobCostingDto = CalculateJobCosting(
                    clientId,
                    jobCostingAssignmentId1,
                    jobCostingAssignmentId2,
                    jobCostingAssignmentId3,
                    jobCostingAssignmentId4,
                    jobCostingAssignmentId5,
                    jobCostingAssignmentId6);
            // Use the supplied cost center/department/division first, then use job costing, then employee default "home".
            var defaultCostCenterId = CalculateDefaultCostCenterId(costCenterId, empJobCostingDto, employee);
            var defaultDivisionId = CalculateDefaultDivisionId(divisionId, empJobCostingDto, employee);
            var defaultDepartmentId = CalculateDefaultDepartmentId(departmentId, empJobCostingDto, employee);

            var employeeLocalTime = ConvertToEmployeeLocalTime(employee: employee, serverTime: GetServerTime());
            var overridePunchLocalTime = overridePunchTime.HasValue ? (DateTime?)ConvertToEmployeeLocalTime(employee: employee, overridePunchTime: overridePunchTime.Value) : null;
            var punchTime = (overridePunchLocalTime ?? employeeLocalTime).RoundDownToMinute();

            employee.ClockEmployee.Punches = GetShiftPunchList(employee.ClockEmployee.TimePolicy.Rules, employee.ClockEmployee.Punches, punchTime).ToOrNewList();
            
            if (overrideLunchBreak == 3)
            {
                employee.ClockEmployee.TimePolicy.Lunches = employee.ClockEmployee.TimePolicy.Lunches
                .RemoveWhere(l => l.PunchType != 2).ToList();
                clockClientLunchID = employee.ClockEmployee.TimePolicy.Lunches.FirstOrDefault()?.ClockClientLunchId;
            }
            if (overrideLunchBreak == 2)
            {
                employee.ClockEmployee.TimePolicy.Lunches = employee.ClockEmployee.TimePolicy.Lunches
                .RemoveWhere(l => l.PunchType != 1).ToList();
                clockClientLunchID = employee.ClockEmployee.TimePolicy.Lunches.FirstOrDefault()?.ClockClientLunchId;
            }
            
            employee.ClockEmployee.TimePolicy.Lunches = employee.ClockEmployee.TimePolicy.Lunches
                .RemoveWhere(l => l.ClockClientLunchId != clockClientLunchID).ToList();

            var lastPunch = _session.UnitOfWork.TimeClockRepository.ClockEmployeeLastPunchSproc(
                clientId:                clientId,
                employeeId:              employeeId,
                punchDateTime:           punchTime,
                costCenterId:            CheckForNull(defaultCostCenterId),
                divisionId:              CheckForNull(defaultDivisionId),
                departmentId:            CheckForNull(defaultDepartmentId),
                jobCostingAssignment1Id: CheckForNull(empJobCostingDto.ClientJobCostingAssignmentId1),
                jobCostingAssignment2Id: CheckForNull(empJobCostingDto.ClientJobCostingAssignmentId2),
                jobCostingAssignment3Id: CheckForNull(empJobCostingDto.ClientJobCostingAssignmentId3),
                jobCostingAssignment4Id: CheckForNull(empJobCostingDto.ClientJobCostingAssignmentId4),
                jobCostingAssignment5Id: CheckForNull(empJobCostingDto.ClientJobCostingAssignmentId5),
                jobCostingAssignment6Id: CheckForNull(empJobCostingDto.ClientJobCostingAssignmentId6),
                clockClientLunchId:      CheckForNull(clockClientLunchID));

            var minutesToRestrictLunchPunch = employee.ClockEmployee?.TimePolicy?.Lunches?.FirstOrDefault(x => x.ClockClientLunchId == clockClientLunchID)?.MinutesToRestrictLunchPunch ?? 0;
            
            if (PunchCanBeLunchRestricted(isOutPunch, clockHardwareId, clockClientLunchID, clientMachineId, minutesToRestrictLunchPunch))
            {
                if (PreviousPunchIsLunchPunch(clockClientLunchID, employee, lastPunch?.ClockClientLunchId ?? 0))
                {
                    var startTime = lastPunch.ModifiedPunch;
                    var endTime = lastPunch.ModifiedPunch.AddMinutes(minutesToRestrictLunchPunch);
                    if (TimeFrame.IsBetweenTimeFrame(startTime, endTime, punchTime))
                    {
                        return new RealTimePunchResult {Succeeded = false, Message = $"Punch not recorded. Minimum lunch length has not been met.{Environment.NewLine}Please try again at {endTime:h:mm tt}."};
                    }
                }
            }

            CheckPunchAllowedResult punchAllowedResult = null;
            if (!(lastPunch?.IsTransferPunch).GetValueOrDefault() && clockClientLunchID == null)
            {
                //normal punch, not a transfer
                Debug.WriteLine(message: $"STARTING CHECK PUNCH ALLOWED {DateTime.Now}");
                punchAllowedResult = CheckPunchAllowed(employeeId: employeeId, punchTime: punchTime);
                Debug.WriteLine(message: $"END CHECK PUNCH ALLOWED {DateTime.Now}");
            }

            if (punchAllowedResult?.IsAllowed ?? true) //If Punch is a transfer, null coalesce with true (always punch for transfers)
            {
                //var punchHelper = new dsClockEmployeePunch();

                #region Call Into InsertClockEmployeePunch and Calculate Weekly Activity

                var punchArgs = new PunchScreenPunchRequestArgs()
                {
                    ClientCostCenterId    = CalculatePunchCostCenterId(isOutPunch, lastPunch, defaultCostCenterId),
                    ClientDepartmentId    = CalculatePunchDepartmentId(isOutPunch, lastPunch, defaultDepartmentId),
                    ClientDivisionId      = CalculatePunchDivisionId(isOutPunch, lastPunch, defaultDivisionId),
                    ClientShiftId         = NullId,
                    ClockClientLunchId    = clockClientLunchID ?? NullId,
                    ClockName             = clockName,
                    ClockClientHardwareId = clockHardwareId ?? NullId,
                    EmployeeId            = employeeId,
                    EmployeeComment       = employeeComment,
                    Comment               = comment,
                    ShouldStopAutoLunch   = false,
                    IsPaid                = employee.ClockEmployee.TimePolicy.Lunches.FirstOrDefault()?.IsPaid ?? false,
                    RawPunchTime          = punchTime,
                    TimeZoneId            = employee.ClockEmployee.TimePolicy.TimeZone.TimeZoneId,
                    ModifiedPunch         = punchTime,
                    ModifiedBy            = _session.LoggedInUserInformation.IsAnonymous ? (employee.UserId ?? 0) : _session.LoggedInUserInformation.UserId ,
                    TransferOption        = (lastPunch?.IsTransferPunch).GetValueOrDefault() ? 1 : NullId,
                    EmployeeJobCosting    = CalculatePunchJobCosting(empJobCostingDto, isOutPunch, lastPunch),
                    PunchLocationID       = punchLocationID,
                    ClientMachineId       = clientMachineId
                };

                if (punchArgs.ClientCostCenterId == employee.ClientCostCenterId)
                    punchArgs.ClientCostCenterId = NullId;

                var clockEmployeePunchId = _dataServicesClockService
                    .InsertClockEmployeePunch_PunchScreen(punchArgs);

                var shouldRoundAllPunches = true;
                bool isDuplicatePunch = false;
                var isSuccessfulPunch = clockEmployeePunchId > 0;

                if (isSuccessfulPunch) //Punch Succeeded
                {
                    var hasMoreThanOnePunchThisShift = employee.ClockEmployee.Punches.Count > 0;
                    if (isOutPunch && lastPunch != null && !employee.ClockEmployee.TimePolicy.Rules.IsHideJobCosting && hasMoreThanOnePunchThisShift)
                    {
                        empJobCostingDto = CalculateJobCosting(
                            clientId,
                            jobCostingAssignmentId1,
                            jobCostingAssignmentId2,
                            jobCostingAssignmentId3,
                            jobCostingAssignmentId4,
                            jobCostingAssignmentId5,
                            jobCostingAssignmentId6);
                    }

                    var estimatedOutPunch = employee.ClockEmployee.Punches.Count() % 2 != 0;
                    //is a regular punch and is a transfer and is not using "input punches"
                    if (ShouldAddTransferPunch(isOutPunch, clockClientLunchID, lastPunch, hasMoreThanOnePunchThisShift, employee.ClockEmployee.TimePolicy.Rules, empJobCostingDto, 
                        defaultCostCenterId, defaultDepartmentId, defaultDivisionId) && estimatedOutPunch)
                    {
                        punchArgs.ClientCostCenterId = CalculateTransferPunchCostCenterId(defaultCostCenterId);
                        punchArgs.ClientDivisionId   = CalculateTransferPunchDivisionId(defaultDivisionId);
                        punchArgs.ClientDepartmentId = CalculateTransferPunchDepartmentId(defaultDepartmentId);
                        punchArgs.ClockClientLunchId = NullId;
                        punchArgs.TransferOption     = 2;
                        punchArgs.EmployeeJobCosting = empJobCostingDto;

                        // then because it is a transfer, we need to insert another punch because they just checked into the new location.
                        // the new location is represented by the requested cost center, division, and department IDs, which would not have been used for the 
                        // previous punch above due to it being an out punch.
                        result.TransferPunchId = _dataServicesClockService
                            .InsertClockEmployeePunch_PunchScreen(punchArgs);
                     
                        shouldRoundAllPunches = false;
                    }
                    else if (clockClientLunchID != null)
                    {
                        shouldRoundAllPunches = employee.ClockEmployee.TimePolicy.Lunches.FirstOrDefault().AllPunchesClockRoundingTypeId != null;
                    }

                    // TODO: Find Shift
                    var calculateWeeklyActivityArgs = new CalculateWeeklyActivityRequestArgs()
                    {
                        ClientId = employee.ClientId,
                        ClockEmployeePunchId = clockEmployeePunchId,
                        DatePunch = punchTime,
                        EmployeeId = employee.EmployeeId,
                        ShouldIncludeAutoLunch = true,
                        ShouldRoundPunch = shouldRoundAllPunches
                    };

                    isDuplicatePunch = _dataServicesClockService
                        .CalculateWeeklyActivity(calculateWeeklyActivityArgs, _clientSerivce);

                    if (isDuplicatePunch)
                        _dataServicesClockService.DeleteClockEmployeePunch(clockEmployeePunchId, _session.LoggedInUserInformation.UserId);
                }

                #endregion

                result.PunchId = isSuccessfulPunch ? (int?)clockEmployeePunchId : null;
                result.IsDuplicatePunch = isDuplicatePunch;
                result.Succeeded = isSuccessfulPunch && !isDuplicatePunch;
                result.PunchTime = punchTime;
                result.Message = isSuccessfulPunch && !isDuplicatePunch //TODO: find a way to add timezone abbreviation
                    ? $"Your Punch Has Been Recorded on {punchTime.ToShortDateString()} {punchTime.ToShortTimeString()}"
                    : $"Your Punch at {punchTime.ToShortDateString()} {punchTime.ToShortTimeString()} has already been recorded";
                    // THIS WAS PREVIOUSLY USED TO DISPLAY PREVIOUSLY RECORDED TIMES, THIS WAS SHOWING THE MOST
                    // RECENT PUNCH AND NOT THE ACTUAL PUNCH THAT WAS INPUT
                    //: $"Your Punch at {lastPunch.ModifiedPunch.ToShortDateString()} {lastPunch.ModifiedPunch.ToShortTimeString()} has already been recorded";
            }
            else if (punchAllowedResult != null)
            {
                result.Succeeded = false;
                result.Message = punchAllowedResult.Message;
            }

            // This is required to ensure that iPads delete the punches that fail. 
            if (!result.Succeeded) result.IsDuplicatePunch = true;

            if (result.IsDuplicatePunch)
            {
                var client = _session.UnitOfWork.ClientRepository
                    .QueryClients()
                    .ByClientId(clientId)
                    .ExecuteQueryAs(x => new
                    {
                        x.ClientCode
                    })
                    .FirstOrDefault();
                
                var lostPunch = new ClockEmployeeLostPunch()
                {
                    ClientId = clientId,
                    CompanyIdentifier = client?.ClientCode,
                    BadgeNumber = employee.EmployeeNumber,
                    ClockName = clockName,
                    EmployeeId = employee.EmployeeId,
                    ClientCostCenterId = lastPunch.ClientCostCenterId,
                    CostCenterCode = "",
                    RawPunch = punchTime,
                    Modified = DateTime.Now
                };
                
                RegisterLostPunch(lostPunch);
            }
            
            return result;
        }

        private void RegisterLostPunch(ClockEmployeeLostPunch punch)
        {
            var result = new OpResult();
            
            _session.UnitOfWork.RegisterNew(punch);
            
            _session.UnitOfWork.Commit().MergeInto(result);

            if (result.HasError)
            {
                // LOG THIS ERROR TO SEQ
                _logger.Error("Failed to save lost punch.", result);
            }
        }

        private static bool PunchCanBeLunchRestricted(bool isOutPunch, int? clockHardwareId, int? clockClientLunchId, int? clientMachineId, int minutesToRestrictLunchPunch)
        {
            return clockClientLunchId != null && minutesToRestrictLunchPunch > 0 && !isOutPunch && clockHardwareId == null && clientMachineId == null;
        }

        private static bool PreviousPunchIsLunchPunch(int? clockClientLunchIdInput, EmployeeDto employee, int? lastPunchClockClientLunchId)
        {
            return employee.ClockEmployee.Punches.Any() && lastPunchClockClientLunchId > 0 && lastPunchClockClientLunchId == clockClientLunchIdInput;
        }

        /// <summary>
        /// Determines whether to add a transfer punch based on the given values.
        /// </summary>
        /// <param name="isOutPunch">Whether or not the punch is an out punch.</param>
        /// <param name="punchTypeId">The ID of the punch type. Nullable.</param>
        /// <param name="lastPunch">The last punch. Nullable.</param>
        /// <param name="hasMoreThanOnePunchThisShift">Whether the employee has more than one punch in the shift.</param>
        /// <param name="rules">The clock rules that are being used.</param>
        /// <param name="empJobCosting">Job Costing from current punch.</param>
        /// <param name="clientCostCenterId">The cost center of the current punch.</param>
        /// <param name="clientDepartmentId">The department of the current punch.</param>
        /// <param name="clientDivisionId">The division of the current punch.</param>
        /// <returns></returns>
        public static bool ShouldAddTransferPunch(bool isOutPunch, int? punchTypeId, ClockEmployeeLastPunchDto lastPunch, bool hasMoreThanOnePunchThisShift, ClockClientRulesDto rules, 
            EmployeeJobCostingDto empJobCosting, int? clientCostCenterId, int? clientDepartmentId, int? clientDivisionId)
        {
            var jobCostingCausesTransfer = false;
            if (!rules.IsHideJobCosting)
            {
                if ((empJobCosting.UsesCostCenter && clientCostCenterId.GetValueOrDefault() != lastPunch?.ClientCostCenterId.GetValueOrDefault()) ||
                    (empJobCosting.UsesDepartment && clientDepartmentId.GetValueOrDefault() != lastPunch?.ClientDepartmentId.GetValueOrDefault()) ||
                    (empJobCosting.UsesDivision && clientDivisionId.GetValueOrDefault() != lastPunch?.ClientDivisionId.GetValueOrDefault()) ||
                    empJobCosting.ClientJobCostingAssignmentId1.GetValueOrDefault() != lastPunch?.JobCostingAssignment1.GetValueOrDefault() ||
                    empJobCosting.ClientJobCostingAssignmentId2.GetValueOrDefault() != lastPunch?.JobCostingAssignment2.GetValueOrDefault() ||
                    empJobCosting.ClientJobCostingAssignmentId3.GetValueOrDefault() != lastPunch?.JobCostingAssignment3.GetValueOrDefault() ||
                    empJobCosting.ClientJobCostingAssignmentId4.GetValueOrDefault() != lastPunch?.JobCostingAssignment4.GetValueOrDefault() ||
                    empJobCosting.ClientJobCostingAssignmentId5.GetValueOrDefault() != lastPunch?.JobCostingAssignment5.GetValueOrDefault() ||
                    empJobCosting.ClientJobCostingAssignmentId6.GetValueOrDefault() != lastPunch?.JobCostingAssignment6.GetValueOrDefault()
                ) jobCostingCausesTransfer = true;
            }
            return !rules.AllowInputPunches && 
                punchTypeId == null &&
                (lastPunch?.IsTransferPunch).GetValueOrDefault() &&
                hasMoreThanOnePunchThisShift &&
                isOutPunch &&
                (clientCostCenterId.GetValueOrDefault() != lastPunch?.ClientCostCenterId.GetValueOrDefault() ||
                jobCostingCausesTransfer);
        }

        IOpResult<ClockCostCenterRequirementType> IEmployeePunchProvider.GetClockCostCenterRequirementClientOption(int clientId)
        {
            var result = new OpResult<ClockCostCenterRequirementType>();
            var clientOptions = _clientSettingProvider.GetClientAccountOptionSettings(clientId, AccountOption.TimeClock_RequireEmployeeToPickCostCenter).MergeInto(result).Data;

            var costCenterOption = clientOptions?.FirstOrDefault(x => x.AccountOption == AccountOption.TimeClock_RequireEmployeeToPickCostCenter);
            var costCenterOptionType = costCenterOption?.SelectedItem != null ? (ClockCostCenterRequirementType)costCenterOption.SelectedItem.AccountOptionItemId : ClockCostCenterRequirementType.Never;

            return result.SetDataOnSuccess(costCenterOptionType);
        }

        IOpResult<EmployeeClockConfiguration> IEmployeePunchProvider.GetEmployeeClockConfiguration(int employeeId, DateTime punchHistoryStartTime, DateTime punchHistoryEndTime)
        {
            return new OpResult<EmployeeClockConfiguration>().TrySetData(() => _session.UnitOfWork.EmployeeRepository
                .QueryEmployees()
                .ByEmployeeId(employeeId)
                .ExecuteQueryAs(e => new EmployeeClockConfiguration
                {
                    EmployeeId         = employeeId,
                    ClientId           = e.ClientId,
                    FirstName          = e.FirstName,
                    LastName           = e.LastName,
                    MiddleInitial      = e.MiddleInitial,
                    EmployeeNumber     = e.EmployeeNumber,
                    HomeCostCenterId   = e.ClientCostCenterId,
                    HomeDepartmentId   = e.ClientDepartmentId,
                    HomeDivisionId     = e.ClientDivisionId,

                    ClockEmployee = e.ClockEmployee != null ? new EmployeeClockSetupConfiguration
                    {
                        BadgeNumber = e.ClockEmployee.BadgeNumber,
                        TimePolicy = e.ClockEmployee.TimePolicy != null ? new EmployeeTimePolicyConfiguration
                        {
                            ClockClientTimePolicyId = e.ClockEmployee.TimePolicy.ClockClientTimePolicyId,
                            Name = e.ClockEmployee.TimePolicy.Name,
                            Rules = e.ClockEmployee.TimePolicy.Rules != null ? new EmployeeTimePolicyRuleConfiguration
                            {
                                ClockClientRulesId  = e.ClockEmployee.TimePolicy.Rules.ClockClientRulesId,
                                Name                = e.ClockEmployee.TimePolicy.Rules.Name,
                                IsHideCostCenter    = e.ClockEmployee.TimePolicy.Rules.IsHideCostCenter,
                                IsHideDepartment    = e.ClockEmployee.TimePolicy.Rules.IsHideDepartment,
                                IsHideJobCosting    = e.ClockEmployee.TimePolicy.Rules.IsHideJobCosting,
                                IsHideEmployeeNotes = e.ClockEmployee.TimePolicy.Rules.IsHideEmployeeNotes,
                                IsHidePunchType     = e.ClockEmployee.TimePolicy.Rules.IsHidePunchType,
                                IsHideShift         = e.ClockEmployee.TimePolicy.Rules.IsHideShift,
                                MaxShift            = e.ClockEmployee.TimePolicy.Rules.MaxShift,
                                ApplyHoursOption    = e.ClockEmployee.TimePolicy.Rules.ApplyHoursOption,
                                AllowInputPunches   = e.ClockEmployee.TimePolicy.Rules.AllowInputPunches,
                                AllowMobilePunching = e.ClockEmployee.TimePolicy.Rules.IsAllowMobilePunch,
                                PunchOption         = e.ClockEmployee.TimePolicy.Rules.PunchOption != null ? e.ClockEmployee.TimePolicy.Rules.PunchOption : PunchOptionType.NormalPunch,
                                StartDayOfWeek      = e.ClockEmployee.TimePolicy.Rules.WeeklyStartingDayOfWeekId != null ? e.ClockEmployee.TimePolicy.Rules.WeeklyStartingDayOfWeekId : (byte?)DayOfWeek.Sunday,
                                InEarlyGraceTime    = e.ClockEmployee.TimePolicy.Rules.InEarlyGraceTime,
                                OutLateGraceTime    = e.ClockEmployee.TimePolicy.Rules.OutLateGraceTime
                            } : null,

                            Lunches = e.ClockEmployee.TimePolicy.LunchSelected.Select(l => new EmployeeTimePolicyLunchConfiguration
                            {
                                ClockClientLunchId = l.ClockClientLunchId,
                                ClientCostCenterId = l.Lunch.ClientCostCenterId,
                                Name        = l.Lunch.Name,
                                StartTime   = l.Lunch.StartTime,
                                StopTime    = l.Lunch.StopTime,
                                IsSunday    = l.Lunch.IsSunday,
                                IsMonday    = l.Lunch.IsMonday,
                                IsTuesday   = l.Lunch.IsTuesday,
                                IsWednesday = l.Lunch.IsWednesday,
                                IsThursday  = l.Lunch.IsThursday,
                                IsFriday    = l.Lunch.IsFriday,
                                IsSaturday  = l.Lunch.IsSaturday
                            }).ToList()
                        } : null,
                        
                        Punches = e.Punches.Select(p => new EmployeeRecentPunchInfo
                        {
                            ClockEmployeePunchId          = p.ClockEmployeePunchId,
                            ModifiedPunch                 = p.ModifiedPunch,
                            RawPunch                      = p.RawPunch,
                            ShiftDate                     = p.ShiftDate,
                            ClientCostCenterId            = p.ClientCostCenterId,
                            ClientDepartmentId            = p.ClientDepartmentId,
                            ClientDivisionId              = p.ClientDivisionId,
                            ClockClientLunchId            = p.ClockClientLunchId,
                            TransferOption                = p.TransferOption,
                            ClientJobCostingAssignmentId1 = p.ClientJobCostingAssignmentId1,
                            ClientJobCostingAssignmentId2 = p.ClientJobCostingAssignmentId2,
                            ClientJobCostingAssignmentId3 = p.ClientJobCostingAssignmentId3,
                            ClientJobCostingAssignmentId4 = p.ClientJobCostingAssignmentId4,
                            ClientJobCostingAssignmentId5 = p.ClientJobCostingAssignmentId5,
                            ClientJobCostingAssignmentId6 = p.ClientJobCostingAssignmentId6
                        }).Where(p => p.ModifiedPunch >= punchHistoryStartTime && p.ModifiedPunch <= punchHistoryEndTime).ToList()
                    } : null,

                })
                .FirstOrDefault()
            );
        }

        private IOpResult DeletePunchFromResult(RealTimePunchResult result)
        {
            return result?.PunchId != null ?
                DeletePunch(result.PunchId.Value, result.PunchTime) :
                new OpResult();
        }

        private IOpResult DeleteTransferPunchFromResult(RealTimePunchResult result)
        {
            return result?.TransferPunchId != null ?
                DeletePunch(result.TransferPunchId.Value, result.PunchTime) :
                new OpResult();
        }

        /// <summary>
        /// Makes sure that some data is not different between the two given punches.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static RealTimePunchRequest CleanPunch(RealTimePunchRequest first, RealTimePunchRequest second)
        {
            return new RealTimePunchRequest()
            {
                EmployeeId = first.EmployeeId,
                ClientId = first.ClientId,
                CostCenterId = first.CostCenterId,
                DepartmentId = first.DepartmentId,
                JobCostingAssignmentId1 = first.JobCostingAssignmentId1,
                JobCostingAssignmentId2 = first.JobCostingAssignmentId2,
                JobCostingAssignmentId3 = first.JobCostingAssignmentId3,
                JobCostingAssignmentId4 = first.JobCostingAssignmentId4,
                JobCostingAssignmentId5 = first.JobCostingAssignmentId5,
                JobCostingAssignmentId6 = first.JobCostingAssignmentId6,
                EmployeeComment = second.EmployeeComment,
                OverridePunchTime = second.OverridePunchTime,
                PunchTypeId = second.PunchTypeId,
                IsOutPunch = true,
                ClockName = second.ClockName,
                PunchLocation = first.PunchLocation
            };
        }

        private static RealTimePunchPairResult GetFailedPunchPairResult(
            RealTimePunchResult failed,
            RealTimePunchResult first,
            RealTimePunchResult second,
            RealTimePunchRequest punch)
        {
            return new RealTimePunchPairResult()
            {
                Message = failed != null ?
                    failed.Message :
                    $"Your punch at {(punch.OverridePunchTime ?? DateTime.Now.RoundDownToMinute()).ToShortTimeString()} failed.",
                Succeeded = false,
                First = first,
                Second = second
            };
        }

        #region PRIVATE METHODS

        private IOpResult DeletePunch(int punchId, DateTime punchDate)
        {
            var result = new OpResult();
            return result.TryCatch(() => DeletePunchInternal(punchId, punchDate));
        }

        private void DeletePunchInternal(int punchId, DateTime punchDate)
        {
            var userId = _session.LoggedInUserInformation.UserId;

            _dataServicesClockService.DeleteClockEmployeePunch(punchId, userId);

            IEnumerable<ClockExceptionType> geoExceptions = new List<ClockExceptionType>() { ClockExceptionType.NoLocation, ClockExceptionType.BadLocation };

            //DELETE GEOFENCE CLOCK EXCEPTION
            _clockEmployeeExceptionService.DeleteClockEmployeeException(geoExceptions, punchId);

            //Delete job costing if necessary
            _dataServicesClockService.DeleteEmployeeJobCosting(punchId, userId);

            // Recalculate employee activity rrice 6/16/2008
            var requestArgs = new CalculateWeeklyActivityRequestArgs()
            {
                ClientId = _session.LoggedInUserInformation.ClientId.Value,
                EmployeeId = _session.LoggedInUserInformation.EmployeeId.Value,
                DatePunch = punchDate,
                ShouldIncludeAutoLunch = false,
                ShouldRoundPunch = false,
                ClockEmployeePunchId = NullId
            };

            _dataServicesClockService.CalculateWeeklyActivity(requestArgs, _clientSerivce);
        }

        private int? CheckForNull(int? value)
        {
            if (value.HasValue)
            {
                if (value < 1) value = null;                
            }
            return value;
        }

        private static int CalculateTransferPunchDepartmentId(int? defaultDepartmentId)
        {
            return defaultDepartmentId ?? NullId;
        }

        private static int CalculateTransferPunchDivisionId(int? defaultDivisionId)
        {
            return defaultDivisionId ?? NullId;
        }

        private static int CalculateTransferPunchCostCenterId(int? defaultCostCenterId)
        {
            return defaultCostCenterId ?? NullId;
        }

        public static int CalculatePunchDepartmentId(bool isOutPunch, ClockEmployeeLastPunchDto lastPunch, int? defaultDepartmentId)
        {
            return CalculateAttributePunchId(isOutPunch, lastPunch, defaultDepartmentId, lastPunch?.ClientDepartmentId);
        }

        public static int CalculatePunchDivisionId(bool isOutPunch, ClockEmployeeLastPunchDto lastPunch, int? defaultDivisionId)
        {
            return CalculateAttributePunchId(isOutPunch, lastPunch, defaultDivisionId, lastPunch?.ClientDivisionId);
        }

        public static int CalculatePunchCostCenterId(bool isOutPunch, ClockEmployeeLastPunchDto lastPunch, int? defaultCostCenterId)
        {
            return CalculateAttributePunchId(isOutPunch, lastPunch, defaultCostCenterId, lastPunch?.ClientCostCenterId);
        }

        /// <summary>
        /// Calculates an ID for common punch ID relational attributes such as Cost Center, Division, and Department.
        /// </summary>
        /// <param name="isOutPunch"></param>
        /// <param name="lastPunch"></param>
        /// <param name="defaultCostCenterId"></param>
        /// <param name="lastPunchRelationId"></param>
        /// <returns></returns>
        public static int CalculateAttributePunchId(bool isOutPunch, ClockEmployeeLastPunchDto lastPunch, int? defaultCostCenterId, int? lastPunchRelationId)
        {
            return (isOutPunch && lastPunch != null
                ? lastPunchRelationId
                : defaultCostCenterId)
                ?? NullId;
        }

        public static int? CalculateDefaultDepartmentId(int? departmentId, EmployeeJobCostingDto empJobCostingDto, EmployeeDto employee)
        {
            return departmentId ??
                (empJobCostingDto.ClientDepartmentId != 0 ?
                       empJobCostingDto.ClientDepartmentId :
                       employee.ClientDepartmentId);
        }

        public static int? CalculateDefaultDivisionId(int? divisionId, EmployeeJobCostingDto empJobCostingDto, EmployeeDto employee)
        {
            return divisionId ??
                (empJobCostingDto.ClientDivisionId != 0 ?
                       empJobCostingDto.ClientDivisionId :
                       employee.ClientDivisionId);
        }

        public static int? CalculateDefaultCostCenterId(int? costCenterId, EmployeeJobCostingDto empJobCostingDto, EmployeeDto employee)
        {
            return costCenterId ??
                (empJobCostingDto.ClientCostCenterId != 0 ?
                       empJobCostingDto.ClientCostCenterId :
                       employee.ClientCostCenterId);
        }

        private EmployeeJobCostingDto CalculateJobCosting(int clientId, params int?[] jobCostingAssignmentIds)
        {
            var final = new EmployeeJobCostingDto();

            var ids = jobCostingAssignmentIds.Where(id => id.HasValue).Select(id => id.Value).ToArray();

            if (ids.Length <= 0) return final;

            var jobCostings = _jobCostingProvider.GetClientJobCostingList(clientId);
            var jobCostingIds = jobCostings.Data.Select(jc => jc.ClientJobCostingId).ToArray();

            var assignments = _session.UnitOfWork.JobCostingRepository.GetJobCostingAssignmentQuery()
                .ByClientId(clientId)
                .ByIsEnabled(true)
                .ByClientJobCostingParents(jobCostingIds)
                .ExecuteQueryAs(x => new ClientJobCostingAssignmentDto()
                {
                    ClientJobCostingId = x.ClientJobCostingId,
                    ClientJobCostingAssignmentId = x.ClientJobCostingAssignmentId,
                    ForeignKeyId = x.ForeignKeyId
                });

            var selectedAssignments = _session.UnitOfWork.JobCostingRepository.GetJobClientJobCostingAssignmentSelectedQuery()
                .ByClientId(clientId)
                .ByIsEnabled(true)
                .BySelectedJobCostings(jobCostingIds)
                .ExecuteQueryAs(x => new ClientJobCostingAssignmentDto()
                {
                    ClientJobCostingId = x.ClientJobCostingId_Selected ?? default(int),
                    ClientJobCostingAssignmentId = x.ClientJobCostingAssignment != null ? x.ClientJobCostingAssignment.ClientJobCostingAssignmentId : default(int),
                    ForeignKeyId = x.ForeignKeyId_Selected
                });

            // combine our assignment lists to include all levels
            assignments = (assignments ?? Enumerable.Empty<ClientJobCostingAssignmentDto>())
                .Concat((selectedAssignments ?? Enumerable.Empty<ClientJobCostingAssignmentDto>()));

            if (assignments == null) return final;

            foreach (var assignId in jobCostingAssignmentIds)
            {
                if (!assignId.HasValue) continue;

                var filteredAssignment = assignments.FirstOrDefault(a => a.ForeignKeyId == assignId) ?? assignments.FirstOrDefault(x => x.ClientJobCostingAssignmentId == assignId);

                var filteredJobCosting =
                    jobCostings.Data.FirstOrDefault(jc => jc.ClientJobCostingId == filteredAssignment.ClientJobCostingId);

                if (filteredJobCosting == null) continue;

                switch (filteredJobCosting.JobCostingTypeId)
                {
                    case (int)ClientJobCostingType.CostCenter:
                        final.ClientCostCenterId = assignId.Value;
                        final.UsesCostCenter = true;
                        break;
                    case (int)ClientJobCostingType.Department:
                        final.ClientDepartmentId = assignId.Value;
                        final.UsesDepartment = true;
                        break;
                    case (int)ClientJobCostingType.Division:
                        final.ClientDivisionId = assignId.Value;
                        final.UsesDivision = true;
                        break;
                    case (int)ClientJobCostingType.Group:
                        final.ClientGroupId = assignId.Value;
                        final.UsesGroup = true;
                        break;
                    case (int)ClientJobCostingType.Custom:
                        final.UsesCustom = true;
                        if (final.ClientJobCostingId1 == 0)
                        {
                            final.ClientJobCostingId1 = filteredJobCosting.ClientJobCostingId;
                            final.ClientJobCostingAssignmentId1 = assignId.Value;
                        }
                        else if (final.ClientJobCostingId2 == 0)
                        {
                            final.ClientJobCostingId2 = filteredJobCosting.ClientJobCostingId;
                            final.ClientJobCostingAssignmentId2 = assignId.Value;
                        }
                        else if (final.ClientJobCostingId3 == 0)
                        {
                            final.ClientJobCostingId3 = filteredJobCosting.ClientJobCostingId;
                            final.ClientJobCostingAssignmentId3 = assignId.Value;
                        }
                        else if (final.ClientJobCostingId4 == 0)
                        {
                            final.ClientJobCostingId4 = filteredJobCosting.ClientJobCostingId;
                            final.ClientJobCostingAssignmentId4 = assignId.Value;
                        }
                        else if (final.ClientJobCostingId5 == 0)
                        {
                            final.ClientJobCostingId5 = filteredJobCosting.ClientJobCostingId;
                            final.ClientJobCostingAssignmentId5 = assignId.Value;
                        }
                        else
                        {
                            final.ClientJobCostingId6 = filteredJobCosting.ClientJobCostingId;
                            final.ClientJobCostingAssignmentId6 = assignId.Value;
                        }
                        break;
                }
            }
            return final;
        }

        private EmployeeJobCostingDto CalculatePunchJobCosting(EmployeeJobCostingDto punchJobCostingDto, bool isOutPunch, ClockEmployeeLastPunchDto lastPunch)
        {
            if (isOutPunch && lastPunch != null)
            {
                punchJobCostingDto.ClientCostCenterId = lastPunch.ClientCostCenterId.GetValueOrDefault();
                punchJobCostingDto.ClientDepartmentId = lastPunch.ClientDepartmentId.GetValueOrDefault();
                punchJobCostingDto.ClientDivisionId = lastPunch.ClientDivisionId.GetValueOrDefault();
                punchJobCostingDto.ClientJobCostingAssignmentId1 = lastPunch.JobCostingAssignment1;
                punchJobCostingDto.ClientJobCostingAssignmentId2 = lastPunch.JobCostingAssignment2;
                punchJobCostingDto.ClientJobCostingAssignmentId3 = lastPunch.JobCostingAssignment3;
                punchJobCostingDto.ClientJobCostingAssignmentId4 = lastPunch.JobCostingAssignment4;
                punchJobCostingDto.ClientJobCostingAssignmentId5 = lastPunch.JobCostingAssignment5;
                punchJobCostingDto.ClientJobCostingAssignmentId6 = lastPunch.JobCostingAssignment6;
            }
            return punchJobCostingDto;
        }

        private DateTime ConvertToEmployeeLocalTime(EmployeeDto employee, DateTimeOffset overridePunchTime)
        {
            var easternStandardTime = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var timeInEastern = TimeZoneInfo.ConvertTime(overridePunchTime, easternStandardTime);
            return ConvertToEmployeeLocalTime(employee, timeInEastern.DateTime);
        }

        private DateTime ConvertToEmployeeLocalTime(EmployeeDto employee, DateTime serverTime)
        {
            return GetTimeByEmployeeTimeZone(employee, serverTime);
        }

        private DateTime GetServerTime()
        {
            return _session.UnitOfWork.TimeClockRepository.GetSqlServerTimeSproc().ServerTime;
        }

        public IOpResult<CanEmployeePunchDto> CanEmployeePunchFromIp(int employeeId, string ipAddress)
        {
            var opResult = new OpResult<CanEmployeePunchDto>();

            // if a system admin ... allow punch from any IP
            var isAdmin = _session.CanPerformAction(ClockActionType.Administrator).Success;
            if (isAdmin)
                return opResult.SetDataOnSuccess(new CanEmployeePunchDto() { CanPunch = true, IpAddress = ipAddress });

            opResult.TrySetData(() =>
            {
                bool isLockout = false;
                var timePolicy = _session.UnitOfWork.TimeClockRepository.GetClockEmployeeQuery()
                    .ByEmployeeId(employeeId).Result.MapTo(x => new
                    {
                        x.TimePolicy.Rules.IsIpLockout,
                        x.Employee.ClientId
                    }).FirstOrDefault();

                isLockout = timePolicy?.IsIpLockout ?? false;
                if (isLockout)
                {
                    var results = _session.UnitOfWork.ClientRepository.QueryClientIpSecurity()
                        .ByClientId(timePolicy.ClientId)
                        .ExecuteQueryAs(ip => new
                        {
                            ip.IpAddress

                        }).ToList();

                    isLockout = results.All(x => x.IpAddress.Trim() != ipAddress.Trim());
                }

                return new CanEmployeePunchDto()
                {
                    CanPunch = !isLockout,
                    IpAddress = ipAddress
                };
            });

            return opResult;
        }

        public IOpResult<RealTimePunchPairResult> ProcessRealTimePunchPair(RealTimePunchRequest first, RealTimePunchRequest second)
        {
            first.IsOutPunch = false;
            var cleanedSecondPunch = CleanPunch(first, second);

            var result = new OpResult<RealTimePunchPairResult>();

            var firstResult = ProcessRealTimePunch(first).MergeInto(result);
            if (firstResult.Success && firstResult.Data.Succeeded && firstResult.Data.PunchId != null)
            {
                var secondResult = ProcessRealTimePunch(cleanedSecondPunch).MergeInto(result);

                if (secondResult.Success && secondResult.Data.Succeeded && secondResult.Data.PunchId != null)
                {
                    // Both passed.
                    result.Data = new RealTimePunchPairResult()
                    {
                        Message = $"Your punches at {firstResult.Data.PunchTime.ToShortTimeString()} and {secondResult.Data.PunchTime.ToShortTimeString()} were recorded.",
                        Succeeded = true,
                        First = firstResult.Data,
                        Second = secondResult.Data,
                    };
                }
                else
                {
                    // First passed but second failed.
                    DeletePunchFromResult(firstResult.Data).MergeInto(result);
                    DeleteTransferPunchFromResult(firstResult.Data).MergeInto(result);

                    // Delete second punch if it somehow was created
                    DeletePunchFromResult(secondResult.Data).MergeInto(result);
                    DeleteTransferPunchFromResult(secondResult.Data).MergeInto(result);

                    result.Data = GetFailedPunchPairResult(secondResult.Data, firstResult.Data, secondResult.Data, cleanedSecondPunch);
                }
            }
            else
            {
                // First punch failed
                result.Data = GetFailedPunchPairResult(firstResult.Data, firstResult.Data, null, first);
            }

            return result;
        }

        private OpResult CheckPunchIsNotClosedPayroll(RealTimePunchRequest request)
        {
            var result = new OpResult();
            var punchTime = request.OverridePunchTime ?? DateTime.Now;

            var lastClosedPayroll = _session.UnitOfWork.PayrollRepository
                .QueryPayrolls()
                .ByClientId(request.ClientId)
                .ByStatus(false)
                .ByPayrollRunType(PayrollRunType.NormalPayroll)
                .SortByPeriodEnded()
                .ExecuteQueryAs(x => new
                {
                    x.PeriodEnded
                })
                .FirstOrDefault();

            if (lastClosedPayroll != null && punchTime.Date <= lastClosedPayroll.PeriodEnded.Date)
            {
                result.SetToFail(() => new GenericMsg("Cannot associate this punch with a closed payroll period."));
            }

            return result;
        }

        private OpResult CheckPunchIsNotClosedPayroll(ClockEmployeeBenefitDto benefit)
        {
            var result = new OpResult();
            var punchTime = benefit.EventDate ?? DateTime.Now;

            var payFrequency = _payrollService.GetPayFrequencyForEmployeeId(benefit.EmployeeId, benefit.ClientId).Data;

            var lastClosedPayroll = _session.UnitOfWork.PayrollRepository
                .QueryPayrolls()
                .ByClientId(benefit.ClientId)
                .ByStatus(false)
                .ByPayrollRunType(PayrollRunType.NormalPayroll)
                .ByPayFrequency(payFrequency)
                .SortByPeriodEnded(SortDirection.Descending)
                .ExecuteQueryAs(x => new
                {
                    x.PeriodEnded
                })
                .FirstOrDefault();

            if (lastClosedPayroll != null && punchTime.Date <= lastClosedPayroll.PeriodEnded.Date)
            {
                result.SetToFail(() => new GenericMsg("Cannot associate this punch with a closed payroll period."));
            }

            return result;
        }

        IOpResult<EmployeeDto> IEmployeePunchProvider.GetEmployeeWithPunchesByEmployeeId(int employeeId, DateTime startDate, DateTime endDate)
        {
            return new OpResult<EmployeeDto>(_session.UnitOfWork.EmployeeRepository.QueryEmployees().ByEmployeeId(employeeId)
                .ExecuteQueryAs(e => new EmployeeDto
                {
                    EmployeeId = employeeId,
                    UserId = e.UserAccounts.Select(u => u.UserId).FirstOrDefault(),
                    ClientId = e.ClientId,
                    EmployeeNumber = e.EmployeeNumber,
                    ClientCostCenterId = e.ClientCostCenterId,
                    ClientDepartmentId = e.ClientDepartmentId,
                    ClockEmployee = e.ClockEmployee != null ? new ClockEmployeeDto
                    {
                        TimePolicy = e.ClockEmployee.TimePolicy != null ? new ClockClientTimePolicyDtos.ClockClientTimePolicyDto
                        {
                            Rules = e.ClockEmployee.TimePolicy.Rules != null ? new ClockClientRulesDto
                            {
                                MaxShift = e.ClockEmployee.TimePolicy.Rules.MaxShift,
                                ApplyHoursOption = e.ClockEmployee.TimePolicy.Rules.ApplyHoursOption,
                                AllowInputPunches = e.ClockEmployee.TimePolicy.Rules.AllowInputPunches,
                                AllPunchesClockRoundingTypeId = e.ClockEmployee.TimePolicy.Rules.AllPunchesClockRoundingTypeId,
                                IsHideJobCosting = e.ClockEmployee.TimePolicy.Rules.IsHideJobCosting
                            } : null,
                            TimeZone = e.ClockEmployee.TimePolicy.TimeZone != null ? new TimeZoneDto
                            {
                                TimeZoneId = e.ClockEmployee.TimePolicy.TimeZone.TimeZoneId,
                                Description = e.ClockEmployee.TimePolicy.TimeZone.Description,
                                FriendlyDescription = e.ClockEmployee.TimePolicy.TimeZone.FriendlyDesc,
                                Utc = e.ClockEmployee.TimePolicy.TimeZone.Utc
                            } : null,
                            Lunches = e.ClockEmployee.TimePolicy.LunchSelected.Select(l => new ClockClientLunchDto
                            {
                                ClockClientLunchId = l.ClockClientLunchId,
                                ClientCostCenterId = l.ClockClientTimePolicyId,
                                StartTime = l.Lunch.StartTime,
                                StopTime = l.Lunch.StopTime,
                                IsSunday = l.Lunch.IsSunday,
                                IsMonday = l.Lunch.IsMonday,
                                IsTuesday = l.Lunch.IsTuesday,
                                IsWednesday = l.Lunch.IsWednesday,
                                IsThursday = l.Lunch.IsThursday,
                                IsFriday = l.Lunch.IsFriday,
                                IsSaturday = l.Lunch.IsSaturday,
                                AllPunchesClockRoundingTypeId = l.Lunch.AllPunchesClockRoundingTypeId,
                                IsPaid = l.Lunch.IsPaid,
                                PunchType = l.Lunch.PunchType,
                                MinutesToRestrictLunchPunch = l.Lunch.MinutesToRestrictLunchPunch
                            }).ToList()
                        } : null,

                        Punches = e.Punches.Select(p => new ClockEmployeePunchDto
                        {
                            ModifiedPunch = p.ModifiedPunch,
                            ShiftDate = p.ShiftDate,
                            ClientCostCenterId = p.ClientCostCenterId,
                            ClientDepartmentId = p.ClientDepartmentId,
                            ClientDivisionId = p.ClientDivisionId,
                            ClockClientLunchId = p.ClockClientLunchId,
                            ClientJobCostingAssignmentId1 = p.ClientJobCostingAssignmentId1,
                            ClientJobCostingAssignmentId2 = p.ClientJobCostingAssignmentId2,
                            ClientJobCostingAssignmentId3 = p.ClientJobCostingAssignmentId3,
                            ClientJobCostingAssignmentId4 = p.ClientJobCostingAssignmentId4,
                            ClientJobCostingAssignmentId5 = p.ClientJobCostingAssignmentId5,
                            ClientJobCostingAssignmentId6 = p.ClientJobCostingAssignmentId6
                        }).Where(p => p.ModifiedPunch >= startDate && p.ModifiedPunch <= endDate).ToList()
                    } : null,

                })
                .FirstOrDefault());
        }

        #endregion // PRIVATE METHODS
    }
}