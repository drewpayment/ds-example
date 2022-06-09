using Dominion.Domain.Entities.TimeClock;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.Utility.Mapping;
using System;
using System.Linq.Expressions;
using Dominion.Core.Dto.Labor;

namespace Dominion.LaborManagement.Service.Mapping.Clock
{
    public class ClockClientExceptionMaps
    {
        public class ToClockClientExceptionDto : ExpressionMapper<ClockClientException, ClockClientExceptionDto>
        {
            public override Expression<Func<ClockClientException, ClockClientExceptionDto>> MapExpression
            {
                get
                {
                    return x => new ClockClientExceptionDto()
                    {
                        ClockClientExceptionId = x.ClockClientExceptionId,
                        ClientId = x.ClientId,
                        Name = x.Name,
                        ModifiedBy = x.ModifiedBy,
                        Modified = x.Modified
                    };
                }
            }
        }
    }
}
