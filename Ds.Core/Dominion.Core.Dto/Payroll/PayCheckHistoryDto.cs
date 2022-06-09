using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Misc;
using Dominion.Core.Dto.EEOC;
using Dominion.Core.Dto.Employee;

namespace Dominion.Core.Dto.Payroll
{
    public class PaycheckHistoryDto
    {
        public int GenPaycheckHistoryId { get; set; }
        public int GenPayrollHistoryId { get; set; }
        public int EmployeeId { get; set; }
        public string SubCheck { get; set; }
        public DateTime? PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public decimal CheckAmount { get; set; }
        public int? CheckNumber { get; set; }
        public decimal GrossPay { get; set; }
        public decimal PartialGrossPay { get; set; }
        public decimal Tips { get; set; }
        public decimal PartialTips { get; set; }
        public decimal NetPay { get; set; }
        public bool IsAdjustment { get; set; }
        public bool IsFicaExempt { get; set; }
        public bool IsFutaExempt { get; set; }
        public bool IsSutaExempt { get; set; }
        public bool IsIncomeTaxExempt { get; set; }
        public bool Is1099Exempt { get; set; }
        public decimal SocSecWages { get; set; }
        public decimal MedicareWages { get; set; }
        public decimal FutaWages { get; set; }
        public decimal MedicareTax { get; set; }
        public decimal EmployerMedicareTax { get; set; }
        public decimal SocSecTax { get; set; }
        public decimal EmployerSocSecTax { get; set; }
        public decimal EmployerFutaTax { get; set; }
        public decimal ExemptWages { get; set; }
        public decimal FlexDeductions { get; set; }
        public decimal TotalTax { get; set; }
        public double StraightHours { get; set; }
        public decimal StraightPay { get; set; }
        public double PremiumHours { get; set; }
        public decimal PremiumPay { get; set; }
        public bool IsLt3Psp { get; set; }
        public bool IsSt3Psp { get; set; }
        public decimal TipCredits { get; set; }
        public decimal HireActWages { get; set; }
        public decimal HireActCredit { get; set; }
        public int ClientId { get; set; }
        public DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }
        public decimal? CustomGrossPay { get; set; }
        public DateTime? PayrollCheckDate { get; set; }
        public int? PayrollId { get; set; }
        public bool IsStateTaxExempt { get; set; }
        public bool Void { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Amount { get; set; }

        //REVERSE NAVIGATION
        public IEnumerable<PaycheckPayDataHistoryDto> PaycheckPayDataHistory { get; set; } // many-to-one;
        //public ICollection<EEO1ExportDto.PaycheckEarningHistoryDto> EarningHistory { get; set; }
        //public EEO1ExportDto.EmployeeDto Employee { get; set; }

        //FOREIGN KEYS
        public PayrollHistoryDto PayrollHistory { get; set; }
        public ClientDto Client { get; set; }
        public PayrollDto Payroll { get; set; }
        public EmployeeBasicDto Employee { get; set; }
    }

    public class PayCheckHistoryPairingDto
    {
        public int Year { get; set; }
        public IEnumerable<PaycheckHistoryDto> PaycheckHistory { get; set; }
    }

    public class PaycheckHistorySaveVoidChecksDto : IEquatable<PaycheckHistorySaveVoidChecksDto>
    {
        public int GenPaycheckHistoryId { get; set; }
        public int EmployeeId { get; set; }

        bool IEquatable<PaycheckHistorySaveVoidChecksDto>.Equals(PaycheckHistorySaveVoidChecksDto other)
        {
            return this.GenPaycheckHistoryId == other.GenPaycheckHistoryId && this.EmployeeId == other.EmployeeId;
        }
    }
}
