using System;
using Dominion.Core.Dto.Payroll;

namespace Dominion.Core.Dto.Labor
{
    public partial class ClockClientHolidayDetailDto : IHasClockClientHolidayDetailValidation
    {
        public virtual int      ClockClientHolidayDetailId           { get; set; }
        public virtual int      ClockClientHolidayId                 { get; set; }
        public virtual string   ClientHolidayName                    { get; set; }
        public virtual bool     IsPaid                               { get; set; }
        public virtual DateTime EventDate                            { get; set; }
        public virtual double?  OverrideHours                        { get; set; }
        public virtual int?     OverrideClientEarningId              { get; set; }
        public virtual int?     OverrideHolidayWorkedClientEarningId { get; set; }

        public virtual ClockClientHolidayDto ClockClientHoliday                 { get; set; }
        
    }
}
