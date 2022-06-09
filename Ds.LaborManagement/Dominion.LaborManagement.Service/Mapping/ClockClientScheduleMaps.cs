using Dominion.Core.Dto.Labor;
using Dominion.Domain.Entities.TimeClock;
using Dominion.Utility.Mapping;
using System;
using System.Linq.Expressions;

namespace Dominion.LaborManagement.Service.Mapping.Clock
{
    public class ClockClientScheduleMaps
    {
        public class ToClockClientScheduleDto : ExpressionMapper<ClockClientSchedule, ClockClientScheduleDto>
        {
            public override Expression<Func<ClockClientSchedule, ClockClientScheduleDto>> MapExpression
            {
                get
                {
                    return  x => new ClockClientScheduleDto()
                    {
                        ClientId = x.ClientId,
                        Name = x.Name,
                        ClientDepartmentId = x.ClientDepartmentId,
                        ClientShiftId = x.ClientShiftId,
                        ClientStatusId = x.ClientStatusId,
                        ClockClientScheduleId = x.ClockClientScheduleId,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        StartTime = x.StartDate,
                        StopTime = x.StopTime,
                        IsMonday = x.IsMonday,
                        IsTuesday = x.IsTuesday,
                        IsWednesday = x.IsWednesday,
                        IsThursday = x.IsThursday,
                        IsFriday = x.IsFriday,
                        IsSaturday = x.IsSaturday,
                        IsSunday = x.IsSunday,
                        IsIncludeOnHolidays = x.IsIncludeOnHolidays,
                        IsOverrideSchedules = x.IsOverrideSchedules,
                        PayType = x.PayType,
                        RecurEveryOption = x.RecurEveryOption,
                        RecurrenceOption = x.RecurrenceOption,
                        RepeatInterval = x.RepeatInterval
                    };
                }
            }
        }
    }
}