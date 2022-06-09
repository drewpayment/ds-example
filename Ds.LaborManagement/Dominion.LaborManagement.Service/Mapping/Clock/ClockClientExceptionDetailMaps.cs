using Dominion.Domain.Entities.TimeClock;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.Utility.Mapping;
using System;
using System.Linq.Expressions;
using Dominion.Core.Dto.Labor;

namespace Dominion.LaborManagement.Service.Mapping.Clock
{
    public class ClockClientExceptionDetailMaps
    {
        public class ToClockClientExceptionDetailDto : ExpressionMapper<ClockClientExceptionDetail, ClockClientExceptionDetailDto>
        {
            public override Expression<Func<ClockClientExceptionDetail, ClockClientExceptionDetailDto>> MapExpression
            {
                get
                {
                    return x => new ClockClientExceptionDetailDto()
                    {
                        ClockClientExceptionDetailId = x.ClockClientExceptionDetailId,
                        ClockClientExceptionId = x.ClockClientExceptionId,
                        ClockClientLunchId = x.ClockClientLunchId,
                        ClockExceptionId = x.ClockExceptionId,
                        Amount = x.Amount,
                        IsSelected = x.IsSelected,
                        PunchTimeOption = x.PunchTimeOption ?? PunchType.Rounded
                    };
                }
            }
        }
    }
}
