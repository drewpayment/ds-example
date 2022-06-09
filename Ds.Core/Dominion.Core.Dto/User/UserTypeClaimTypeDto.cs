namespace Dominion.Core.Dto.User
{
    public class UserTypeClaimTypeDto : ClaimTypeDto
    {
        public override ClaimSource Source => ClaimSource.UserType;
        public UserType UserType { get; set; }
        public override bool IsClaimMatch(ClaimTypeDto claim)
        {
            return claim is UserTypeClaimTypeDto typed && typed.UserType == UserType;
        }
    }
}