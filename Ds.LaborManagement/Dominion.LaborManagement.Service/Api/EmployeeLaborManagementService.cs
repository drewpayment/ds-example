using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Labor;
using System.Data;
using System.Linq;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Common;
using Dominion.Core.Dto.Misc;
using Dominion.Core.Dto.Payroll;
using Dominion.Core.Dto.User;
using Dominion.Core.Services.Api;
using Dominion.Core.Services.Interfaces;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.Security;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.LaborManagement.Dto.EmployeeLaborManagement;
using Dominion.LaborManagement.Service.Api.DataServicesInjectors;
using Dominion.LaborManagement.Service.Internal.Providers;
using Dominion.LaborManagement.Service.Internal.Security;
using Dominion.LaborManagement.Service.Mapping;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.Mapping;
using Dominion.Utility.OpResult;
using Dominion.Utility.Security;
using Dominion.Utility.Msg.Specific;


namespace Dominion.LaborManagement.Service.Api
{
    public class UserSupervisorSecuritySettingDto
    {
        //public bool IsAllowEditCompanySchedules { get; set; }
        public bool IsAllowEditEmployeeSetup    { get; set; }
        public bool IsLimitCostCenters          { get; set; }
        //public bool IsAllowEditManualSchedules  { get; set; }
    }

    public class UserSupervisorSecurityGroupAccessDto
    {
        public int              UserId             { get; set; }
        public int              ClientId           { get; set; }
        public int?             ClientCostCenterId { get; set; }
        public ClientCostCenter ClientCostCenter   { get; set; }
    }


    public class EmployeeLaborManagementService : IEmployeeLaborManagementService
    {
        #region Variables and Properties
        
        private readonly IBusinessApiSession _session;

        private readonly IDsDataServicesClockEmployee _dsEmployee;
        private readonly IEmployeeLaborManagementProvider _employeeLaborManagementProvider;
        internal IEmployeeLaborManagementService Self { get; set; }

        #endregion

        #region Constructors and Initializers

        public EmployeeLaborManagementService(
            IBusinessApiSession session, 
            IDsDataServicesClockEmployee dsEmployee, 
            IEmployeeLaborManagementProvider employeeLaborManagementProvider)
        {
            Self        = this;
            _session    = session;
            _dsEmployee = dsEmployee;
            _employeeLaborManagementProvider = employeeLaborManagementProvider;
        }

        #endregion

        #region IEmployeeLaborManagementService

        /// <summary>
        /// Get a list of TimePolicies for display in a list.
        /// Basic data.
        /// </summary>
        /// <returns></returns>
        IOpResult<IEnumerable<ClockClientTimePolicyDtos.ClockClientTimePolicyListDto>> IEmployeeLaborManagementService.GetClockClientTimePolicyList(int? clientId)
        {
            var opResult = new OpResult<IEnumerable<ClockClientTimePolicyDtos.ClockClientTimePolicyListDto>>();

            opResult.CombineSuccessAndMessages(_session.CanPerformAction(LaborManagementActionType.ReadTimePolicy));

            opResult.TryCatchIfSuccessful(() =>
            {
                var timePolicyList = _session.UnitOfWork
                    .LaborManagementRepository
                    .ClockClientTimePolicyQuery()
                    .ByClientId(clientId.GetValueOrDefault(_session.LoggedInUserInformation.ClientId.Value))
                    .OrderByName()
                    .ExecuteQueryAs(new ClockClientTimePolicyMaps.ToTimePolicyListDto());

                List<int> timePolicyIds = new List<int>();

                for (int i = 0; i < timePolicyList.Count(); i++ )
                {
                    timePolicyIds.Add(timePolicyList.ElementAt(i).ClockClientTimePolicyId);
                }

                var employeeList = _session.UnitOfWork.PayrollRepository.QueryEmployeePay()
                    .ByEmployeeActivity(true)
                    .ByTimePolicyIds(timePolicyIds)
                    .ByClient(_session.LoggedInUserInformation.ClientId.Value)
                    .ExecuteQueryAs(x => new ClockClientTimePolicyDtos.ClockClientTimePolicyDto()
                    {
                         ClockClientTimePolicyId = x.ClockEmployee.TimePolicy.ClockClientTimePolicyId,
                    }).ToOrNewList();

                for (int i = 0; i < timePolicyList.Count(); i++)
                {
                    var tmpTimePolicy = timePolicyList.ElementAt(i);
                    tmpTimePolicy.EmployeeCount = employeeList.Where(e => e.ClockClientTimePolicyId == tmpTimePolicy.ClockClientTimePolicyId).Count();
                }

                opResult.Data = timePolicyList;
            });

            return opResult;
        }

        IOpResult<ClockEmployeeSetupDto> IEmployeeLaborManagementService.GetClockEmployeeSetup(int employeeId, bool filterInactive)
        {
            var result = new OpResult<ClockEmployeeSetupDto>();
            var user   = _session.LoggedInUserInformation;

            _session.CanPerformAction(ClockActionType.ClockEmployeeAdministrator).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, user.ClientId.GetValueOrDefault()).MergeInto(result);

            if (result.HasError)
                return result;

            var eeInfo = result.TryGetData(_session.UnitOfWork.EmployeeRepository
                .QueryEmployees()
                .ByEmployeeId(employeeId)
                .ExecuteQueryAs(x => new
                {
                    x.EmployeeId,
                    x.ClientId
                })
                .FirstOrDefault);

            if(result.CheckForNotFound(eeInfo).HasError)
                return result;

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, eeInfo.ClientId).MergeInto(result);

            if (result.HasError)
                return result;

            var userSuperVisorSecuritySetting = _session.UnitOfWork.UserRepository.QuerySupervisorSecuritySettings()
                .ByUserId(_session.LoggedInUserInformation.UserId)
                .ExecuteQueryAs(new FastMapperDs<UserSupervisorSecuritySetting, UserSupervisorSecuritySettingDto>())
                .ToList();


