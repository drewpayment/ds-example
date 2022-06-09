using System;
using System.Collections.Generic;

using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Tax;
using Dominion.Pay.Dto.Deductions;

namespace Dominion.Domain.Entities.Payroll
{
    using Dominion.Domain.Entities.Employee;

    public class ClientDeduction : Entity<ClientDeduction>
    {
        public virtual int                          ClientDeductionId         { get; set; } //pk
        public virtual int                          ClientId                  { get; set; } //fk
        public virtual string                       Code                      { get; set; } 
        public virtual string                       Description               { get; set; } 
        public virtual bool                         IsDoMultipleChecks        { get; set; } 
        public virtual bool?                        IsDoRegularChecks         { get; set; } //highfix: jay: this says null for the main table but not null for the change history. This will have to be changed in the database and in code.
        public virtual int                          StubOptions               { get; set; } 
        public virtual byte                         TaxOption_Legacy          { get; set; } 
        public virtual bool                         IsMemoDeduction           { get; set; } 
        public virtual int?                         DeferredCompId            { get; set; } //fk (null)
        public virtual byte                         Delinquency               { get; set; } 
        public virtual bool                         IsCheckW2                 { get; set; } 
        public virtual bool                         IsNonQualified            { get; set; } 
        public virtual bool                         IsDependantCare           { get; set; } 
        public virtual byte                         SpecialOption             { get; set; } 
        public virtual int?                         ClientVendorId            { get; set; } 
        public virtual bool                         IsEarningsOnly            { get; set; } 
        public virtual int?                         ClientEarningId           { get; set; } //fk
        public virtual byte                         SequenceNum               { get; set; } 
        public virtual DateTime                     Modified                  { get; set; } 
        public virtual string                       ModifiedBy                { get; set; } 
        public virtual ClientDeductionCategoryType? ClientDeductionCategoryId { get; set; } 
        public virtual bool                         IsPaidBenefitsOnStub      { get; set; } 
        public virtual bool?                        IsTotVendOnManCheck       { get; set; } 
        public virtual bool?                        IsAllowDirectDeposit      { get; set; } 
        public virtual string                       W2Description             { get; set; } 
        public virtual bool                         IsActive                  { get; set; } 
        public virtual bool                         IsMedical                 { get; set; } 
        public virtual double                       MaxPercent                { get; set; } 
        public virtual int?                         MaxType                   { get; set; } 
        public virtual bool                         IsAllowPercentOfNet       { get; set; } 
        public virtual bool                         IsCompanySponsoredHealth  { get; set; } 
        public virtual decimal?                     DeductionMinimum          { get; set; }
        public virtual decimal?                     DeductionMaximum          { get; set; }
        public virtual bool                         IsStopDeferralAtWageLimit { get; set; }

        public virtual int                        Frequency                 { get; set; } //fk 
        public virtual DeductionFrequencyType     DeductionFrequencyType    { get; set; }
        public virtual int?                       TaxOptionId               { get; set; } //fk

        // FOREIGN KEYS
        public virtual Client Client { get; set; }
        public virtual TaxOption TaxOption { get; set; }
        public virtual ICollection<EmployeeDeduction> EmployeeDeductions { get; set; }
        public virtual ClientVendor ClientVendor { get; set; }
        public virtual ClientEarning ClientEarning { get; set; }
        public virtual ICollection<ClientDeductionMappingPlanInfo> ClientDeductionMappingPlan { get; set; }

    }
}