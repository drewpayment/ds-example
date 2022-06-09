using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Clients
{
    public partial class ClientShift : Entity<ClientShift>, IHasModifiedUserNameData
    {
        public virtual int       ClientShiftId           { get; set; }
        public virtual int       ClientId                { get; set; }
        public virtual string    Description             { get; set; }
        public virtual double    AdditionalAmount        { get; set; }
        public virtual int       AdditionalAmountTypeId  { get; set; }
        public virtual int       Destination             { get; set; }
        public virtual DateTime  Modified                { get; set; }
        public virtual string    ModifiedBy              { get; set; }
        public virtual double?   Limit                   { get; set; }
        public virtual DateTime? StartTime               { get; set; }
        public virtual DateTime? StopTime                { get; set; }
        public virtual double?   AdditionalPremiumAmount { get; set; }
        public virtual bool      IsSunday                { get; set; }
        public virtual bool      IsMonday                { get; set; }
        public virtual bool      IsTuesday               { get; set; }
        public virtual bool      IsWednesday             { get; set; }
        public virtual bool      IsThursday              { get; set; }
        public virtual bool      IsFriday                { get; set; }
        public virtual bool      IsSaturday              { get; set; }
        public virtual decimal?  ShiftStartTolerance     { get; set; }
        public virtual decimal?  ShiftEndTolerance       { get; set; }
        public virtual Client    Client                  { get; set; }
        public virtual ICollection<ClockClientTimePolicy> TimePolicies { get; set; }
        public virtual ICollection<ClientShiftSelected> ShiftSelected { get; set; }
    }
}
