using System;

namespace Dominion.Core.Dto.Banks
{
    public partial class BankHolidayDto
    {
        public int      BankHolidayId { get; set; }
        public DateTime Date          { get; set; }
        public string   Name          { get; set; }
        public DateTime Modified      { get; set; }
        public int      ModifiedBy    { get; set; }
    }
}
