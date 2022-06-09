using System.Collections.Generic;
using Dominion.Core.Dto.User;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.User
{
    public class AccessRuleSet : Entity<AccessRuleSet>
    {
        public int RuleId { get; set; }
        public AccessRuleSetConditionType Condition { get; set; } 
        public AccessRule BaseRule { get; set; }
        public ICollection<AccessRule> Rules { get; set; }
    }
}