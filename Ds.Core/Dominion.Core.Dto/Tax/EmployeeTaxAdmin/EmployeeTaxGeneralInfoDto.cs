using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Tax.EmployeeTaxAdmin
{
    public class EmployeeTaxGeneralInfoDto
    {
        public int EmployeePayId { get; set; }
        public int? SutaClientTaxId { get; set; }
        public string PsdCode { get; set; }
        public int? WotcReasonId { get; set; }
        public bool DeferEmployeeSocSecTax { get; set; }
        public bool Is1099Exempt { get; set; }
        public bool IsFicaExempt { get; set; }
        public bool IsFutaExempt { get; set; }
        public bool IsSutaExempt { get; set; }
        public bool IsSocSecExempt { get; set; }
        public bool IsStateTaxExempt { get; set; }
        public bool IsIncomeTaxExempt { get; set; }
        public bool IsHireActQualified { get; set; }
        public DateTime HireActStartDate { get; set; }
        public bool HasYtdTaxOrWages { get; set; }
        public bool ClientHasReimbursableEarning { get; set; }
        public bool AllowIncomeWageExemptOption { get; set; }
    }
}
