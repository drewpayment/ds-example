namespace Dominion.Core.Dto.User
{
    public class OtherAccessClaimTypeDto : ClaimTypeDto
    {
        public override ClaimSource Source => ClaimSource.Other;
        public OtherAccessClaimType OtherAccessType { get; set; }
        public override bool IsClaimMatch(ClaimTypeDto claim)
        {
            return claim is OtherAccessClaimTypeDto typed && typed.OtherAccessType == OtherAccessType;
        }
    }
}