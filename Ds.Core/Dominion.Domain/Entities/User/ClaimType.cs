using System.Collections.Generic;
using Dominion.Core.Dto.Misc;
using Dominion.Core.Dto.User;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.User
{
    public class ClaimType : Entity<ClaimType>
    {
        public int ClaimTypeId { get; set; }
        public AccountFeatureEnum? AccountFeatureId { get; set; }
        public int? UserActionTypeId { get; set; }
        public UserType? UserTypeId { get; set; }
        public OtherAccessClaimType? OtherAccessType { get; set; }
        public UserActionType UserActionType { get; set; }

        public ICollection<AccessRuleClaim> ClaimRules { get; set; }
    }
}