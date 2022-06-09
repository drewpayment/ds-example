using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Payroll;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Entity representation of a single plan selection for an employee. Maps to the [dbp.[bpEmployeeOpenEnrollmentSelection] table.
    /// </summary>
    public class EmployeeOpenEnrollmentSelection : Entity<EmployeeOpenEnrollmentSelection>, IHasModifiedOptionalData
    {
        public virtual int               EmployeeOpenEnrollmentSelectionId { get; set; }
        public virtual int               EmployeeOpenEnrollmentId          { get; set; }
        public virtual int?              PlanCategoryId                    { get; set; }
        public virtual int?              CoverageTypeId                    { get; set; }
        public virtual int?              PlanId                            { get; set; }
        public virtual int?              PlanOptionId                      { get; set; }
        public virtual int?              BenefitAmountId                   { get; set; }
        public virtual decimal?          AlternateBenefitAmountValue       { get; set; }
        public virtual bool              WaivedCoverage                    { get; set; }
        public virtual int?              ModifiedBy                        { get; set; }
        public virtual DateTime?         Modified                          { get; set; }
        public virtual decimal?          Cost                              { get; set; }  
        public virtual PayFrequencyType? CostPayFrequency                  { get; set; }
        public virtual int?              PlanWaiveReasonId                 { get; set; }
        public virtual string            PlanWaiveExplanation              { get; set; }

        public virtual PlanWaiveReason        PlanWaiveReason        { get; set; }
        public virtual EmployeeOpenEnrollment EmployeeOpenEnrollment { get; set; }
        public virtual PlanCategory           PlanCategory           { get; set; }
        public virtual CoverageType           CoverageType           { get; set; }
        public virtual Plan                   Plan                   { get; set; }
        public virtual PlanOption             PlanOption             { get; set; }
        public virtual BenefitAmount          BenefitAmount          { get; set; }
        public virtual PayFrequency           CostPayFrequencyInfo   { get; set; }
        public virtual User.User              ModifiedByUser         { get; set; }
        
        public virtual ICollection<EmployeeSelectionDependent> CoveredDependents { get; set; }


        #region Filters

        /// <summary>
        /// Predicate definition used to limit based on a specific employee open enrollment selection.
        /// </summary>
        /// <param name="employeeOpenEnrollmentSelectionId">The employe open enrollment selelction id.</param>
        /// <returns>Predicate.</returns>
        public static Expression<Func<EmployeeOpenEnrollmentSelection, bool>> IsEmployeeOpenEnrollmentSelection(
            int employeeOpenEnrollmentSelectionId)
        {
            return x => x.EmployeeOpenEnrollmentSelectionId == employeeOpenEnrollmentSelectionId;
        }

        /// <summary>
        /// Predicate definition used to limit based on a specific employee open enrollment.
        /// </summary>
        /// <param name="employeeId">The employee id.</param>
        /// <param name="employeeOpenEnrollmentSelectionId">The employe open enrollment selelction id.</param>
        /// <returns>Predicate.</returns>
        public static Expression<Func<EmployeeOpenEnrollmentSelection, bool>> IsEmployeeOpenEnrollmentSelection(
            int employeeId, int employeeOpenEnrollmentSelectionId)
        {
            return x => x.EmployeeOpenEnrollment.EmployeeId == employeeId &&
                        x.EmployeeOpenEnrollmentSelectionId == employeeOpenEnrollmentSelectionId;
        }

        #endregion
    }
}