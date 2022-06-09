namespace Dominion.Core.Dto.User
{
    public class UserActionTypeClaimTypeDto : ClaimTypeDto
    {
        public override ClaimSource Source => ClaimSource.UserActionType;
        public int? UserActionTypeId { get; set; }
        public string Designation { get; set; }
        public string Description { get; set; }
        public override bool IsClaimMatch(ClaimTypeDto claim)
        {
            return claim is UserActionTypeClaimTypeDto typed && typed.Designation == Designation;
        }
    }
}