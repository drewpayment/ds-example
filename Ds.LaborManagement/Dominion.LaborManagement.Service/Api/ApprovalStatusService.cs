using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.LeaveManagement;
using Dominion.Core.Dto.Payroll;
using Dominion.Core.Dto.Security;
using Dominion.Core.Dto.User;
using Dominion.Core.Services.Interfaces;
using Dominion.Core.Services.Internal.Providers;
using Dominion.LaborManagement.Dto.Approval;
using Dominion.Utility.Msg.Specific;
using Dominion.Utility.OpResult;

namespace Dominion.LaborManagement.Service.Api
{
    public class ApprovalStatusService : IApprovalStatusService
    {
        private readonly IBusinessApiSession _session;
        private readonly IClientSettingProvider _clientSettings;
        internal ApprovalStatusService(IBusinessApiSession session, IClientSettingProvider clientSettings)
        {
            _session = session;
            _clientSettings = clientSettings;
        }

        IOpResult<SupervisorStatusResultDto> IApprovalStatusService.GetEmployeeExceptionStatus(int clientId, int payrollId, ApprovalOptionType approvalOption, int? userId = null, int? employeeId = null, bool isActiveOnly = true)
        {
            var result = new OpResult<SupervisorStatusResultDto>();

            userId = userId ?? _session.LoggedInUserInformation.UserId;

            //todo: what if user not found
            var isSuper = result.TryGetData(() => _session.UnitOfWork.UserRepository.QueryUsers()
                .ByUserId(userId.Value)
                .ExecuteQueryAs(x => x.UserTypeId == UserType.Supervisor)
                .FirstOrDefault());

            var payrollInfo = result.TryGetData(() => _session.UnitOfWork.PayrollRepository.QueryPayrolls()
                .ByPayrollId(payrollId)
                .ByClientId(clientId)
                .ExecuteQueryAs(p => new
                {
                    p.PayrollId,

                    p.IsFrequencyAltBiWeekly,
                    p.IsFrequencyBiWeekly,
                    p.IsFrequencyMonthly,
                    p.IsFrequencyQuarterly,
                    p.IsFrequencySemiMonthly,
                    p.IsFrequencyWeekly,

                    p.PeriodEnded,
                    p.PeriodStart,

                    p.FrequencyAltBiWeeklyPeriodStart,
                    p.FrequencyBiWeeklyPeriodStart,
                    p.FrequencyMonthlyPeriodStart,
                    p.FrequencyQuarterlyPeriodStart,
                    p.FrequencySemiMonthlyPeriodStart,

                    p.FrequencyAltBiWeeklyPeriodEnded,
                    p.FrequencyBiWeeklyPeriodEnded,
                    p.FrequencyMonthlyPeriodEnded,
                    p.FrequencyQuarterlyPeriodEnded,
                    p.FrequencySemiMonthlyPeriodEnded
                })
                .FirstOrDefault());


            var clockEmployees = result.TryGetData(() => {

                var q = _session.UnitOfWork.TimeClockRepository.GetClockEmployeeQuery()
                    .ByClientId(clientId);

                if(isActiveOnly)
                    q.ByActiveEmployee(isActive:true);

                return q.ExecuteQueryAs(x => new
                    {
                        x.EmployeeId,
                        x.Employee.FirstName,
                        x.Employee.MiddleInitial,
                        x.Employee.LastName,
                        x.Employee.ClientCostCenterId,
                        x.Employee.ClientDepartmentId,
                        x.GeofenceEnabled,
                        ClientDepartmentCode = x.Employee.Department != null ? x.Employee.Department.Code : null,
                        PayFrequency = x.Employee.EmployeePayInfo.Select(ep => ep.PayFrequencyId).FirstOrDefault(),
                        PayType = x.Employee.EmployeePayInfo.Select(ep => ep.Type).FirstOrDefault()
                    })
                    .Select(x =>
                    {
                        var periodStart = payrollInfo.PeriodStart;
                        var periodEnd = payrollInfo.PeriodEnded;

                        switch (x.PayFrequency)
                        {
                            case PayFrequencyType.BiWeekly:
                                if(payrollInfo.IsFrequencyBiWeekly ?? false)
                                {
                                    periodStart = payrollInfo.FrequencyBiWeeklyPeriodStart ?? periodStart;
                                    periodEnd = payrollInfo.FrequencyBiWeeklyPeriodEnded ?? periodEnd;
                                }
                                break;
                            case PayFrequencyType.SemiMonthly:
                                if(payrollInfo.IsFrequencySemiMonthly ?? false)
                                {
                                    periodStart = payrollInfo.FrequencySemiMonthlyPeriodStart ?? periodStart;
                                    periodEnd = payrollInfo.FrequencySemiMonthlyPeriodEnded ?? periodEnd;
                                }
                                break;
                            case PayFrequencyType.Monthly:
                                if(payrollInfo.IsFrequencyMonthly ?? false)
                                {
                                    periodStart = payrollInfo.FrequencyMonthlyPeriodStart ?? periodStart;
                                    periodEnd = payrollInfo.FrequencyMonthlyPeriodEnded ?? periodEnd;
                                }
                                break;
                            case PayFrequencyType.AlternateBiWeekly:
                                if(payrollInfo.IsFrequencyAltBiWeekly ?? false)
                                {
                                    periodStart = payrollInfo.FrequencyAltBiWeeklyPeriodStart ?? periodStart;
                                    periodEnd = payrollInfo.FrequencyAltBiWeeklyPeriodEnded ?? periodEnd;
                                }
                                break;
                            case PayFrequencyType.Quarterly:
                                if(payrollInfo.IsFrequencyQuarterly ?? false)
                                {
                                    periodStart = payrollInfo.FrequencyQuarterlyPeriodStart ?? periodStart;
                                    periodEnd = payrollInfo.FrequencyQuarterlyPeriodEnded ?? periodEnd;
                                }
                                break;
                        }

                        return new EmployeeClockApprovalStatusDto
                        {
                            EmployeeId           = x.EmployeeId,
                            FirstName            = x.FirstName,
                            LastName             = x.LastName,
                            MiddleInitial        = x.MiddleInitial,
                            ClientCostCenterId   = x.ClientCostCenterId,
                            ClientDepartmentId   = x.ClientDepartmentId,
                            ClientDepartmentCode = x.ClientDepartmentCode,
                            PayFrequency         = x.PayFrequency,
                            PeriodStart          = periodStart,
                            PeriodEnd            = periodEnd,
                            PayType              = x.PayType
                        };
                    })
                    .ToList();
            });

            if(!clockEmployees.Any())
            {
                result.SetToFail().AddMessage(new GenericMsg("No employees with time clock setups were found."));
                return result;
            }

            //now go out and get the exception, punch, benefit, etc data required to determined approval status
            var eventsToApprove = new List<IClockEventToBeApproved>();

            var minPeriodStart = clockEmployees.Min(x => x.PeriodStart).Date;
            var maxPeriodEnd = clockEmployees.Max(x => x.PeriodEnd).Date.AddDays(1).AddSeconds(-1);

            //get punches for time period...used to filter exceptions and/or for punch activity (depending on approval option) 
            var punches = result.TryGetData(_session.UnitOfWork.TimeClockRepository.GetClockEmployeePunchQuery()
                .ByClientId(clientId)
                .ByDates(minPeriodStart, maxPeriodEnd)
                .ExecuteQueryAs(x => new EmployeePunchEventToBeApproved
                {
                    EmployeePunchId = x.ClockEmployeePunchId,
                    EmployeeId      = x.EmployeeId,
                    CostCenterId    = x.ClientCostCenterId,
                    ModifiedPunch   = x.ModifiedPunch,
                    ShiftDate       = x.ShiftDate
                }).ToList);

            if(approvalOption == ApprovalOptionType.Exceptions || approvalOption == ApprovalOptionType.AllActivity)
            {
                var exceptions = result.TryGetData(_session.UnitOfWork.LaborManagementRepository.ClockEmployeeExceptionHistoryQuery()
                    .ByClientId(clientId)
                    .ByEventDateFrom(minPeriodStart)
                    .ByEventDateTo(maxPeriodEnd)
                    .ExecuteQueryAs(x => new ExceptionEventToBeApproved
                    {
                        ClockEmployeeExceptionHistoryId = x.ClockEmployeeExceptionHistoryId,
                        EmployeeId                      = x.EmployeeId.Value,
                        EventDate                       = x.EventDate.Value,
                        ClockExceptionTypeId            = x.ClockExceptionTypeId,
                        ClockEmployeePunchId            = x.ClockEmployeePunchId
                    }).ToList);

                //only get exceptions that have punch activity on a given day
                var uniqueEmployeePunchDates = punches.Select(x => new { x.EmployeeId, x.EventDate.Date }).Distinct().ToList();

                eventsToApprove.AddRange(exceptions.Where(x => uniqueEmployeePunchDates.Any(p => p.EmployeeId == x.EmployeeId && p.Date == x.EventDate.Date)).ToList());
            }
            else
            {
                eventsToApprove.AddRange(punches);

                var benefits = result.TryGetData(_session.UnitOfWork.LaborManagementRepository.ClockEmployeeBenefitQuery()
                    .ByClientId(clientId)
                    .ByDateRange(minPeriodStart, maxPeriodEnd)
                    .ExecuteQueryAs(x => new EmployeeBenefitEventToBeApproved
                    {
                        EmployeeBenefitId = x.ClockEmployeeBenefitId,
                        EmployeeId        = x.EmployeeId,
                        CostCenterId      = x.ClientCostCenterId,
                        EventDate         = x.EventDate.Value
                    }).ToList);

                eventsToApprove.AddRange(benefits);
            }
            

            var pendingTimeOff = result.TryGetData(_session.UnitOfWork.LeaveManagementRepository.QueryTimeOffRequestDetails()
                .ByClientId(clientId)
                .ByStatus(TimeOffStatusType.Pending)
                .ByDateRangeFrom(minPeriodStart)
                .ByDateRangeTo(maxPeriodEnd)
                .ExecuteQueryAs(x => new TimeOffEventToBeApproved
                {
                    EmployeeId             = x.EmployeeId,
                    TimeOffRequestDetailId = x.TimeOffRequestDetailId,
                    TimeOffRequestId       = x.TimeOffRequestId,
                    RequestDate            = x.RequestDate,
                    Status                 = x.TimeOffRequest.Status
                }).ToList);

            eventsToApprove.AddRange(pendingTimeOff);

            var employeeEvents = eventsToApprove.GroupBy(x => x.EmployeeId).Select(g => new
            {
                EmployeeId = g.Key,
                Events = g.ToList()
            }).ToDictionary(x => x.EmployeeId, x => x.Events);

            var approvalStatuses = result.TryGetData(() =>_session.UnitOfWork.LaborManagementRepository.ClockEmployeeApproveDateQuery()
                .ByClientId(clientId)
                .ByEventDateFrom(minPeriodStart)
                .ByEventDateTo(maxPeriodEnd)
                .ExecuteQueryAs(x => new ClockApprovalDayDto
                {
                    EmployeeId                 = x.EmployeeId,
                    ClockEmployeeApproveDateId = x.ClockEmployeeApproveDateId,
                    CostCenterId               = x.ClientCostCenterId,
                    IsApproved                 = x.IsApproved ?? false,
                    EventDate                  = x.EventDate
                }).ToList());


            foreach(var ee in clockEmployees)
            {
                var events = employeeEvents.ContainsKey(ee.EmployeeId) 
                    ? employeeEvents[ee.EmployeeId]
                        .Where(x => x.EventDate.Date >= ee.PeriodStart.Date && x.EventDate.Date <= ee.PeriodEnd.Date)
                        .Select(x =>
                        {
                            x.CostCenterId = x.CostCenterId ?? ee.ClientCostCenterId;
                            return x;
                        })
                        .ToList() 
                    : null;

                if(events == null || !events.Any())
                {
                    ee.IsApproved = true;
                    continue;
                }

                //breakdown events by day (and cost-center if applicable)
                ee.ApprovalDates = new List<ClockApprovalDayDto>();
                var eeStatuses = approvalStatuses.Where(x => x.EmployeeId == ee.EmployeeId).ToList();

                if(approvalOption != ApprovalOptionType.CostCenter)
                {
                    //approve by date only
                    foreach(var d in events.GroupBy(x => x.EventDate.Date))
                    {
                        var approvalStatus = eeStatuses.FirstOrDefault(x => x.EventDate.Date == d.Key) ?? new ClockApprovalDayDto
                        {
                            EmployeeId = ee.EmployeeId,
                            EventDate  = d.Key,
                            IsApproved = false
                        };

                        approvalStatus.Events = d.ToList();
                        ee.ApprovalDates.Add(approvalStatus);
                    }
                }
                else
                {
                    //approve by date and cost center
                    foreach(var d in events.GroupBy(x => new { x.EventDate.Date, x.CostCenterId }))
                    {
                        var approvalStatus = eeStatuses.FirstOrDefault(x => x.EventDate.Date == d.Key.Date && x.CostCenterId == d.Key.CostCenterId) ?? new ClockApprovalDayDto
                        {
                            EmployeeId   = ee.EmployeeId,
                            EventDate    = d.Key.Date,
                            CostCenterId = d.Key.CostCenterId,
                            IsApproved   = false
                        };

                        approvalStatus.Events = d.ToList();
                        ee.ApprovalDates.Add(approvalStatus);
                    }
                }
                

                ee.IsApproved = ee.ApprovalDates.All(x => x.IsApproved) && events.OfType<TimeOffEventToBeApproved>().All(x => x.Status != TimeOffStatusType.Pending);
            }

            //userid
            //clientid
            //employeeid = 0
            //paytype = 3
            //employeestatusid = 0
            
            //check department settings
            var deptConfig = _clientSettings.GetClientDepartmentSetupInfo(clientId).MergeInto(result).Data;

            //now that we have employees and supers figure out who can see who
            //this mimics fnGetEmployeesByUserIDAndEmulation
            //which calls fnGetEmployeesByUserID (for any supervisor that can emulate another supervisor)
            //both only match on employees and departments from the supervisor security screen
            var supervisors = result.TryGetData(() =>
            {
                var q = _session.UnitOfWork.UserRepository.QuerySupervisorSecurityGroupAccess()
                    .ByCanApproveHours(true)
                    .ByUserType(UserType.Supervisor)
                    .ByClientId(clientId);

                if(isSuper)
                    q.ByUserId(userId.Value);

                return q.ExecuteQueryAs(x => new
                    {
                        x.UserId,
                        x.User.ViewEmployeePayTypes,
                        x.UserSecurityGroupId,
                        x.ForeignKeyId,

                        UserEmployeeId = x.User.EmployeeId,
                        EmployeeInfo = x.User.Employee != null ? new 
                        {
                            x.User.Employee.FirstName,
                            x.User.Employee.MiddleInitial,
                            x.User.Employee.LastName
                        } : null
                    })
                    .Where(x => x.EmployeeInfo != null)
                    .GroupBy(x => x.UserId)
                    .Select(g =>
                    {
                        var employeesIds = new Collection<int>();
                        var departmentIds = new Collection<int>();
                        var emulatedSuperIds = new Collection<int>();

                        foreach(var i in g)
                        {
                            switch (i.UserSecurityGroupId)
                            {
                                case UserSecurityGroupType.None:
                                    break;
                                case UserSecurityGroupType.Employee:
                                    employeesIds.Add(i.ForeignKeyId);
                                    break;
                                case UserSecurityGroupType.ClientDepartment:
                                    departmentIds.Add(i.ForeignKeyId);
                                    break;
                                case UserSecurityGroupType.Report:
                                    break;
                                case UserSecurityGroupType.ClientCostCenter:
                                    break;
                                case UserSecurityGroupType.UserEmulation:
                                    emulatedSuperIds.Add(i.ForeignKeyId);
                                    break;
                                default:
                                    break;
                            }
                        }

                        var user = g.First();

                        var employees = new List<EmployeeClockApprovalStatusDto>();
                        if(deptConfig.UseDepartmentsAcrossDivisions)
                        {
                            var codes = departmentIds.Select(id => deptConfig.DepartmentCodeLookup[id]).Distinct().ToList();
                            employees = clockEmployees.Where(x => 
                                CanViewEmployeeByPayType(user.ViewEmployeePayTypes, x.PayType) &&
                                ((!string.IsNullOrWhiteSpace(x.ClientDepartmentCode) && codes.Any(c => c.Equals(x.ClientDepartmentCode, StringComparison.OrdinalIgnoreCase))) ||
                                 employeesIds.Contains(x.EmployeeId))).Select(x =>
                            {
                                x.HasSupervisor = true;
                                return x;
                            }).ToList();
                        }
                        else
                        {
                            employees = clockEmployees.Where(x => 
                                CanViewEmployeeByPayType(user.ViewEmployeePayTypes, x.PayType) &&
                                ((x.ClientDepartmentId.HasValue && departmentIds.Contains(x.ClientDepartmentId.Value)) ||
                                 employeesIds.Contains(x.EmployeeId))).Select(x =>
                            {
                                x.HasSupervisor = true;
                                return x;
                            }).ToList();
                        }

                        var super = new SupervisorApprovalStatus
                        {
                            UserId        = user.UserId,

                            EmployeePayTypeAccess = user.ViewEmployeePayTypes,
                            
                            EmployeeId    = user.UserEmployeeId,
                            FirstName     = user.EmployeeInfo?.FirstName,
                            LastName      = user.EmployeeInfo?.LastName,
                            MiddleInitial = user.EmployeeInfo?.MiddleInitial,

                            EmulatedSupervisorIds = emulatedSuperIds,

                            EmployeeApprovals = employees
                        };

                        return super;
                    })
                    .ToList();
            });

            //handle supervisor emulation and overall approval status
            foreach(var s in supervisors)
            {
                //add any emulated supervisor's employees
                foreach(var emulatedId in s.EmulatedSupervisorIds)
                {
                    var emulatedSupervisor = supervisors.FirstOrDefault(x => x.UserId == emulatedId);
                    if(emulatedSupervisor != null)
                    {
                        foreach(var ee in emulatedSupervisor.EmployeeApprovals)
                        {
                            //only add employees the supervisor has pay-type access to
                            if(CanViewEmployeeByPayType(s.EmployeePayTypeAccess, ee.PayType))
                                s.EmployeeApprovals.Add(ee);
                        }
                    }
                }

                s.IsApproved = s.EmployeeApprovals.All(x => x.IsApproved);
            }

            var dto = new SupervisorStatusResultDto
            {
                Supervisors = supervisors,
                UnassignedEmployeeStatuses = clockEmployees.Where(x => !x.HasSupervisor).ToList()
            };

            result.SetDataOnSuccess(dto);

            return result;
        }

        private bool CanViewEmployeeByPayType(UserViewEmployeePayType viewAccess, PayType? payType)
        {
            if(viewAccess == UserViewEmployeePayType.Both)
                return true;

            if(viewAccess == UserViewEmployeePayType.None || payType == null)
                return false;

            var iPayType = (int)payType.Value;
            var iViewAccess = (int)viewAccess;

            return iPayType == iViewAccess;
        }
    }
}