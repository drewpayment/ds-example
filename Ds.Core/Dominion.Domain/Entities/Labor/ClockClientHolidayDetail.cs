using System;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Payroll;

namespace Dominion.Domain.Entities.Labor
{
    /// <summary>
    /// Entity representation of a dbo.ClockClientHolidayDetail record.
    /// </summary>
    public partial class ClockClientHolidayDetail : Entity<ClockClientHolidayDetail>
    {
        public virtual int      ClockClientHolidayDetailId           { get; set; } 
        public virtual int      ClockClientHolidayId                 { get; set; } 
        public virtual string   ClientHolidayName                    { get; set; } 
        public virtual bool     IsPaid                               { get; set; } 
        public virtual DateTime EventDate                            { get; set; } 
        public virtual double?  OverrideHours                        { get; set; } 
        public virtual int?     OverrideClientEarningId              { get; set; }
        public virtual int?     OverrideHolidayWorkedClientEarningId { get; set; }

        public virtual ClockClientHoliday ClockClientHoliday { get; set; }
        
    }
}