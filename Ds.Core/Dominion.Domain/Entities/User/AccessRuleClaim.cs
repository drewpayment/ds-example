using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.User
{
    public class AccessRuleClaim : Entity<AccessRuleClaim>
    {
        public int RuleId { get; set; }
        public int ClaimTypeId { get; set; }

        public AccessRule Rule { get; set; }
        public ClaimType ClaimType { get; set; }
    }
}