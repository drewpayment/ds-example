using System;
using System.Collections.Generic;
using Dominion.Benefits.Dto.Enrollment;
using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    /// <summary>
    /// Query <see cref="OpenEnrollment"/> data.
    /// </summary>
    public interface IOpenEnrollmentQuery : IQuery<OpenEnrollment, IOpenEnrollmentQuery>
    {
        /// <summary>
        /// Filters data to a single open enrollment.
        /// </summary>
        /// <param name="openEnrollmentId">ID of open enrollment to filter by.</param>
        /// <returns></returns>
        IOpenEnrollmentQuery ByOpenEnrollmentId(int openEnrollmentId);

        /// <summary>
        /// Filters to enrollment data for a single client.
        /// </summary>
        /// <param name="clientId">ID of client to filter enrollment info for.</param>
        /// <returns></returns>
        IOpenEnrollmentQuery ByClientId(int clientId);

        IOpenEnrollmentQuery ByLifeEventReasonType(LifeEventReasonType lifeEventReasonType);

        IOpenEnrollmentQuery ByEventDate(DateTime lifeeventDateEventReasonType);

        /// <summary>
        /// A date will be passed in and return any open enrollment that is valid for the date. 
        /// This can be 0.
        /// </summary>
        /// <param name="dateToCheck"></param>
        /// <returns> Any open enrollment that is valid for the date.</returns>
        IOpenEnrollmentQuery ByEnrollmentsOpenOnDate(DateTime dateToCheck);

        /// <summary>
        /// Returns the enrollments (both regular and life-events) available to the specified employee.
        /// </summary>
        /// <param name="employeeId">ID of employee to get open enrollments for.</param>
        /// <returns></returns>
        IOpenEnrollmentQuery ByEnrollmentsOpenForEmployee(int employeeId);

        /// <summary>
        /// Filters by regular <see cref="OpenEnrollmentType.OpenEnrollment"/>(s) that are not associated with a 
        /// particular employee (i.e. <see cref="OpenEnrollment.LifeEventEmployeeId"/> is null).
        /// </summary>
        /// <returns></returns>
        IOpenEnrollmentQuery ByRegularEnrollments();

        /// <summary>
        /// Filters by a specific type of enrollment (i.e. Open-Enrollment or Life-Event).
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IOpenEnrollmentQuery ByEnrollmentType(OpenEnrollmentType type);

        /// <summary>
        /// Filters by enrollments with the specified approval status. Default is approved = true.
        /// </summary>
        /// <param name="isApproved">Indication if the resulting enrollments should be approved or not. 
        /// Default is approved (true).</param>
        /// <returns></returns>
        IOpenEnrollmentQuery ByIsApproved(bool isApproved = true);

        /// <summary>
        /// Filters by enrollments and life events that have not been declined by the admin.
        /// </summary>
        /// <returns></returns>
        IOpenEnrollmentQuery ByNotDeclined();

        /// <summary>
        /// Filters by enrollments that have ended by the specified date.
        /// </summary>
        /// <param name="endedBy">Date the enrollment period should have ended by. If null (default), 
        /// <see cref="DateTime.Today"/> will be used.</param>
        /// <returns></returns>
        IOpenEnrollmentQuery ByHasEndedBy(DateTime? endedBy = null);

        /// <summary>
        /// Filters by enrollments ending between the specfied inclusive
        /// <see cref="from"/> and <see cref="to"/> dates.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        IOpenEnrollmentQuery ByHasEndedWithinRange(DateTime from, DateTime to);

        /// <summary>
        /// Filters by enrollments that have not been integrated (per deduction integration date and user).
        /// </summary>
        /// <param name="wasIntegrated">If true, will return only enrollments that have been integrated. 
        /// If false, will return only enrollments that have NOT been integrated.</param>
        /// <returns></returns>
        IOpenEnrollmentQuery ByHasBeenIntegratedWithPayroll(bool wasIntegrated = true);

        /// <summary>
        /// Filters by only enrollments with at least one signed employee enrollment with a plan selection requiring integration.
        /// </summary>
        /// <returns></returns>
        IOpenEnrollmentQuery ByHasSignedEmployeeEnrollmentWithSelectionsRequiringIntegration();

        /// <summary>
        /// Filters by enrollments requiring payroll integration that must be effective as of the specified date. 
        /// Enrollments with <see cref="OpenEnrollment.IsPayrollIntegrationWaived"/> = true will always be excluded.
        /// </summary>
        /// <param name="effectiveAsOf">Date the payroll integration must have taken place by. If null, <see cref="DateTime.Now"/> will be used.</param>
        /// <returns></returns>
        IOpenEnrollmentQuery ByIntegrationEffectiveAsOf(DateTime? effectiveAsOf = null);

        /// <summary>
        /// Filters enrollments by those that have at least one plan that is active (per start/end dates) 
        /// within the provided date range.
        /// </summary>
        /// <param name="from">Start of date range a plan must be active within. If null, 
        /// any plans starting before the <see cref="to"/> date will be considered 'active'.</param>
        /// <param name="to">End of date range a plan must be active within. If null, 
        /// any plans ending after the <see cref="from"/> date will be considered 'active'.</param>
        /// <returns></returns>
        IOpenEnrollmentQuery ByHasActivePlansWithinDateRange(DateTime? from = null, DateTime? to = null);

        /// <summary>
        /// Filters life events by approval status types.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        IOpenEnrollmentQuery ByApprovalStatusType(IEnumerable<LifeEventApprovalType> types);
        IOpenEnrollmentQuery ByHasOpenEnrollmentPlan(int planId);

        IOpenEnrollmentQuery ByPlanProvider(int providerId);
        IOpenEnrollmentQuery ByHasActivePlansStartWithinDateRange(DateTime? @from, DateTime? to);

        IOpenEnrollmentQuery ByHasStartWithinRange(DateTime? @from, DateTime? to);
        IOpenEnrollmentQuery ByPlanId(int planId);
    }
}
