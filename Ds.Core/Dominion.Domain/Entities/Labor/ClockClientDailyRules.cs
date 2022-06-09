using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Entities.TimeClock;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    public class ClockClientDailyRules : Entity<ClockClientDailyRules>, IHasModifiedData
    {
        public virtual int ClockClientDailyRulesId { get; set; }
        public virtual int ClockClientRulesId { get; set; }
        public virtual byte DayOfWeekId { get; set; }
        public virtual int? ClientEarningId { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual double? MinHoursWorked { get; set; }
        public virtual bool IsApplyOnlyIfMinHoursMetPrior { get; set; }

        public ClockClientRules ClockClientRule { get; set; }
        public ClientEarning ClientEarning { get; set; }
    }
}
