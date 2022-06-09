using System;
using Dominion.Core.Dto.Tax;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Tax.Legacy
{
    /// <summary>
    /// Date based federal tax configuration settings.
    /// </summary>
    public partial class LegacyFederalTaxProfile : Entity<LegacyFederalTaxProfile>, ILegacyTaxIdAndType
    {
        public virtual int      FederalTaxId                   { get; set; } 
        public virtual DateTime EffectiveDate                  { get; set; } 
        public virtual double   AnnualExemptionAmount          { get; set; } 
        public virtual double   SocialSecurityLimit            { get; set; } 
        public virtual double   MedicareLimit                  { get; set; } 
        public virtual double   FederalUnemployment            { get; set; } 
        public virtual double   CompanySocialSecurityRate      { get; set; } 
        public virtual double   CompanyMedicareRate            { get; set; } 
        public virtual double   EmployeeSocialSecurityRate     { get; set; } 
        public virtual double   EmployeeMedicareRate           { get; set; } 
        public virtual double   CompanyFederalUnemploymentRate { get; set; } 
        public virtual double   MinimumWage                    { get; set; } 
        public virtual double?  TipCredit                      { get; set; } 
        public virtual double   Percent                        { get; set; } 
        public virtual DateTime LastModifiedDate               { get; set; } 
        public virtual string   LastModifiedByDescription      { get; set; } 
        public virtual double   MedicareThresholdAmount        { get; set; } 
        public virtual double   EmployeeAdditionalMedicareRate { get; set; } 
        public virtual decimal  StandardDeductionAmountSingle  { get; set; }
        public virtual decimal  StandardDeductionAmountMarried { get; set; }

        // ILegacyTaxIdAndType Mappings
        public virtual int TaxId => FederalTaxId;
        public virtual LegacyTaxType? LegacyTaxType => Dominion.Core.Dto.Tax.LegacyTaxType.FederalWitholding;
    }
}
