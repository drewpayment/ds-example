using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Dominion.Core.Dto.Labor;
using Dominion.Domain.Entities.TimeClock;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.LaborManagement.Dto.EmployeeLaborManagement;
using Dominion.Utility.Mapping;

namespace Dominion.LaborManagement.Service.Mapping.Clock
{
    public class ClockClientLunchMaps
    {
        public class ToClockClientLunchDto : ExpressionMapper<ClockClientLunch, ClockClientLunchDto>
        {
            public override Expression<Func<ClockClientLunch, ClockClientLunchDto>> MapExpression
            {
                get
                {
                    return x => new ClockClientLunchDto()
                    {
                        ClockClientLunchId = x.ClockClientLunchId,
                        ClientId = x.ClientId,
                        AllPunchesClockRoundingTypeId = x.AllPunchesClockRoundingTypeId,
                        AllPunchesGraceTime = x.AllPunchesGraceTime,
                        AutoDeductedWorkedHours = x.AutoDeductedWorkedHours,
                        ClientCostCenterId = x.ClientCostCenterId,
                        GraceTime = x.GraceTime,
                        Name = x.Name,
                        StopTime = x.StopTime,
                        InLateClockRoundingTypeId = x.InLateClockRoundingTypeId,
                        InEarlyClockRoundingTypeId = x.InEarlyClockRoundingTypeId,
                        InEarlyGraceTime = x.InEarlyGraceTime,
                        InLateGraceTime = x.InLateGraceTime,
                        IsAllowMultipleTimePeriods = x.IsAllowMultipleTimePeriods,
                        IsAutoDeducted = x.IsAutoDeducted,
                        IsDoEmployeesPunch = x.IsDoEmployeesPunch,
                        IsMonday = x.IsMonday,
                        IsTuesday = x.IsTuesday,
                        IsWednesday = x.IsWednesday,
                        IsThursday = x.IsThursday,
                        IsFriday = x.IsFriday,
                        IsSaturday = x.IsSaturday,
                        IsSunday = x.IsSunday,
                        IsShowPunches = x.IsShowPunches,
                        IsUseStartStopTimes = x.IsUseStartStopTimes,
                        Length = x.Length,
                        OutEarlyClockRoundingTypeId = x.OutEarlyClockRoundingTypeId,
                        OutLateGraceTime = x.OutLateGraceTime,
                        OutLateClockRoundingTypeId = x.OutLateClockRoundingTypeId,
                        OutEarlyGraceTime = x.OutEarlyGraceTime,
                        PunchType = x.PunchType,
                        StartTime = x.StartTime,
                        IsPaid = x.IsPaid,
                        IsMaxPaid = x.IsMaxPaid,
                        MinutesToRestrictLunchPunch = x.MinutesToRestrictLunchPunch,
                        TimePolicies = x.LunchSelected.Where(l => l.TimePolicy != null)
                            .Select(t => new ClockClientTimePolicyDtos.ClockClientTimePolicyDto
                            {
                                ClockClientTimePolicyId = t.TimePolicy.ClockClientTimePolicyId,
                                ClientId = t.TimePolicy.ClientId,
                                ClockClientExceptionId = t.TimePolicy.ClockClientExceptionId,
                                Name = t.TimePolicy.Name,
                                ClockClientHolidayId = t.TimePolicy.ClockClientHolidayId,
                                ClockClientRulesId = t.TimePolicy.ClockClientRulesId,
                                ClientDepartmentId = t.TimePolicy.ClientDepartmentId,
                                ClientShiftId = t.TimePolicy.ClientShiftId,
                                ClientStatusId = t.TimePolicy.ClientStatusId,
                                IsAddToOtherPolicy = t.TimePolicy.IsAddToOtherPolicy,
                                PayType = t.TimePolicy.PayType,
                                TimeZoneId = t.TimePolicy.TimeZoneId
                            })
                    };
                }
            }
        }
    }
}