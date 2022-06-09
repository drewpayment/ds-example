using System;
using System.Linq.Expressions;
using Dominion.Core.Dto.Labor;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Mapping;

namespace Dominion.LaborManagement.Service.Mapping
{
    public class ClockClientTimePolicyMaps
    {
        public class ToTimePolicyListDto :
            ExpressionMapper<ClockClientTimePolicy, ClockClientTimePolicyDtos.ClockClientTimePolicyListDto>
        {
            public override Expression<Func<ClockClientTimePolicy, ClockClientTimePolicyDtos.ClockClientTimePolicyListDto>> MapExpression
            {
                get
                {
                    return x => new ClockClientTimePolicyDtos.ClockClientTimePolicyListDto()
                    {
                       ClientId = x.ClientId,
                       ClockClientTimePolicyId = x.ClockClientTimePolicyId,
                       Name = x.Name,
                       GeofenceEnabled = x.GeofenceEnabled,
                      
                    };
                }

            }
        }

        public class ToClockClientTimePolicyDto :
            ExpressionMapper<ClockClientTimePolicy, ClockClientTimePolicyDtos.ClockClientTimePolicyDto>
        {
            public override Expression<Func<ClockClientTimePolicy, ClockClientTimePolicyDtos.ClockClientTimePolicyDto>> MapExpression
            {
                get
                {
                    return x => new ClockClientTimePolicyDtos.ClockClientTimePolicyDto()
                    {
                        ClockClientTimePolicyId = x.ClockClientTimePolicyId,
                        ClientId = x.ClientId,
                        Name = x.Name,
                        ClientDepartmentId = x.ClientDepartmentId,
                        ClientShiftId = x.ClientShiftId,
                        ClientStatusId = x.ClientStatusId,
                        ClockClientExceptionId = x.ClockClientExceptionId,
                        ClockClientHolidayId = x.ClockClientHolidayId,
                        ClockClientRulesId = x.ClockClientRulesId,
                        HasCombinedOtFrequencies = x.HasCombinedOtFrequencies,
                        IsAddToOtherPolicy = x.IsAddToOtherPolicy,
                        PayType = x.PayType,
                        TimeZoneId = x.TimeZoneId,
                        AutoPointsEnabled = x.AutoPointsEnabled,
                        ShowTCARatesEnabled = x.ShowTCARatesEnabled,
                        GeofenceEnabled = x.GeofenceEnabled,
                    };
                }

            }
        }
    }
}
