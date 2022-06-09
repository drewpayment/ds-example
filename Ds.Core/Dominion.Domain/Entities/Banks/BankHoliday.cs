using Dominion.Domain.Entities.Base;
using System;

namespace Dominion.Domain.Entities.Banks
{
    public partial class BankHoliday : Entity<BankHoliday>
    {
        public virtual int      BankHolidayId { get; set; }
        public virtual DateTime Date          { get; set; }
        public virtual string   Name          { get; set; }
        public virtual DateTime Modified      { get; set; }
        public virtual int      ModifiedBy    { get; set; }
    }
}