            if (result.HasError)
                return result;

            var setup = result.TryGetData(_session.UnitOfWork.TimeClockRepository
                .GetClockEmployeeQuery()
                //.ByClientId(_session.LoggedInUserInformation.ClientId ?? 0)
                .ByClientId(eeInfo.ClientId)
                .ByEmployeeId(employeeId)
                .ExecuteQueryAs(x => new ClockEmployeeSetupDto
                {
                    ClientId             = x.ClientId,
                    EmployeeId           = x.EmployeeId,
                    BadgeNumber          = x.BadgeNumber,
                    Pin                  = x.EmployeePin,
                    GeofenceEnabled      = x.GeofenceEnabled,
                    existsPrior          = true,
                    PunchOption = x.TimePolicy != null && x.TimePolicy.Rules != null 
                        ? x.TimePolicy.Rules.PunchOption : null,
                    SelectedTimePolicy = x.TimePolicy != null ? new EmployeeTimePolicyInfoDto
                        {
                            ClientId     = x.TimePolicy.ClientId,
                            Name         = x.TimePolicy.Name,
                            IsActive     = true,
                            TimePolicyId = x.TimePolicy.ClockClientTimePolicyId
                        } : default(EmployeeTimePolicyInfoDto),
                    SelectedSchedules = x.Employee.SelectedSchedules.Select(s => new EmployeeScheduleSetupDto
                        {
                            ClientId        = s.ClientId,
                            Name            = s.ClockSchedule.Name,
                            IsActive        = s.ClockSchedule.IsActive,
                            ScheduleId      = s.ClockClientScheduleId,
                            ScheduleDetails = s.ClockSchedule != null ? new ClockClientScheduleDto
                            {
                                ClockClientScheduleId = s.ClockSchedule.ClockClientScheduleId,
                                ClientShiftId         = s.ClockSchedule.ClientShiftId,
                                ClientStatusId        = s.ClockSchedule.ClientStatusId,
                                ClientDepartmentId    = s.ClockSchedule.ClientDepartmentId,
                                StartTime             = s.ClockSchedule.StartTime,
                                StopTime              = s.ClockSchedule.StopTime,
                                IsIncludeOnHolidays   = s.ClockSchedule.IsIncludeOnHolidays,
                                IsOverrideSchedules   = s.ClockSchedule.IsOverrideSchedules,
                                IsSunday              = s.ClockSchedule.IsSunday,
                                IsMonday              = s.ClockSchedule.IsMonday,
                                IsTuesday             = s.ClockSchedule.IsTuesday,
                                IsWednesday           = s.ClockSchedule.IsWednesday,
                                IsThursday            = s.ClockSchedule.IsThursday,
                                IsFriday              = s.ClockSchedule.IsFriday,
                                IsSaturday            = s.ClockSchedule.IsSaturday
                            } : default(ClockClientScheduleDto)
                        }),
                    SelectedCostCenters = x.ClockEmployeeCostCenters.Select(c => new EmployeeCostCenterSetupDto
                    {
                        CostCenterId              = c.ClientCostCenterId,
                        ClientId                  = x.ClientId,
                        ClockEmployeeCostCenterId = c.ClockEmployeeCostCenterId,
                        Name                      = c.CostCenter.IsActive ? c.CostCenter.Description + " (" + c.CostCenter.Code + ")" : c.CostCenter.Description + " (" + c.CostCenter.Code + ") -- Inactive",
                        IsActive                  = c.CostCenter.IsActive
                    })
                })
                .FirstOrDefault);

            if(result.HasError)
                return result;

            if(setup == null)
            {
                setup = new ClockEmployeeSetupDto
                {
                    EmployeeId  = eeInfo.EmployeeId,
                    ClientId    = eeInfo.ClientId,
                    existsPrior = false,
                    GeofenceEnabled = null
                };
            }
            var usertype = _session.UnitOfWork.UserRepository.GetUser(user.UserId).UserTypeId;

            setup.AllowGroupScheduling = _session.UnitOfWork.ClientAccountFeatureRepository
                .ClientAccountFeatureQuery()
                .ByClientId(setup.ClientId)
                .ByAccountFeatureId(AccountFeatureEnum.GroupScheduler)
                .ExecuteQueryAs(new FastMapperDs<ClientAccountFeature, ClientAccountFeatureDto>()).Any();

            setup.AllowEditEmployeeSetup = (usertype == UserType.Supervisor ? 
                userSuperVisorSecuritySetting.Count > 0 ? 
                    userSuperVisorSecuritySetting.First().IsAllowEditEmployeeSetup : false 
                : true);

            result.SetDataOnSuccess(setup);

            if (filterInactive && result.HasNoErrorAndHasData && result.Data.SelectedCostCenters != null)
            {
                // Filter out inactive cost-centers.
                result.Data.SelectedCostCenters = result.Data.SelectedCostCenters.Where(cc => cc.IsActive);
            }

