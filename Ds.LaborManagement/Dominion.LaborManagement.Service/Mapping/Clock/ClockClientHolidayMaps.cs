using System;
using System.Linq;
using System.Linq.Expressions;
using Dominion.Core.Dto.Labor;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.TimeClock;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.Utility.Mapping;

namespace Dominion.LaborManagement.Service.Mapping.Clock
{
    public class ClockClientHolidayMaps
    {
        public class ToClockClientHolidayDto : ExpressionMapper<ClockClientHoliday, ClockClientHolidayDto>
        {
            public override Expression<Func<ClockClientHoliday, ClockClientHolidayDto>> MapExpression
            {
                get
                {
                    return x => new ClockClientHolidayDto()
                    {
                        ClientId = x.ClientId,
                        Name = x.Name,
                        ClockClientHolidayId = x.ClockClientHolidayId,
                        ClientEarningId = x.ClientEarningId,
                        HolidayWaitingPeriodDateId = x.HolidayWaitingPeriodDateId,
                        HolidayWorkedClientEarningId = x.HolidayWorkedClientEarningId,
                        Hours = x.Hours,
                        WaitingPeriod = x.WaitingPeriod
                    };
                }
            }
        }

        public class ToClockClientHolidayDetailDto : ExpressionMapper<ClockClientHolidayDetail, ClockClientHolidayDetailDto>
        {
            public override Expression<Func<ClockClientHolidayDetail, ClockClientHolidayDetailDto>> MapExpression
            {
                get
                {
                    return x => new ClockClientHolidayDetailDto()
                    {
                        ClockClientHolidayId = x.ClockClientHolidayId,
                        ClockClientHolidayDetailId = x.ClockClientHolidayDetailId,
                        ClientHolidayName = x.ClientHolidayName,
                        EventDate = x.EventDate,
                        IsPaid = x.IsPaid,
                        OverrideHours = x.OverrideHours,
                        OverrideClientEarningId = x.OverrideClientEarningId,
                        OverrideHolidayWorkedClientEarningId = x.OverrideHolidayWorkedClientEarningId
                    };
                }
            }
        }

        public class ToHolidayDtoWithDetail : ExpressionMapper<ClockClientHoliday, ClockClientHolidayDto>
        {
            public override Expression<Func<ClockClientHoliday, ClockClientHolidayDto>> MapExpression
            {
                get
                {
                    return x => new ClockClientHolidayDto()
                    {
                        ClientId = x.ClientId,
                        Name = x.Name,
                        ClockClientHolidayId = x.ClockClientHolidayId,
                        ClientEarningId = x.ClientEarningId,
                        HolidayWaitingPeriodDateId = x.HolidayWaitingPeriodDateId,
                        HolidayWorkedClientEarningId = x.HolidayWorkedClientEarningId,
                        Hours = x.Hours,
                        WaitingPeriod = x.WaitingPeriod,
                        ClientHolidayDetails = x.ClientHolidayDetails.Select(d => new ClockClientHolidayDetailDto
                        {
                            ClockClientHolidayDetailId = d.ClockClientHolidayDetailId,
                            ClockClientHolidayId = d.ClockClientHolidayId,
                            ClientHolidayName = d.ClientHolidayName,
                            EventDate = d.EventDate,
                            IsPaid = d.IsPaid,
                            OverrideHours = d.OverrideHours,
                            OverrideClientEarningId = d.OverrideClientEarningId,
                            OverrideHolidayWorkedClientEarningId = d.OverrideHolidayWorkedClientEarningId
                        })
                    };
                }
            }
        }
    }
}