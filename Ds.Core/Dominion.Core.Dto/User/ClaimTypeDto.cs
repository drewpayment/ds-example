namespace Dominion.Core.Dto.User
{
    public abstract class ClaimTypeDto
    {
        public int?     ClaimTypeId { get; set; }
        public string   Name        { get; set; }
        public abstract ClaimSource Source { get; }
        public abstract bool IsClaimMatch(ClaimTypeDto claim);
    }
}