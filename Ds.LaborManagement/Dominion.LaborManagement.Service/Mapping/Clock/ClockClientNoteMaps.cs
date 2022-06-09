using System;
using System.Linq.Expressions;
using Dominion.Core.Dto.Labor;
using Dominion.Domain.Entities.Labor;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.Utility.Mapping;

namespace Dominion.LaborManagement.Service.Mapping.Clock
{
    public class ClockClientNoteMaps
    {
        public class ToClockClientNoteDto : ExpressionMapper<ClockClientNote, ClockClientNoteDto>
        {
            public override Expression<Func<ClockClientNote, ClockClientNoteDto>> MapExpression
            {
                get
                {
                    return x => new ClockClientNoteDto
                    {
                        ClientId           = x.ClientId,
                        ClockClientNoteId  = x.ClockClientNoteId,
                        /*Note               = x.IsActive ? x.Note : x.Note + " -- Inactive",*/
                        Note               = x.Note,
                        IsHideFromEmployee = x.IsHideFromEmployee,
                        IsActive           = x.IsActive
                    };
                }
            }
        }
    }
}
