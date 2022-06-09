using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Dominion.Benefits.Dto.Plans;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Benefit plan entity. Maps to the [dbo].[bpPlan] table.
    /// </summary>
    public class Plan : Entity<Plan>, IHasModifiedData
    {
        public virtual int                 PlanId                     { get; set; }
        public virtual int                 ClientId                   { get; set; }
        public virtual int                 PlanCategoryId             { get; set; }
        public virtual int?                PlanProviderId             { get; set; }
        public virtual PlanEligibilityType EligibilityType            { get; set; }
        public virtual string              PlanName                   { get; set; }
        public virtual string              PlanDescription            { get; set; }
        public virtual string              PolicyNumber               { get; set; }
        public virtual DateTime?           StartDate                  { get; set; }
        public virtual DateTime?           EndDate                    { get; set; }
        public virtual DateTime            Modified                   { get; set; }
        public virtual int                 ModifiedBy                 { get; set; }
        public virtual bool                IsActive                   { get; set; }
        public virtual bool                IsPcpRequired              { get; set; }
        public virtual int?                ClientDeductionId          { get; set; }
        public virtual int                 PlanTypeId                 { get; set; }
        public virtual bool                IsPayrollIntegrationWaived { get; set; }
        public virtual byte?               DependentAgeLimit          { get; set; }
        public virtual byte?               StudentDependentAgeLimit   { get; set; }
        public virtual byte?               DisabledDependentAgeLimit  { get; set; }
        public virtual AgeDetermination?   AgeDeterminationTypeId     { get; set; }
        public virtual DateTime?           CustomAgeDeterminationDate { get; set; }
        public virtual bool                IsEoiRequired              { get; set; }
        public virtual int?                EoiResourceId              { get; set; }
        public virtual bool                IsEmployerPaid             { get; set; }

        //FOREIGN KEYS
        public virtual PlanType                PlanType             { get; set; }
        public virtual PlanProvider            PlanProvider         { get; set; }
        public virtual PlanCategory            PlanCategory         { get; set; }
        public virtual ClientDeduction         ClientDeduction      { get; set; }
        public virtual AgeDeterminationType    AgeDeterminationType { get; set; }
        public virtual PlanEligibilityTypeInfo EligibilityTypeInfo  { get; set; }

        public virtual ICollection<PlanOptionPlan>                  PlanOptionPlans            { get; set; }
        public virtual ICollection<PlanOption>                      PlanOptions                { get; set; }
        public virtual ICollection<BenefitResource>                 PlanResources              { get; set; }
        public virtual ICollection<OpenEnrollmentPlan>              OpenEnrollments            { get; set; }
        public virtual ICollection<EmployeeOpenEnrollmentSelection> EmployeeSelections         { get; set; }
        public virtual ICollection<PlanEmployeeAvailability>        PlanEmployeeAvailabilities { get; set; }
        public virtual ICollection<PlanPackage>                     PlanPackages               { get; set; }

        public void SetValues(Plan item)
        {
            this.PlanId = item.PlanId;
            this.ClientId = item.ClientId;
            this.PlanCategoryId = item.PlanCategoryId;
            this.PlanProviderId = item.PlanProviderId;
            this.EligibilityType = item.EligibilityType;
            this.PlanName = item.PlanName;
            this.PlanDescription = item.PlanDescription;
            this.PolicyNumber = item.PolicyNumber;
            this.StartDate = item.StartDate;
            this.EndDate = item.EndDate;
            this.Modified = item.Modified;
            this.ModifiedBy = item.ModifiedBy;
            this.IsActive = item.IsActive;
            this.IsPcpRequired = item.IsPcpRequired;
            this.ClientDeductionId = item.ClientDeductionId;
            this.PlanTypeId = item.PlanTypeId;
            this.IsPayrollIntegrationWaived = item.IsPayrollIntegrationWaived;
            this.DependentAgeLimit = item.DependentAgeLimit;
            this.StudentDependentAgeLimit = item.StudentDependentAgeLimit;
            this.DisabledDependentAgeLimit = item.DisabledDependentAgeLimit;
            this.AgeDeterminationTypeId = item.AgeDeterminationTypeId;
            this.CustomAgeDeterminationDate = item.CustomAgeDeterminationDate;
            this.IsEoiRequired = item.IsEoiRequired;
            this.EoiResourceId = item.EoiResourceId;
        }
    }
}