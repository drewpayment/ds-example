using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Labor;
using Dominion.Domain.Entities.TimeClock;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.Utility.Mapping;

namespace Dominion.LaborManagement.Service.Mapping.Clock
{
    public class ClockExceptionMaps
    {
        public class ToClockExceptionDto : ExpressionMapper<ClockExceptionTypeInfo, ClockExceptionTypeInfoDto>
        {
            public override Expression<Func<ClockExceptionTypeInfo, ClockExceptionTypeInfoDto>> MapExpression
            {
                get
                {
                    return x => new ClockExceptionTypeInfoDto()
                    {
                        ClockExceptionId = x.ClockExceptionTypeId,
                        ClockException = x.ClockException,
                        IsHours = x.IsHours,
                        HasPunchTimeOption = x.IsHasPunchTimeOption,
                        HasAmountTextBox = x.IsHasAmountTextBox,
                        ClockExceptionType = x.ClockExceptionType
                    };
                }
            }
        }

        public class ToClientExceptionDetailDto : ExpressionMapper<ClockExceptionTypeInfo, ClientExceptionDetailDto>
        {
            public override Expression<Func<ClockExceptionTypeInfo, ClientExceptionDetailDto>> MapExpression
            {
                get
                {
                    return x => new ClientExceptionDetailDto()
                    {
                        ClockExceptionId = x.ClockExceptionTypeId,
                        ClockClientExceptionId = null,
                        ClockClientExceptionDetailId = null,
                        ClockClientLunchId = null,
                        ExceptionName = x.ClockException,
                        PunchTimeOption = PunchType.Rounded,
                        Amount = 0,
                        HasAmountTextBox = x.IsHasAmountTextBox,
                        HasPunchTimeOption = x.IsHasPunchTimeOption,
                        IsSelected = false,
                        IsHour = (x.IsHours == true),
                        ClockExceptionType = x.ClockExceptionType
                    };
                }
            }
        }
    }
}
