using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    public interface IEmployeeOpenEnrollmentQuery : IQuery<EmployeeOpenEnrollment, IEmployeeOpenEnrollmentQuery>
    {
        /// <summary>
        /// Filters by employee enrollment info for a single open enrollment.
        /// </summary>
        /// <param name="openEnrollmentId">Open enrollment to query by.</param>
        /// <returns></returns>
        IEmployeeOpenEnrollmentQuery ByOpenEnrollmentId(int openEnrollmentId);

        /// <summary>
        /// Filters by employee enrollment info for one or more open enrollments.
        /// </summary>
        /// <param name="enrollmentIds">Open enrollment(s) to query by.</param>
        /// <returns></returns>
        IEmployeeOpenEnrollmentQuery ByOpenEnrollments(IEnumerable<int> enrollmentIds);

        /// <summary>
        /// Filters by employee enrollment info for a single employee.
        /// </summary>
        /// <param name="employeeId">Employee to get enrollment info for.</param>
        /// <returns></returns>
        IEmployeeOpenEnrollmentQuery ByEmployeeId(int employeeId);

        /// <summary>
        /// Filters by employee enrollment info for one or more employees.
        /// </summary>
        /// <param name="employeeIds">Employee(s) to get enrollment info for.</param>
        /// <returns></returns>
        IEmployeeOpenEnrollmentQuery ByEmployeeIds(IEnumerable<int> employeeIds);

        /// <summary>
        /// Filters by employee enrollment info for a single client.
        /// </summary>
        /// <param name="clientId">Client to filter enrollment info by.</param>
        /// <returns></returns>
        IEmployeeOpenEnrollmentQuery ByClientId(int clientId);

        /// <summary>
        /// Filters by enrollments that have been signed by the employee.
        /// </summary>
        /// <param name="isSigned">If true (default), will only include enrollment information that has been
        /// signed by the employee.</param>
        /// <returns></returns>
        IEmployeeOpenEnrollmentQuery ByIsSigned(bool isSigned = true);

        /// <summary>
        /// Filters by employee enrollment data for enrollments that are no longer open.
        /// </summary>
        /// <param name="closedAsOf">Date the enrollment must be closed by in order for the employee enrollment data to 
        /// be included. If null, <see cref="DateTime.Today"/> will be used.</param>
        /// <returns></returns>
        IEmployeeOpenEnrollmentQuery ByEnrollmentIsClosed(DateTime? closedAsOf = null);

        IEmployeeOpenEnrollmentQuery ByEmployeeOpenEnrollmentId(int employeeOpenEnrollmentId);
    }
}
