using System.Collections.Generic;
using System.Linq;

namespace Dominion.Core.Dto.User
{
    public abstract class AccessRuleDto
    {
        public int? RuleId { get; set; }
        public string Name { get; set; }
        public bool HasAccessWhen { get; set; }
        public abstract bool HasAccess(IEnumerable<ClaimTypeDto> claims);
    }
}