            return result;
        }

        IOpResult<ClockEmployeeDto> IEmployeeLaborManagementService.VerifyPin(string pin)
        {
            var result = new OpResult<ClockEmployeeDto>();
            var clientId = _session.LoggedInUserInformation.ClientId.GetValueOrDefault();

            _session.CanPerformAction(ClockActionType.ClockEmployeeAdministrator).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(clientId).MergeInto(result);

            if (result.HasError) return result;

            var matches = _session.UnitOfWork.TimeClockRepository.GetClockEmployeeQuery()
                .ByClientId(clientId)
                .ByEmployeePin(pin)
                .ExecuteQueryAs(x => new ClockEmployeeDto
                {
                    EmployeeId = x.EmployeeId,
                    ClientId = x.ClientId,
                    BadgeNumber = x.BadgeNumber,
                    EmployeePin = x.EmployeePin,
                    GeofenceEnabled = x.GeofenceEnabled,
                    AverageWeeklyHours = x.AverageWeeklyHours,
                    ClockClientTimePolicyId = x.ClockClientTimePolicyId,
                    TimePolicy = x.TimePolicy != null ? new ClockClientTimePolicyDtos.ClockClientTimePolicyDto
                    {
                        ClockClientTimePolicyId = x.TimePolicy.ClockClientTimePolicyId,
                        ClientId = x.TimePolicy.ClientId,
                        ClientShiftId = x.TimePolicy.ClientShiftId,
                        ClockClientHolidayId = x.TimePolicy.ClockClientHolidayId,
                        ClientDepartmentId = x.TimePolicy.ClientDepartmentId,
                        ClientStatusId = x.TimePolicy.ClientStatusId,
                        ClockClientExceptionId = x.TimePolicy.ClockClientExceptionId,
                        ClockClientRulesId = x.TimePolicy.ClockClientRulesId,
                        HasCombinedOtFrequencies = x.TimePolicy.HasCombinedOtFrequencies,
                        IsAddToOtherPolicy = x.TimePolicy.IsAddToOtherPolicy,
                        Name = x.TimePolicy.Name,
                        PayType = x.TimePolicy.PayType,
                        TimeZoneId = x.TimePolicy.TimeZoneId,
                        Rules = x.TimePolicy.Rules != null ? new ClockClientRulesDto
                        {
                            ClockClientRulesId = x.TimePolicy.Rules.ClockClientRulesId,
                            ClientId = x.TimePolicy.Rules.ClientId,
                            AllowInputPunches = x.TimePolicy.Rules.AllowInputPunches,
                            AllPunchesClockRoundingTypeId = x.TimePolicy.Rules.AllPunchesClockRoundingTypeId,
                            AllPunchesGraceTime = x.TimePolicy.Rules.AllPunchesGraceTime,
                            ApplyHoursOption = x.TimePolicy.Rules.ApplyHoursOption,
                            BiWeeklyStartingDayOfWeekId = x.TimePolicy.Rules.BiWeeklyStartingDayOfWeekId,
                            ClockAllocateHoursFrequencyId = x.TimePolicy.Rules.ClockAllocateHoursFrequencyId,
                            ClockAllocateHoursOptionId = x.TimePolicy.Rules.ClockAllocateHoursOptionId,
                            ClockTimeFormatId = x.TimePolicy.Rules.ClockTimeFormatId,
                            InEarlyAllowPunchTime = x.TimePolicy.Rules.InEarlyAllowPunchTime,
                            InEarlyClockRoundingTypeId = x.TimePolicy.Rules.InEarlyClockRoundingTypeId,
                            InEarlyGraceTime = x.TimePolicy.Rules.InEarlyGraceTime,
                            InEarlyOutsideGraceTimeClockRoundingTypeId = x.TimePolicy.Rules.InEarly_OutsideGraceTimeClockRoundingTypeId,
                            InLateAllowPunchTime = x.TimePolicy.Rules.InLateAllowPunchTime,
                            InLateClockRoundingTypeId = x.TimePolicy.Rules.InLateClockRoundingTypeId,
                            InLateGraceTime = x.TimePolicy.Rules.InLateGraceTime,
                            InLateOutsideGraceTimeClockRoundingTypeId = x.TimePolicy.Rules.InLate_OutsideGraceTimeClockRoundingTypeId,
                            IsAllowMobilePunch = x.TimePolicy.Rules.IsAllowMobilePunch,
                            IsEditBenefits = x.TimePolicy.Rules.IsEditBenefits,
                            IsEditPunches = x.TimePolicy.Rules.IsEditPunches,
                            IsHideCostCenter = x.TimePolicy.Rules.IsHideCostCenter,
                            IsHideDepartment = x.TimePolicy.Rules.IsHideDepartment,
                            IsHideEmployeeNotes = x.TimePolicy.Rules.IsHideEmployeeNotes,
                            IsHideJobCosting = x.TimePolicy.Rules.IsHideJobCosting,
                            IsHideMultipleSchedules = x.TimePolicy.Rules.IsHideMultipleSchedules,
                            IsHidePunchType = x.TimePolicy.Rules.IsHidePunchType,
                            IsHideShift = x.TimePolicy.Rules.IsHideShift,
                            IsImportBenefits = x.TimePolicy.Rules.IsImportBenefits,
                            IsImportPunches = x.TimePolicy.Rules.IsImportPunches,
                            IsIpLockout = x.TimePolicy.Rules.IsIpLockout,
                            MaxShift = x.TimePolicy.Rules.MaxShift,
                            MonthlyStartingDayOfWeekId = x.TimePolicy.Rules.MonthlyStartingDayOfWeekId,
                            Name = x.TimePolicy.Rules.Name,
                            OutEarlyAllowPunchTime = x.TimePolicy.Rules.OutEarlyAllowPunchTime,
                            OutEarlyClockRoundingTypeId = x.TimePolicy.Rules.OutEarlyClockRoundingTypeId,
                            OutEarlyGraceTime = x.TimePolicy.Rules.OutEarlyGraceTime,
                            OutEarlyOutsideGraceTimeClockRoundingTypeId = x.TimePolicy.Rules.OutEarly_OutsideGraceTimeClockRoundingTypeId,
                            OutLateAllowPunchTime = x.TimePolicy.Rules.OutLateAllowPunchTime,
                            OutLateClockRoundingTypeId = x.TimePolicy.Rules.OutLateClockRoundingTypeId,
                            OutLateGraceTime = x.TimePolicy.Rules.OutLateGraceTime,
                            OutLateOutsideGraceTimeClockRoundingTypeId = x.TimePolicy.Rules.OutLate_OutsideGraceTimeClockRoundingTypeId,
                            SemiMonthlyStartingDayOfWeekId = x.TimePolicy.Rules.SemiMonthlyStartingDayOfWeekId,
                            PunchOption = x.TimePolicy.Rules.PunchOption,
                            ShiftInterval = x.TimePolicy.Rules.ShiftInterval,
                            StartTime = x.TimePolicy.Rules.StartTime,
                            StopTime = x.TimePolicy.Rules.StopTime,
                            WeeklyStartingDayOfWeekId = x.TimePolicy.Rules.WeeklyStartingDayOfWeekId
                        } : default(ClockClientRulesDto),
                        Lunches = x.TimePolicy.LunchSelected.Select(l => new ClockClientLunchDto
                        {
                            ClockClientLunchId = l.Lunch.ClockClientLunchId,
                            AllPunchesClockRoundingTypeId = l.Lunch.AllPunchesClockRoundingTypeId,
                            AllPunchesGraceTime = l.Lunch.AllPunchesGraceTime,
                            AutoDeductedWorkedHours = l.Lunch.AutoDeductedWorkedHours,
                            ClientCostCenterId = l.Lunch.ClientCostCenterId,
                            ClientId = l.Lunch.ClientId,
                            GraceTime = l.Lunch.GraceTime,
                            InEarlyClockRoundingTypeId = l.Lunch.InEarlyClockRoundingTypeId,
                            InEarlyGraceTime = l.Lunch.InEarlyGraceTime,
                            InLateClockRoundingTypeId = l.Lunch.InLateClockRoundingTypeId,
                            InLateGraceTime = l.Lunch.InLateGraceTime,
                            IsAllowMultipleTimePeriods = l.Lunch.IsAllowMultipleTimePeriods,
                            IsAutoDeducted = l.Lunch.IsAutoDeducted,
                            IsDoEmployeesPunch = l.Lunch.IsDoEmployeesPunch,
                            IsFriday = l.Lunch.IsFriday,
                            IsMaxPaid = l.Lunch.IsMaxPaid,
                            IsMonday = l.Lunch.IsMonday,
                            IsPaid = l.Lunch.IsPaid,
                            IsSaturday = l.Lunch.IsSaturday,
                            IsShowPunches = l.Lunch.IsShowPunches,
                            IsSunday = l.Lunch.IsSunday,
                            IsThursday = l.Lunch.IsThursday,
                            IsTuesday = l.Lunch.IsTuesday,
                            IsUseStartStopTimes = l.Lunch.IsUseStartStopTimes,
                            IsWednesday = l.Lunch.IsWednesday,
                            Length = l.Lunch.Length, 
                            Name = l.Lunch.Name,
                            OutEarlyClockRoundingTypeId = l.Lunch.OutEarlyClockRoundingTypeId,
                            OutEarlyGraceTime = l.Lunch.OutEarlyGraceTime,
                            OutLateClockRoundingTypeId = l.Lunch.OutLateClockRoundingTypeId,
                            OutLateGraceTime = l.Lunch.OutLateGraceTime,
                            PunchType = l.Lunch.PunchType,
                            StartTime = l.Lunch.StartTime, 
                            StopTime = l.Lunch.StopTime,
                            MinutesToRestrictLunchPunch = l.Lunch.MinutesToRestrictLunchPunch
                        }).ToList(),
                    } : default(ClockClientTimePolicyDtos.ClockClientTimePolicyDto),
                    Employee = x.Employee != null ? new EmployeeFullWithCostCenterSetupDto
                    {
                        EmployeeId = x.EmployeeId,
                        ClientId = x.ClientId,
                        FirstName = x.Employee.FirstName,
                        MiddleInitial = x.Employee.MiddleInitial,
                        LastName = x.Employee.LastName,
                        ClientDivisionId = x.Employee.ClientDivisionId,
                        ClientDepartmentId = x.Employee.ClientDepartmentId,
                        CostCenterId = x.Employee.ClientCostCenterId ?? 0,
                        CostCenter = x.Employee.CostCenter != null ? new ClientCostCenterDto
                        {
                            ClientCostCenterId = x.Employee.CostCenter.ClientCostCenterId,
                            ClientId = x.Employee.CostCenter.ClientId,
                            Code = x.Employee.CostCenter.Code,
                            Description = x.Employee.CostCenter.Description,
                            IsActive = x.Employee.CostCenter.IsActive,
                            IsDefaultGlCostCenter = x.Employee.CostCenter.IsDefaultGlCostCenter,
                        } : default(ClientCostCenterDto),
                        EmployeeNumber = x.Employee.EmployeeNumber,
                    } : default(EmployeeFullWithCostCenterSetupDto),
                    Client = x.Client != null ? new TimeClockClientDto
                    {
                        ClientId = x.Client.ClientId,
                        AddressLine1 = x.Client.AddressLine1,
                        AddressLine2 = x.Client.AddressLine2,
                        City = x.Client.City,
                        ClientCode = x.Client.ClientCode,
                        ClientName = x.Client.ClientName,
                        PostalCode = x.Client.PostalCode,
                        StateId = x.Client.StateId.ToString(),
                    } : default(TimeClockClientDto),
                })
                .FirstOrDefault();

            if (matches == null) return result.SetToFail("Could not verify pin. Please try again.");

            return result.TrySetData(() => matches);
        }

        IOpResult<int> IEmployeeLaborManagementService.GetIpadPinLength()
        {
            var result   = new OpResult<int>();
            var clientId = _session.LoggedInUserInformation.ClientId.GetValueOrDefault();
            
            _session.CanPerformAction(ClockActionType.ClockEmployeeAdministrator).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(clientId).MergeInto(result);

            if (result.HasError)
                return result;

            var query = _session.UnitOfWork.ClientAccountFeatureRepository
                .ClientOptionQuery()
                .ByClientId(clientId)
                .ByOption(AccountOption.TimeClock_IPadPinNumberLength)
                .ExecuteQueryAs(x => new ClientAccountOptionDto
                {
                    ClientAccountOptionId = x.ClientAccountOptionId,
                    Value                 = x.Value
                }).ToList();

            if (query.Any())
            {
                var query2 = _session.UnitOfWork.ClientAccountFeatureRepository
                    .AccountOptionItemQuery()
                    .ByClientOptionItemId(Int32.Parse(query[0].Value))
                    .ExecuteQueryAs(x => new Core.Dto.Misc.AccountOptionItemDto
                    {
                        AccountOptionItemId = x.AccountOptionItemId,
                        Value               = x.Value
                    }).ToList();

                if (query2.Any())
                    result.Data = (int)query2[0].Value;
                else
                    result.Data = 4;
            }
            else
                result.Data = 4;


            return result;
        }

        public IOpResult<ClockEmployeeSetupDto> SaveClockEmployee(ClockEmployeeSetupDto dto)
        {
            var result = new OpResult<ClockEmployeeSetupDto>();
            var userId = _session.LoggedInUserInformation.UserId;
            _session.CanPerformAction(ClockActionType.ClockEmployeeAdministrator).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessByAccessibleClientIds(dto.ClientId).MergeInto(result);

            if (result.HasError)
                return result;

            // Pass filterActive=false to preserve original behavior.
            var sqlClockEmployee = Self.GetClockEmployeeSetup(dto.EmployeeId, false);

            if (sqlClockEmployee.Success && sqlClockEmployee.HasData && sqlClockEmployee.Data.existsPrior)
            {
                //var clockEmployeeEntity = new ClockEmployee
                //{
                //    BadgeNumber             = dto.BadgeNumber,
                //    EmployeePin             = dto.Pin,
                //    ClockClientTimePolicyId = dto.SelectedTimePolicy?.TimePolicyId,
                //    ClientId                = _session.LoggedInUserInformation.ClientId.GetValueOrDefault(),
                //    EmployeeId              = dto.EmployeeId,

                //};

                //_session.SetModifiedProperties(clockEmployeeEntity);

                //var clockEmployeePropertyList = new PropertyList<ClockEmployee>();
                
                //if (sqlClockEmployee.Data.BadgeNumber != clockEmployeeEntity.BadgeNumber)
                //    clockEmployeePropertyList.Add(x => x.BadgeNumber);
                //if (sqlClockEmployee.Data.Pin != clockEmployeeEntity.EmployeePin)
                //    clockEmployeePropertyList.Add(x => x.EmployeePin);
                //if (sqlClockEmployee.Data.SelectedTimePolicy != null)
                //    if (sqlClockEmployee.Data.SelectedTimePolicy.TimePolicyId != clockEmployeeEntity.ClockClientTimePolicyId)
                //        clockEmployeePropertyList.Add(x => x.ClockClientTimePolicyId);
                //if (sqlClockEmployee.Data.SelectedTimePolicy == null && clockEmployeeEntity.ClockClientTimePolicyId != null)
                //    clockEmployeePropertyList.Add(x => x.ClockClientTimePolicyId);

                //clockEmployeePropertyList.IncludeModifiedOptionalProperties();

                //_session.UnitOfWork.RegisterModified(clockEmployeeEntity, clockEmployeePropertyList);
                var dt = DateTime.Now;
                _dsEmployee.UpdateClockEmployee(dto.EmployeeId, dto.BadgeNumber, dto.Pin, -1, -1, dto.SelectedTimePolicy?.TimePolicyId ?? -1, false, userId, dt, dto.GeofenceEnabled.Value);

                DateTime start = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                DateTime end   = Convert.ToDateTime("12/31/" + DateTime.Now.ToString("yyyy"));

                _dsEmployee.InsertClockEmployeeBenefitHolidays(dto.EmployeeId, 0, start, end,
                    userId, dto.SelectedTimePolicy?.TimePolicyId ?? -1, false);

                var ss  = dto.SelectedSchedules.ToOrNewList();
                var sss = sqlClockEmployee.Data.SelectedSchedules.ToOrNewList();
                var deleteIds = new List<int>();

                // Eliminate the schedules that we will do nothing with.
                foreach (var schedule in ss)
                {
                    foreach (var sqlSchedule in sss)
                    {
                        if (sqlSchedule.ScheduleId == schedule.ScheduleId)
                        {
                            deleteIds.Add(sqlSchedule.ScheduleId);
                        }
                    }
                }

                ss.RemoveAll(x => deleteIds.Contains(x.ScheduleId));
                sss.RemoveAll(x => deleteIds.Contains(x.ScheduleId));

                var scc    = dto.SelectedCostCenters.ToOrNewList();
                var sqlscc = sqlClockEmployee.Data.SelectedCostCenters.ToOrNewList();
                deleteIds  = new List<int>();

                // Eliminate the cost centers that we will do nothing with.
                foreach (var cc in scc)
                {
                    foreach (var sqlcc in sqlscc)
                    {
                        if (sqlcc.ClockEmployeeCostCenterId == cc.ClockEmployeeCostCenterId)
                        {
                            deleteIds.Add(sqlcc.ClockEmployeeCostCenterId);
                        }
                    }
                }

                scc.RemoveAll(x => deleteIds.Contains(x.ClockEmployeeCostCenterId));
                sqlscc.RemoveAll(x => deleteIds.Contains(x.ClockEmployeeCostCenterId));

                // Anything left in sql dto will be deleted from the database
                foreach (var sched in sss)
                {
                    if (ss.Any(x => x.ScheduleId == sched.ScheduleId))
                    {
                        continue;
                    }
                    var scheduleEntity = new ClockClientScheduleSelected
                    {
                        ClockClientScheduleId = sched.ScheduleId,
                        ClientId              = sched.ClientId,
                        EmployeeId            = dto.EmployeeId
                    };
                    _session.UnitOfWork.RegisterDeleted(scheduleEntity);
                    _session.RegisterNewChangeHistoryWithEnum<ClockClientScheduleSelected, ClockClientScheduleSelectedChangeHistory>(scheduleEntity, ChangeModeType.Deleted);

                }

                // Anything left in sql dto will be deleted from the database
                foreach (var cc in sqlscc)
                {
                    var costcenterEntity = new ClockEmployeeCostCenter
                    {
                        ClockEmployeeCostCenterId = cc.ClockEmployeeCostCenterId,
                        EmployeeId                = dto.EmployeeId
                    };

                    _session.UnitOfWork.RegisterDeleted(costcenterEntity);
                }

                // Anything left in our saved dto will be added to the database
                foreach (var sched in ss)
                {
                    if (sss.Any(x => x.ScheduleId == sched.ScheduleId))
                    { continue;}
                    var scheduleEntity = new ClockClientScheduleSelected
                    {
                        ClockClientScheduleId = sched.ScheduleId,
                        EmployeeId            = dto.EmployeeId,
                        ClientId              = dto.ClientId //_session.LoggedInUserInformation.ClientId.GetValueOrDefault()    
                        
                    };
                    _session.SetModifiedProperties(scheduleEntity);
                    _session.UnitOfWork.RegisterNew(scheduleEntity);
                }

                // Anything left in our saved dto will be added to the database
                foreach (var cc in scc)
                {
                    var costcenterEntity = new ClockEmployeeCostCenter
                    {
                        ClockEmployeeCostCenterId = cc.ClockEmployeeCostCenterId,
                        EmployeeId                = dto.EmployeeId,
                        ClientCostCenterId        = cc.CostCenterId
                    };

                    _session.SetModifiedProperties(costcenterEntity);
                    _session.UnitOfWork.RegisterNew(costcenterEntity);
                }

                
            }
            else
            {
                var clockEmployeeEntity = new ClockEmployee
                {
                    BadgeNumber             = dto.BadgeNumber,
                    EmployeePin             = dto.Pin,
                    ClockClientTimePolicyId = dto.SelectedTimePolicy?.TimePolicyId,
                    GeofenceEnabled         = dto.GeofenceEnabled.Value,
                    ClientId                = dto.ClientId, //_session.LoggedInUserInformation.ClientId.GetValueOrDefault(),
                    EmployeeId              = dto.EmployeeId,

                };

                _session.SetModifiedProperties(clockEmployeeEntity);
                _session.UnitOfWork.RegisterNew(clockEmployeeEntity);
                _session.UnitOfWork.Commit().MergeInto(result);
                DateTime start = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy"));
                DateTime end   = Convert.ToDateTime("12/31/" + DateTime.Now.ToString("yyyy"));

                _dsEmployee.InsertClockEmployeeBenefitHolidays(dto.EmployeeId, 0, start, end,
                    userId, dto.SelectedTimePolicy?.TimePolicyId ?? -1, false);

                var ss  = dto.SelectedSchedules.ToOrNewList();
                var scc = dto.SelectedCostCenters.ToOrNewList();

                foreach (var sched in ss)
                {
                    var scheduleEntity = new ClockClientScheduleSelected
                    {
                        ClockClientScheduleId = sched.ScheduleId,
                        EmployeeId            = dto.EmployeeId,
                        ClientId              = dto.ClientId

                    };
                    _session.SetModifiedProperties(scheduleEntity);
                    _session.UnitOfWork.RegisterNew(scheduleEntity);
                }

                foreach (var cc in scc)
                {
                    var costcenterEntity = new ClockEmployeeCostCenter
                    {
                        ClockEmployeeCostCenterId = cc.ClockEmployeeCostCenterId,
                        EmployeeId                = dto.EmployeeId,
                        ClientCostCenterId        = cc.CostCenterId
                    };

                    _session.SetModifiedProperties(costcenterEntity);
                    _session.UnitOfWork.RegisterNew(costcenterEntity);
                }
            }
            
            _session.UnitOfWork.Commit().MergeInto(result);

            return result;
        }

        IOpResult<IEnumerable<EmployeeCostCenterSetupDto>> IEmployeeLaborManagementService.GetAvailableCostCentersForEmployee(int employeeId, bool filterInactive)
        {
            var result   = new OpResult<IEnumerable<EmployeeCostCenterSetupDto>>();
            var clientId = _session.LoggedInUserInformation.ClientId.GetValueOrDefault();
            var user     = _session.LoggedInUserInformation;
            
            _session.CanPerformAction(ClockActionType.ClockEmployeeAdministrator).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client,clientId).MergeInto(result);

            if (result.HasError)
                return result;

            var eeInfo = result.TryGetData(_session.UnitOfWork.EmployeeRepository
                .QueryEmployees()
                .ByEmployeeId(employeeId)
                .ExecuteQueryAs(x => new
                {
                    x.EmployeeId,
                    x.ClientId,
                    x.ClientCostCenterId

                }).FirstOrDefault);

            result.CheckForNotFound(eeInfo);
            if (result.HasError)
                return result;

            var ecrccInfo = _session.UnitOfWork.EmployeeRepository.EmployeeClientRateCostCenterQuery()
                .ByEmployeeId(eeInfo.EmployeeId).ExecuteQueryAs(x => new
                {
                    x.ClientCostCenterId

                }).FirstOrDefault();

            var eccpInfo = _session.UnitOfWork.EmployeeRepository.EmployeeCostCenterPercentageQuery().ByEmployeeId(eeInfo.EmployeeId).ExecuteQueryAs(x => new
            {
                x.ClientCostCenterId

            }).FirstOrDefault();

            var costCenters = _session.UnitOfWork.LaborManagementRepository
                .ClientCostCenterQuery()
                .ByClientId(clientId)
                .ExecuteQueryAs(x => new EmployeeCostCenterSetupDto
                {
                    ClientId     = x.ClientId,
                    Name         = x.Description,
                    IsActive     = x.IsActive,
                    CostCenterId = x.ClientCostCenterId,
                    Code         = x.Code

                }).OrderBy(x => x.Name).ToList();

            var usertype                          = _session.UnitOfWork.UserRepository.GetUser(user.UserId).UserTypeId;
            var userSuperVisorSecuritySetting     = new UserSupervisorSecuritySettingDto();
            var userSupervisorSecurityGroupAccess = new UserSupervisorSecurityGroupAccessDto();
            
            if (usertype == UserType.Supervisor)
            {
                userSuperVisorSecuritySetting = _session.UnitOfWork.UserRepository
                    .QuerySupervisorSecuritySettings()
                    .ByUserId(user.UserId)
                    .ExecuteQueryAs(new FastMapperDs<UserSupervisorSecuritySetting, UserSupervisorSecuritySettingDto>())
                    .FirstOrDefault();
                userSupervisorSecurityGroupAccess = _session.UnitOfWork.UserRepository
                    .QuerySupervisorSecurityGroupAccess()
                    .ByUserId(user.UserId)
                    .ExecuteQueryAs(new FastMapperDs<UserSupervisorSecurityGroupAccess, UserSupervisorSecurityGroupAccessDto>())
                    .FirstOrDefault();

                costCenters.RemoveWhere(x => x.IsActive == false);
                if (userSuperVisorSecuritySetting != null)
                    if (userSuperVisorSecuritySetting.IsLimitCostCenters)
                    {
                        if (userSupervisorSecurityGroupAccess != null)
                            costCenters =
                                costCenters.Where(x => userSupervisorSecurityGroupAccess.ClientCostCenterId == x.CostCenterId).ToList();
                    
                    }
                
            }
            else if (usertype == UserType.CompanyAdmin)
            {
                costCenters.RemoveWhere(x => x.IsActive == false);
            }

            foreach (var cc in costCenters)
            {
                if (!cc.IsActive)
                {
                    cc.Name = cc.Name + " (" + cc.Code + ") -- Inactive";
                }
                else
                {
                    cc.Name = cc.Name + " (" + cc.Code + ")";
                }

            }

            if (eeInfo.ClientCostCenterId != null)
            {
                var ecc = _session.UnitOfWork.LaborManagementRepository
                    .ClientCostCenterQuery()
                    .ByCostCenter(eeInfo.ClientCostCenterId.GetValueOrDefault())
                    .ByIsActive(false)
                    .ExecuteQueryAs(x => new EmployeeCostCenterSetupDto
                    {
                        ClientId     = x.ClientId,
                        Name         = x.Description + " (" + x.Code + ") -- Inactive",
                        IsActive     = x.IsActive,
                        CostCenterId = x.ClientCostCenterId,
                        Code         = x.Code
                    }).FirstOrDefault();

                if (ecc != null)
                    costCenters.Add(ecc);
            }
            

            if (ecrccInfo != null)
            {
                var ecrcc = _session.UnitOfWork.LaborManagementRepository
                    .ClientCostCenterQuery()
                    .ByCostCenter(ecrccInfo.ClientCostCenterId)
                    .ByIsActive(false)
                    .ExecuteQueryAs(x => new EmployeeCostCenterSetupDto
                    {
                        ClientId     = x.ClientId,
                        Name         = x.Description + " (" + x.Code + ") -- Inactive",
                        IsActive     = x.IsActive,
                        CostCenterId = x.ClientCostCenterId,
                        Code         = x.Code

                    }).FirstOrDefault();
                if (ecrcc != null)
                    costCenters.Add(ecrcc);
            }

            if (eccpInfo != null)
            {
                var eccp = _session.UnitOfWork.LaborManagementRepository
                    .ClientCostCenterQuery()
                    .ByCostCenter(eccpInfo.ClientCostCenterId)
                    .ByIsActive(false)
                    .ExecuteQueryAs(x => new EmployeeCostCenterSetupDto
                    {
                        ClientId     = x.ClientId,
                        Name         = x.Description + " (" + x.Code + ") -- Inactive",
                        IsActive     = x.IsActive,
                        CostCenterId = x.ClientCostCenterId,
                        Code         = x.Code
                    }).FirstOrDefault();
                if (eccp != null)
                    costCenters.Add(eccp);
            }

            result.Data = costCenters;

            if (filterInactive && result.HasNoErrorAndHasData)
            {
                // Filter out inactive cost-centers.
                result.Data = result.Data.Where(cc => cc.IsActive);
            }

            return result;
        }

        IOpResult<IEnumerable<EmployeeScheduleSetupDto>> IEmployeeLaborManagementService.GetAvailableCompanySchedules(int clientId, bool activeOnly)
        {
            var result = new OpResult<IEnumerable<EmployeeScheduleSetupDto>>();

            _session.CanPerformAction(ClockActionType.ClockEmployeeAdministrator).MergeInto(result);
            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, clientId).MergeInto(result);
            if(result.HasError)
                return result;

            var query = _session.UnitOfWork.LaborManagementRepository
                .ClockClientScheduleQuery()
                .ByClient(clientId);

            if(activeOnly)
                query.ByIsActive();

            var companySchedules = result.TryGetData(query.ExecuteQueryAs(x => new EmployeeScheduleSetupDto
                {
                    ClientId = x.ClientId,
                    IsActive = x.IsActive,
                    Name = x.Name,
                    ScheduleId = x.ClockClientScheduleId
                })
                .OrderBy(x => x.Name)
                .ToList);
        
            result.SetDataOnSuccess(companySchedules);

            return result;
        }

        IOpResult<IEnumerable<EmployeeTimePolicyInfoDto>> IEmployeeLaborManagementService.GetAvailableTimePoliciesForEmployee(int employeeId)
        {
            var result = new OpResult<IEnumerable<EmployeeTimePolicyInfoDto>>();
            
            _session.CanPerformAction(ClockActionType.ClockEmployeeAdministrator).MergeInto(result);
            if(result.HasError)
                return result;

            var eeInfo = result.TryGetData(_session.UnitOfWork.EmployeeRepository
                .QueryEmployees()
                .ByEmployeeId(employeeId)
                .ExecuteQueryAs(x => new
                {
                    x.EmployeeId,
                    x.ClientId,
                    PayInfo = x.EmployeePayInfo.Select(p => new
                    {
                        p.PayFrequencyId
                    }).FirstOrDefault()
                })
                .FirstOrDefault);

            result.CheckForNotFound(eeInfo);
            if(result.HasError)
                return result;

            result.CheckForNull(eeInfo.PayInfo, "Employee Pay Setup");
            if(result.HasError)
                return result;

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, eeInfo.ClientId).MergeInto(result);
            //TODO: Supervisor Security / EE Access Check
            if(result.HasError)
                return result;

            var allPolicies = result.TryGetData(_session.UnitOfWork.TimeClockRepository
                .GetClockClientTimePolicyQuery()
                .ByClientId(eeInfo.ClientId)
                .ExecuteQueryAs(x => new RawPolicyInfoDto
                {
                    TimePolicyId = x.ClockClientTimePolicyId,
                    ClientId = x.ClientId,
                    Name = x.Name,
                    Rule = x.Rules != null ? new RawRuleInfoDto
                    {
                        BiWeeklyStartingDayOfWeekId = x.Rules.BiWeeklyStartingDayOfWeekId,
                        WeeklyStartingDayOfWeekId = x.Rules.WeeklyStartingDayOfWeekId,
                        MonthlyStartingDayOfWeekId = x.Rules.MonthlyStartingDayOfWeekId,
                        SemiMonthlyStartingDayOfWeekId = x.Rules.SemiMonthlyStartingDayOfWeekId
                    } : default(RawRuleInfoDto)
                })
                .OrderBy(x => x.Name)
                .ToList);
            if(result.HasError)
                return result;

            var availablePolicies = new List<EmployeeTimePolicyInfoDto>();
            foreach (var policy in allPolicies)
            {
                var availableToEmployee = policy.Rule == null;

                if(!availableToEmployee)
                {
                    switch (eeInfo.PayInfo.PayFrequencyId)
                    {
                        case PayFrequencyType.Weekly:
                            availableToEmployee = policy.Rule.WeeklyStartingDayOfWeekId.HasValue;
                            break;
                        case PayFrequencyType.BiWeekly:
                        case PayFrequencyType.AlternateBiWeekly:
                            availableToEmployee = policy.Rule.BiWeeklyStartingDayOfWeekId.HasValue;
                            break;
                        case PayFrequencyType.SemiMonthly:
                            availableToEmployee = policy.Rule.SemiMonthlyStartingDayOfWeekId.HasValue;
                            break;
                        case PayFrequencyType.Monthly:
                            availableToEmployee = policy.Rule.MonthlyStartingDayOfWeekId.HasValue;
                            break;
                    }
                }

                if(availableToEmployee){
                    availablePolicies.Add(new EmployeeTimePolicyInfoDto
                    {
                        ClientId     = policy.ClientId,
                        TimePolicyId = policy.TimePolicyId,
                        Name         = policy.Name,
                        IsActive     = true
                    });
                }
            }

            result.SetDataOnSuccess(availablePolicies);

            return result;
        }

        /// <summary>
        /// Public method for checking Badge Number and Employee Pin availability
        /// </summary>
        /// <remarks>
        /// The 
        /// </remarks>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IOpResult<bool> CanAssignEmployeePinToEmployee(ClockEmployeeSetupDto dto)
        {
            var opResult = new OpResult<bool>();
            _session.CanPerformAction(ClockActionType.ClockEmployeeAdministrator).MergeInto(opResult);

            if (opResult.HasError)
                return opResult;

            if (dto.Pin.Length > 4 || !int.TryParse(dto.Pin, out var isNumeric) || dto.BadgeNumber.Length > 15)
            {
                opResult.AddMessage(new GenericMsg("Pin should only be numeric and 4 characters long")).SetToFail();
                return opResult;
            }

            var employees = opResult.TryGetData(_session.UnitOfWork.TimeClockRepository
                .GetClockEmployeeQuery()
                .ByClientId(dto.ClientId, true)
                .ExecuteQueryAs(x => new ClockEmployeeSetupDto
                {
                    EmployeeId = x.EmployeeId,
                    Pin = x.EmployeePin,
                    BadgeNumber = x.BadgeNumber
                }).ToList);

            if (opResult.HasError)
                return opResult;

            if (employees.Any(x => x.Pin == dto.Pin && x.EmployeeId != dto.EmployeeId))
            {
                opResult.AddMessage(new GenericMsg("Employee PIN Number already in use.")).SetToFail();
                return opResult;
            }

            if (employees.Any(x => x.BadgeNumber == dto.BadgeNumber && x.EmployeeId != dto.EmployeeId))
            {
                opResult.AddMessage(new GenericMsg("Badge Number already in use")).SetToFail();
                return opResult;
            }

            return opResult;
        }

        public IOpResult<IEnumerable<ClockClientTimePolicyDtos.ClockClientTimePolicyDto>> UpdateGeofenceOnTimePolicy(IEnumerable<ClockClientTimePolicyDtos.ClockClientTimePolicyDto> dto)
        {
            var result = new OpResult<IEnumerable<ClockClientTimePolicyDtos.ClockClientTimePolicyDto>>();

            _session.ResourceAccessChecks.CheckAccessById(ResourceOwnership.Client, _session.LoggedInUserInformation.ClientId.GetValueOrDefault()).MergeInto(result);
            // _session.CanPerformAction(LaborManagementActionType.WriteTimePolicy).MergeInto(result);
            _session.CanPerformAction(LaborManagementActionType.EditTimePolicy).MergeInto(result);
            _session.CanPerformAction(ClockActionType.CanOptInToCompanyFeature).MergeInto(result);

            if (result.HasError) return result;

            _employeeLaborManagementProvider.UpdateGeofenceOnTimePolicy(dto).MergeInto(result);

            return result;
        }

        #endregion
    }
}

