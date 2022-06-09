using System;
using System.Linq.Expressions;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Entity representation of an employee dependent's plan selection. Maps to [dbo].[bpEmployeeSelectionDependent] table.
    /// </summary>
    public class EmployeeSelectionDependent : Entity<EmployeeSelectionDependent>, IHasModifiedData
    {
        public virtual int EmployeeSelectionDependentId { get; set; }
        public virtual int EmployeeOpenEnrollmentSelectionId { get; set; }
        public virtual int EmployeeDependentId { get; set; }
        public virtual DateTime DateAdded { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual DateTime Modified { get; set; }

        public virtual EmployeeDependent EmployeeDependent { get; set; }
        public virtual EmployeeOpenEnrollmentSelection EmployeeSelection { get; set; }

        #region Filters

        /// <summary>
        /// This will retrieve all employee selections dependents that belong to the given employee for a plan selection.
        /// </summary>
        /// <param name="employeeOpenEnrollmentSelectionId"></param>
        /// <returns> A list of all active Plans currently in a suppplied client.</returns>
        public static Expression<Func<EmployeeSelectionDependent, bool>> EmployeeDependentsForPlanOptionSelection(
            int employeeOpenEnrollmentSelectionId)
        {
            return x => (x.EmployeeOpenEnrollmentSelectionId == employeeOpenEnrollmentSelectionId);
        }

        /// <summary>
        /// This will retrieve all employee selections dependents by Id.
        /// </summary>
        /// <param name="employeeEmployeeSelectionDependentId"></param>
        /// <returns> A list of all active Plans currently in a suppplied client.</returns>
        public static Expression<Func<EmployeeSelectionDependent, bool>> EmployeeDependentSelectionById(
            int employeeEmployeeSelectionDependentId)
        {
            return x => (x.EmployeeSelectionDependentId == employeeEmployeeSelectionDependentId);
        }

        /// <summary>
        /// This will retrieve all employee selections dependents by employee open enrollment selection Id.
        /// </summary>
        /// <param name="employeeEmployeeSelectionDependentId"></param>
        /// <returns> A list of all active Plans currently in a suppplied client.</returns>
        public static Expression<Func<EmployeeSelectionDependent, bool>> ByEmployeeOpenEnrollmentSelectionId(
            int employeeEmployeeSelectionDependentId)
        {
            return x => (x.EmployeeOpenEnrollmentSelectionId == employeeEmployeeSelectionDependentId);
        }


        public static Expression<Func<EmployeeSelectionDependent, bool>> ForEmployee(
            int employeeOpenEnrollmentSelectionId, int employeeId)
        {
            return x => x.EmployeeOpenEnrollmentSelectionId == employeeOpenEnrollmentSelectionId &&
                        x.EmployeeDependent.EmployeeId == employeeId;
        }

        public static Expression<Func<EmployeeSelectionDependent, bool>> IsEmployeeSelectionDependent(int employeeId, 
            int employeeSelectionDependentId)
        {
            return x => x.EmployeeSelectionDependentId == employeeSelectionDependentId &&
                        x.EmployeeDependent.EmployeeId == employeeId;
        }

        #endregion
    }
}