using System;
using Dominion.Benefits.Dto.Employee;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Entity representation of the dbo.bpEmployeeBenefitInfo table.
    /// </summary>
    public partial class EmployeeBenefitInfo : Entity<EmployeeBenefitInfo>, IHasModifiedData
    {
        public int               EmployeeId          { get; set; }
        public bool              IsTobaccoUser       { get; set; }
        public bool              IsEligible          { get; set; }
        public DateTime?         EligibilityDate     { get; set; }
        public int?              BenefitPackageId    { get; set; }
        public SalaryMethodType? DefaultSalaryMethod { get; set; }
        public DateTime          Modified            { get; set; }
        public int               ModifiedBy          { get; set; }
        public int? ClientEmploymentClassId { get; set; }

        public virtual Employee.Employee    Employee                { get; set; }
        public virtual BenefitPackage       BenefitPackage          { get; set; }
        public virtual SalaryMethodTypeInfo DefaultSalaryMethodInfo { get; set; }
        public virtual ClientEmploymentClass ClientEmploymentClass  { get; set; }
        public bool             IsRetirementEligible { get; set; }

    }
}
