using Dominion.Core.Dto.Misc;

namespace Dominion.Core.Dto.User
{
    public class AccountFeatureClaimTypeDto : ClaimTypeDto
    {
        public override ClaimSource Source => ClaimSource.AccountFeature;
        public AccountFeatureEnum AccountFeature { get; set; }
        public override bool IsClaimMatch(ClaimTypeDto claim)
        {
            return claim is AccountFeatureClaimTypeDto typed && typed.AccountFeature == AccountFeature;
        }
    }
}