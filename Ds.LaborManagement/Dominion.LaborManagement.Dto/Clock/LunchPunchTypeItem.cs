namespace Dominion.LaborManagement.Dto.Clock
{
    public class LunchPunchTypeItem : PunchTypeItem
    {
        public override int? Id => ClockClientLunchId;
        public int? ClockClientLunchId { get; set; }
    }
}