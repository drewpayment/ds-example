using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Labor
{
    public partial class ClockClientAddHours : Entity<ClockClientAddHours>, IHasModifiedData
    {
        public virtual int      ClockClientAddHoursId { get; set; }
        public virtual int      ClientId              { get; set; }
        public virtual string   Name                  { get; set; }
        public virtual int      CalculationFrequency  { get; set; }
        public virtual double   TimeWorkedThreshold   { get; set; }
        public virtual double   Award                 { get; set; }
        public virtual int      ClientEarningId       { get; set; }
        public virtual bool?    IsSunday              { get; set; }
        public virtual bool?    IsMonday              { get; set; }
        public virtual bool?    IsTuesday             { get; set; }
        public virtual bool?    IsWednesday           { get; set; }
        public virtual bool?    IsThursday            { get; set; }
        public virtual bool?    IsFriday              { get; set; }
        public virtual bool?    IsSaturday            { get; set; }
        public virtual DateTime Modified              { get; set; }
        public virtual int      ModifiedBy            { get; set; }

        public virtual ICollection<ClockClientTimePolicy>       TimePolicies     { get; set; }
        public virtual ICollection<ClockClientAddHoursSelected> AddHoursSelected { get; set; }
    }
}
