using System.Collections.Generic;
using Dominion.Core.Dto.Misc;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.User
{
    public class AccessRule : Entity<AccessRule>
    {
        public int    RuleId        { get; set; }
        public string Name          { get; set; }
        public bool   HasAccessWhen { get; set; }
        public AccessRuleClaim RuleClaim { get; set; }
        public AccessRuleSet RuleSetDetail { get; set; }
        public ICollection<AccessRuleSet> MemberOfRuleSets { get; set; }
    }
}
