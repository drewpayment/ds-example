namespace Dominion.LaborManagement.Dto.Clock
{
    public class PunchTypeItem
    {
        public virtual int? Id { get; set; }
        public virtual string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}