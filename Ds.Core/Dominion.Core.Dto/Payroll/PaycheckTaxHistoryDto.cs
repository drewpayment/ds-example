using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Payroll
{
    public class PaycheckTaxHistoryDto
    {
        public virtual int GenPaycheckTaxHistoryId { get; set; }
        public virtual int GenPaycheckPayDataHistoryId { get; set; }
        public virtual int? ClientTaxId { get; set; }
        public virtual decimal GrossWages { get; set; }
        public virtual decimal TaxableWage { get; set; }
        public virtual string TaxType { get; set; }
        public virtual int MaritalStatusId { get; set; }
        public virtual int Exemptions { get; set; }
        public virtual decimal CalculatedAmount { get; set; }
        public virtual decimal AdditionalAmount { get; set; }
        public virtual decimal TotalAmount { get; set; }
        public virtual int ClientId { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual string ModifiedBy { get; set; }
        public virtual DateTime? PayrollCheckDate { get; set; }
        public virtual int? PayrollId { get; set; }
        public virtual int? PaycheckId { get; set; }
        public virtual decimal EmployerPaidAmount { get; set; }
        public virtual bool IsEmployerPaidTax { get; set; }

        /// <summary>
        /// If <see cref="IsEmployerPaidTax"/>, returns <see cref="EmployerPaidAmount"/>.
        /// Else, returns <see cref="TotalAmount"/>
        /// </summary>
        public decimal TaxAmount => IsEmployerPaidTax ? EmployerPaidAmount : TotalAmount;
    }
}
