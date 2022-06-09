using System;
using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    /// <summary>
    /// Entity representation of a dbo.ClockClientHoliday record.
    /// </summary>
    public partial class ClockClientHoliday : Entity<ClockClientHoliday>, IHasModifiedData
    {
        public virtual int      ClockClientHolidayId              { get; set; } 
        public virtual int      ClientId                          { get; set; } 
        public virtual string   Name                              { get; set; } 
        public virtual int?     ClientEarningId                   { get; set; } 
        public virtual DateTime Modified                          { get; set; } 
        public virtual int      ModifiedBy                        { get; set; } 
        public virtual double?  Hours                             { get; set; } 
        public virtual int?     HolidayWorkedClientEarningId      { get; set; } 
        public virtual int      WaitingPeriod                     { get; set; } 
        public virtual int      HolidayWaitingPeriodDateId        { get; set; } 

        public virtual Client        Client        { get; set; }
        public virtual ClientEarning ClientEarning { get; set; }
        public virtual ClientEarning HolidayWorkedClientEarning { get; set; }
        public virtual ICollection<ClockClientHolidayDetail> ClientHolidayDetails { get; set; } 
    }
}
