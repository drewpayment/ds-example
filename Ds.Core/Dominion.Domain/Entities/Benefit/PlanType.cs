using System.Collections.Generic;
using Dominion.Benefits.Dto.Plans;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Employee;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Entity for dbo.bpPlanType table.
    /// </summary>
    public partial class PlanType : Entity<PlanType>
    {
        public virtual int      PlanTypeId              { get; set; }
        public virtual int      PlanCategoryId          { get; set; }
        public virtual string   Name                    { get; set; }
        public virtual bool     HasPcpRequiredOption    { get; set; }
        public virtual bool     HasEoiRequiredOption    { get; set; }
        public virtual CostType DefaultCostType         { get; set; }
        public virtual bool     IsBasicPlanSetupAllowed { get; set; }
        public virtual bool     IsArchived              { get; set; }

        public virtual PlanCategory                                PlanCategory                       { get; set; }
        public virtual ICollection<PlanTypePlanOption>             PlanTypePlanOptions                { get; set; }
        public virtual ICollection<Plan>                           Plans                              { get; set; }
        public virtual ICollection<CoverageType>                   CoverageTypes                      { get; set; } 
        public virtual ICollection<BenefitAmountSelectionTypeInfo> BenefitAmountSelectionTypesOffered { get ;set; }
        public virtual ICollection<EmployeeDeductionMaxTypeEntity> EmployeeDeductionMaxTypeEntities { get ;set; }
    }
}