using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Client
{
    public class ClientShiftDto
    {
        public int       ClientShiftId           { get; set; }
        public int       ClientId                { get; set; }
        public string    Description             { get; set; }
        public double    AdditionalAmount        { get; set; }
        public int       AdditionalAmountTypeId  { get; set; }
        public int       Destination             { get; set; }
        public DateTime  Modified                { get; set; }
        public string    ModifiedBy              { get; set; }
        public double?   Limit                   { get; set; }
        public DateTime? StartTime               { get; set; }
        public DateTime? StopTime                { get; set; }
        public double?   AdditionalPremiumAmount { get; set; }
        public bool      IsSunday                { get; set; }
        public bool      IsMonday                { get; set; }
        public bool      IsTuesday               { get; set; }
        public bool      IsWednesday             { get; set; }
        public bool      IsThursday              { get; set; }
        public bool      IsFriday                { get; set; }
        public bool      IsSaturday              { get; set; }
        public decimal?  ShiftStartTolerance     { get; set; }
        public decimal?  ShiftEndTolerance       { get; set; }

    }
}
