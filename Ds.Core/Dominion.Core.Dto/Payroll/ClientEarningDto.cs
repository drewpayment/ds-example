using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Labor;

namespace Dominion.Core.Dto.Payroll
{
    public partial class ClientEarningDto
    {
        public int ClientEarningId { get; set; }
        public int ClientId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public double Percent { get; set; }
        public bool IsShowOnStub { get; set; }
        public bool IsShowYtdHours { get; set; }
        public bool IsShowYtdDollars { get; set; }
        public byte CalcShiftPremium { get; set; }
        public bool IsTips { get; set; }
        public bool IsDefault { get; set; }
        public ClientEarningCategory EarningCategoryId { get; set; }
        public bool IsIncludeInDeductions { get; set; }
        public bool IsEic { get; set; }
        public DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }
        public decimal? AdditionalAmount { get; set; }
        public bool IsIncludeInOvertimeCalcs { get; set; }
        public bool IsActive { get; set; }
        public bool IsBlockFromTimeClock { get; set; }
        public bool IsIncludeInAvgRate { get; set; }
        public bool IsShowOnlyIfCurrent { get; set; }
        public int BlockedSecurityUser { get; set; }
        public bool IsUpMinWage { get; set; }
        public bool IsShowLifetimeHours { get; set; }
        public bool IsExcludeHrsInArpCalc { get; set; }
        public bool IsExcludePayInArpCalc { get; set; }
        public bool IsServiceChargeTips { get; set; }
        public bool IsAcaWorkedHours { get; set; }
        public int? TaxOptionId { get; set; }
        public bool IsReimburseTaxes { get; set; }
        public bool IsBasePay { get; set; }
        public int EmergencyLeave { get; set; }

       //public virtual ICollection<PayrollPayDataDetailDto> PayrollPayDataDetail { get; set; } // many-to-one;

        public virtual ICollection<PayrollControlTotalDto> PayrollControlTotal { get; set; } // many-to-one;

        public ClientEarningCategoryDto ClientEarningCategory { get; set; }

    }
}
