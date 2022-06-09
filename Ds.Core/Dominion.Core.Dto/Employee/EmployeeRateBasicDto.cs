using System;

namespace Dominion.Core.Dto.Employee
{
    public class EmployeeRateBasicDto
    {
        public int       ClientRateId         { get; set; }
        public int       EmployeeClientRateId { get; set; }
        public string    RateName             { get; set; }
        public double?   RateAmount           { get; set; }
        public DateTime? RateEffectiveDate    { get; set; }
        public bool      IsDefaultRate        { get; set; }
        
    }
}
