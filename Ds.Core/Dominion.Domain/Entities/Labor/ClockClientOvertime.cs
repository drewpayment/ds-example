using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Entities.TimeClock;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    public class ClockClientOvertime : Entity<ClockClientOvertime>, IHasModifiedData
    {
        public int ClockClientOvertimeId { get; set; }
        public string Name { get; set; }
        public int ClientId { get; set; }
        public int ClientEarningId { get; set; }
        public byte? ClockOvertimeFrequencyId { get; set; }
        public double? Hours { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
        public bool IsSunday { get; set; }
        public bool IsMonday { get; set; }
        public bool IsTuesday { get; set; }
        public bool IsWednesday { get; set; }
        public bool IsThursday { get; set; }
        public bool IsFriday { get; set; }
        public bool IsSaturday { get; set; }

        public virtual ClientEarning ClientEarning { get; set; }
        public virtual Client Client { get; set; }
        public virtual ClockOvertimeFrequency OvertimeFrequency { get; set; }
        public virtual ICollection<ClockClientTimePolicy> TimePolicies { get; set; }
        public virtual ICollection<ClockClientOvertimeSelected> OvertimeSelected { get; set; }

    }
}
