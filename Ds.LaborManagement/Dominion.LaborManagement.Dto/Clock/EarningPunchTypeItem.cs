using Dominion.Core.Dto.Payroll;

namespace Dominion.LaborManagement.Dto.Clock
{
    public class EarningPunchTypeItem : PunchTypeItem
    {
        public override int?                  Id              => ClientEarningId;
        public override string                Name            => $"{Description} ({Code})";
        public          int?                  ClientEarningId { get; set; }
        public          ClientEarningCategory EarningCategory { get; set; }
        public          string                Description     { get; set; }
        public          string                Code            { get; set; }
    }
}