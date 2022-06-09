using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Payroll;

namespace Dominion.Core.Dto.Client
{
    public class ClientDeductionDto
    {
        public int ClientDeductionId { get; set; } //pk
        public int ClientId { get; set; } //fk
        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsDoMultipleChecks { get; set; }
        public bool? IsDoRegularChecks { get; set; } //highfix: jay: this says null for the main table but not null for the change history. This will have to be changed in the database and in code.
        public int StubOptions { get; set; }
        public byte TaxOption_Legacy { get; set; }
        public bool IsMemoDeduction { get; set; }
        public int? DeferredCompId { get; set; } //fk (null)
        public byte Delinquency { get; set; }
        public bool IsCheckW2 { get; set; }
        public bool IsNonQualified { get; set; }
        public bool IsDependantCare { get; set; }
        public byte SpecialOption { get; set; }
        public int? ClientVendorId { get; set; }
        public bool IsEarningsOnly { get; set; }
        public int? ClientEarningId { get; set; } //fk
        public byte SequenceNum { get; set; }
        public DateTime Modified { get; set; }
        public string ModifiedBy { get; set; }
        public ClientDeductionCategoryType? ClientDeductionCategoryId { get; set; }
        public bool IsPaidBenefitsOnStub { get; set; }
        public bool? IsTotVendOnManCheck { get; set; }
        public bool? IsAllowDirectDeposit { get; set; }
        public string W2Description { get; set; }
        public bool IsActive { get; set; }
        public bool IsMedical { get; set; }
        public double MaxPercent { get; set; }
        public int? MaxType { get; set; }
        public bool IsAllowPercentOfNet { get; set; }
        public bool IsCompanySponsoredHealth { get; set; }
        public decimal? DeductionMinimum { get; set; }
        public decimal? DeductionMaximum { get; set; }
        public bool IsStopDeferralAtWageLimit { get; set; }

        public int Frequency { get; set; } //fk 
        public DeductionFrequencyType DeductionFrequencyType { get; set; }
        public int? TaxOptionId { get; set; } //fk
    }
}
