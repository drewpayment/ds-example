using System.Collections.Generic;
using System.Linq;

namespace Dominion.Core.Dto.User
{
    public class AccessRuleClaimDto : AccessRuleDto
    {
        public ClaimTypeDto ClaimType { get; set; }

        public override bool HasAccess(IEnumerable<ClaimTypeDto> claims)
        { 
            return claims.Any(claim => ClaimType.IsClaimMatch(claim));
            //return HasAccessWhen = claims.Any(claim => ClaimType.IsClaimMatch(claim));
        }
    }
}