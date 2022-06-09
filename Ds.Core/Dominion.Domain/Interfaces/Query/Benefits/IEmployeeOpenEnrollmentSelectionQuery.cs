using System;
using System.Text;
using System.Threading.Tasks;

using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    public interface IEmployeeOpenEnrollmentSelectionQuery : IQuery<EmployeeOpenEnrollmentSelection, IEmployeeOpenEnrollmentSelectionQuery>
    {
        /// <summary>
        /// Filters by a single open enrollment.
        /// </summary>
        /// <param name="openEnrollmentId">ID of open enrollment to filter by.</param>
        /// <returns></returns>
        IEmployeeOpenEnrollmentSelectionQuery ByOpenEnrollment(int openEnrollmentId);

        /// <summary>
        /// Filter by a single employee's open enrollment selections.
        /// </summary>
        /// <param name="employeeOpenEnrollmentId">Id of the employee open enrollment.</param>
        /// <returns></returns>
        IEmployeeOpenEnrollmentSelectionQuery ByEmployeeOpenEnrollment(int employeeOpenEnrollmentId);

        /// <summary>
        /// Filter by selections across mutliple enrollments for a single employee.
        /// </summary>
        /// <param name="employeeId">ID of employee.</param>
        /// <returns></returns>
        IEmployeeOpenEnrollmentSelectionQuery ByEmployeeId(int employeeId);

        /// <summary>
        /// Filter selections across multiple enrollments for a single client.
        /// </summary>
        /// <param name="clientId">ID of client.</param>
        /// <returns></returns>
        IEmployeeOpenEnrollmentSelectionQuery ByClientId(int clientId);

        /// <summary>
        /// Filters by selections in which the employee's pay frequency has changed since the selection was made. 
        /// Looks at the employee's current pay frequency vs the selection's cost pay frequency.  Excludes all where 
        /// the cost pay frequency is null.
        /// </summary>
        /// <returns></returns>
        IEmployeeOpenEnrollmentSelectionQuery ByEmployeeHasChangedPayFrequencySinceSelection();

        /// <summary>
        /// Filters selections by the specified waiver status.
        /// </summary>
        /// <param name="isWaived">If true, will only include waived selections. If false, will only include non-waived selections.</param>
        /// <returns></returns>
        IEmployeeOpenEnrollmentSelectionQuery ByWaiverStatus(bool isWaived);

        /// <summary>
        /// Filters selections by those that have an associated plan option selection made.
        /// </summary>
        /// <param name="hasPlanOptionSelection">If true (default), will only return selections that have an associated plan selection.</param>
        /// <returns></returns>
        IEmployeeOpenEnrollmentSelectionQuery ByHasPlanSelection(bool hasPlanOptionSelection = true);

        /// <summary>
        /// Filters by a specific selection record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEmployeeOpenEnrollmentSelectionQuery ByEmployeeSelectionId(int id);
        IEmployeeOpenEnrollmentSelectionQuery ByPlanId(int id);
    }
}
