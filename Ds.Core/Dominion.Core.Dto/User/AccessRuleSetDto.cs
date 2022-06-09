using System;
using System.Collections.Generic;
using System.Linq;

namespace Dominion.Core.Dto.User
{
    public class AccessRuleSetDto : AccessRuleDto
    {
        public AccessRuleSetConditionType Operator { get; set; }

        public IEnumerable<AccessRuleDto> Rules { get; set; }

        public override bool HasAccess(IEnumerable<ClaimTypeDto> claims)
        {
            switch (Operator)
            {
                case AccessRuleSetConditionType.And:
                    return Rules.All(r => r.HasAccess(claims) == r.HasAccessWhen);
                case AccessRuleSetConditionType.Or:
                    return Rules.Any(r => r.HasAccess(claims) == r.HasAccessWhen);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}