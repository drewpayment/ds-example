using Dominion.Core.Dto.Labor;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Mapping;
using System;
using System.Linq.Expressions;

namespace Dominion.LaborManagement.Service.Mapping.Clock
{
    public class ClockClientOvertimeMaps
    {
        public class ToClockClientOvertimeDto : ExpressionMapper<ClockClientOvertime, ClockClientOvertimeDto>
        {
            public override Expression<Func<ClockClientOvertime, ClockClientOvertimeDto>> MapExpression
            {
                get
                {
                    return x => new ClockClientOvertimeDto
                    {
                        ClientId = x.ClientId,
                        ClientEarningId = x.ClientEarningId,
                        ClockOvertimeFrequencyId = x.ClockOvertimeFrequencyId,
                        ClockClientOvertimeId = x.ClockClientOvertimeId,
                        Hours = x.Hours,
                        Name = x.Name,
                        IsSunday = x.IsSunday,
                        IsMonday = x.IsMonday,
                        IsTuesday = x.IsTuesday,
                        IsWednesday = x.IsWednesday,
                        IsThursday = x.IsThursday,
                        IsFriday = x.IsFriday,
                        IsSaturday = x.IsSaturday,
                        Modified = x.Modified,
                        ModifiedBy = x.ModifiedBy
                    };
                }
            }
        }
    }
}
