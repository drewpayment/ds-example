using System;
using System.Collections.Generic;

using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeeBenefitQuery : IQuery<ClockEmployeeBenefit, IEmployeeBenefitQuery>
    {
        /// <summary>
        /// Filters by a particular client.
        /// NOTE: Cluster index exists on ClientID | EmployeeID | EventDate | ClientEarningID | ClockEmployeeBenefitID
        /// </summary>
        /// <param name="clientId">ID of client to filter by.</param>
        /// <returns></returns>
        IEmployeeBenefitQuery ByClientId(int clientId);

        /// <summary>
        /// Filters by a particular employee-set.
        /// NOTE: Cluster index exists on ClientID | EmployeeID | EventDate | ClientEarningID | ClockEmployeeBenefitID
        /// </summary>
        /// <param name="employeeIds">ID(s) of employees to filter by.</param>
        /// <returns></returns>
        IEmployeeBenefitQuery ByEmployees(IEnumerable<int> employeeIds);

        /// <summary>
        /// Filters by benefits whose <see cref="ClockEmployeeBenefit.EventDate"/> is greater-than or equal-to the specified
        /// date/time.
        /// NOTE: Cluster index exists on ClientID | EmployeeID | EventDate | ClientEarningID | ClockEmployeeBenefitID
        /// </summary>
        /// <param name="fromDate">Date/time event date must be greater than or equal to.</param>
        /// <returns></returns>
        IEmployeeBenefitQuery ByDateRangeFrom(DateTime fromDate);

        /// <summary>
        /// Filters by benefits whose <see cref="ClockEmployeeBenefit.EventDate"/> is less-than or equal-to the specified
        /// date/time.
        /// NOTE: Cluster index exists on ClientID | EmployeeID | EventDate | ClientEarningID | ClockEmployeeBenefitID
        /// </summary>
        /// <param name="toDate">Date/time event date must be less than or equal to.</param>
        /// <returns></returns>
        IEmployeeBenefitQuery ByDateRangeTo(DateTime toDate);
    }
}
