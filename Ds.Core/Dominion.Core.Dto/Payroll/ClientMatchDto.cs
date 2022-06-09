using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class ClientMatchDto
    {
        public int ClientMatchId { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public int? DeductionInId { get; set; }
        public int DeductionOutId { get; set; }
        public double Amount { get; set; }
        public byte AmountTypeId { get; set; }
        public double Maximum { get; set; }
        public byte MaximumTypeId { get; set; }
        public byte MinimumAge { get; set; }
        public short WaitingPeriod { get; set; }
        public double YtdMax { get; set; }
        public bool IsAllowMatchWhenZero { get; set; }
        public int? ClientDeductionId { get; set; }
        public double Amount2 { get; set; }
        public double Maximum2 { get; set; }
        public DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsMaxAcrossMatch { get; set; }
        public bool IsPartTimeEmployees { get; set; }
        public bool IsFullTimeEmployees { get; set; }
        public bool IsSalaryEmployees { get; set; }
        public bool IsHourlyEmployees { get; set; }
        public bool? IsUseEnrollmentDates { get; set; }
        public double MinimumContribution { get; set; }
        public byte MinimumContributionTypeId { get; set; }
        public double Minimum { get; set; }
        public byte MinimumTypeId { get; set; }
        public bool IsUseCheckDate { get; set; }
        public decimal Amount3 { get; set; }
        public decimal Maximum3 { get; set; }
    }
}
