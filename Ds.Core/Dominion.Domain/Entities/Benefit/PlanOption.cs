using System;
using System.Collections.Generic;

using Dominion.Benefits.Dto.Plans;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Utility.MathExt;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Entity representation of a dependent/cost option of a benefit plan. Maps to [dbo].[bpPlanOption].
    /// </summary>
    public class PlanOption : Entity<PlanOption>, IHasModifiedData
    {
        public virtual int                               PlanOptionId                                { get; set; }
        public virtual int?                              PlanId                                      { get; set; }
        public virtual int?                              ClientId                                    { get; set; }
        public virtual string                            Name                                        { get; set; }
        public virtual string                            Copay                                       { get; set; }
        public virtual decimal?                          CostPerMonth                                { get; set; }
        public virtual string                            Deductible                                  { get; set; }
        public virtual string                            OutOfPocketMax                              { get; set; }
        public virtual DateTime                          Modified                                    { get; set; }
        public virtual int                               ModifiedBy                                  { get; set; }
        public virtual DependentCoverageOptionType       DependentCoverageOption                     { get; set; }
        public virtual PayFrequencyType?                 CostPayFrequencyType                        { get; set; }
        public virtual CostType?                         CostType                                    { get; set; }
        public virtual RoundingRule?                     CostPerPayRoundingRule                      { get; set; }
        public virtual decimal?                          CostBenefitAmountMultiplier                 { get; set; }
        public virtual decimal?                          MinBenefitAmount                            { get; set; }
        public virtual decimal?                          MaxBenefitAmount                            { get; set; }
        public virtual decimal?                          IncrementAmount                             { get; set; }
        public virtual CostFactorSource?                 CostFactorSource                            { get; set; }
        public virtual decimal?                          EoiMaximum                                  { get; set; }
        public virtual BenefitAmountSelectionType?       BenefitAmountSelectionType                  { get; set; }
        public virtual PayFrequencyType?                 BenefitPayoutFrequencyId                    { get; set; }
        public virtual RoundingRule?                     BenefitAmountRoundingRule                   { get; set; }
        public virtual decimal?                          RoundBenefitToAmount                        { get; set; }
        public virtual decimal?                          MaxPercentOfSalary                          { get; set; }
        public virtual int?                              ParentSelectionPlanOptionId                 { get; set; }
        public virtual decimal?                          MaxPercentOfParentSelection                 { get; set; }
        public virtual int?                              EmployeeDeductionMaxTypeId                  { get; set; }

        public virtual Plan                             Plan                           { get; set; }
        public virtual PlanOptionPlan                   PlanOptionPlan                 { get; set; }
        public virtual DependentCoverageOptionTypeInfo  DependentCoverageOptionInfo    { get; set; }
        public virtual PayFrequency                     CostPayFrequency               { get; set; }
        public virtual CostTypeInfo                     CostTypeInfo                   { get; set; }
        public virtual RoundingRuleTypeInfo             CostPerPayRoundingRuleInfo     { get; set; } 
        public virtual BenefitAmountSelectionTypeInfo   BenefitAmountSelectionTypeInfo { get; set; }
        public virtual RoundingRuleTypeInfo             BenefitAmountRoundingRuleInfo  { get; set; }
        public virtual PayFrequency                     BenefitPayoutFrequency         { get; set; }
        public virtual PlanOption                       ParentSelectionPlanOption      { get; set; }
        
        public virtual ICollection<PlanOptionCost>                    Costs                     { get; set; }
        public virtual ICollection<PlanOptionCostFactorConfiguration> CostFactorSettings        { get; set; }
        public virtual ICollection<CostFactorOption>                  CustomCostFactorOptions   { get; set; }
        public virtual ICollection<EmployeeOpenEnrollmentSelection>   EmployeeSelections        { get; set; }
        public virtual ICollection<PlanTypePlanOption>                PlanTypePlanOptions       { get; set; }
        public virtual ICollection<BenefitAmount>                     BenefitAmounts            { get; set; }
        public virtual ICollection<PlanOption>                        DependentChildPlanOptions { get; set; }

        public void SetValues(PlanOption item)
        {
            this.PlanOptionId = item.PlanOptionId;
            this.PlanId = item.PlanId;
            this.ClientId = item.ClientId;
            this.Name = item.Name;
            this.Copay = item.Copay;
            this.CostPerMonth = item.CostPerMonth;
            this.Deductible = item.Deductible;
            this.OutOfPocketMax = item.OutOfPocketMax;
            this.Modified = item.Modified;
            this.ModifiedBy = item.ModifiedBy;
            this.DependentCoverageOption = item.DependentCoverageOption;
            this.CostPayFrequencyType = item.CostPayFrequencyType;
            this.CostType = item.CostType;
            this.CostPerPayRoundingRule = item.CostPerPayRoundingRule;
            this.CostBenefitAmountMultiplier = item.CostBenefitAmountMultiplier;
            this.MinBenefitAmount = item.MinBenefitAmount;
            this.MaxBenefitAmount = item.MaxBenefitAmount;
            this.IncrementAmount = item.IncrementAmount;
            this.CostFactorSource = item.CostFactorSource;
            this.EoiMaximum = item.EoiMaximum;
            this.BenefitAmountSelectionType = item.BenefitAmountSelectionType;
            this.BenefitPayoutFrequencyId = item.BenefitPayoutFrequencyId;
            this.BenefitAmountRoundingRule = item.BenefitAmountRoundingRule;
            this.RoundBenefitToAmount = item.RoundBenefitToAmount;
            this.MaxPercentOfSalary = item.MaxPercentOfSalary;
            this.ParentSelectionPlanOptionId = item.ParentSelectionPlanOptionId;
            this.MaxPercentOfParentSelection = item.MaxPercentOfParentSelection;
        }
    }
}