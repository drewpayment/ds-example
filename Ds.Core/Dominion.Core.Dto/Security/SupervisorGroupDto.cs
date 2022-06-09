namespace Dominion.Core.Dto.Security
{
    public class SupervisorGroupDto
    {
        public UserSecurityGroupType GroupType { get; set; }
        public int ForeignKeyId { get; set; }
        public bool IsEmulated { get; set; }
        public int? EmulatedUserId { get; set; }
    }
}