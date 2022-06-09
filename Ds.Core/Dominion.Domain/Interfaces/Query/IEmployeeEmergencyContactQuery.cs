using System;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Queries <see cref="EmployeeEmergencyContact"/> data.
    /// </summary>
    public interface IEmployeeEmergencyContactQuery : IQuery<EmployeeEmergencyContact, IEmployeeEmergencyContactQuery>
    {
        IEmployeeEmergencyContactQuery ByEmployeeEmergencyContactId(int employeeEmergencyContactId);

        /// <summary>
        /// Filters emergency contacts by those who belong to the specified employee.
        /// </summary>
        /// <param name="employeeId">Employee to filter by.</param>
        /// <returns></returns>
        IEmployeeEmergencyContactQuery ByEmployeeId(int employeeId);

        /// <summary>
        /// Filters emergency contact by those who belong to one of the specified employees.
        /// </summary>
        /// <param name="employeeIds">Employees to filter by.</param>
        /// <returns></returns>
        IEmployeeEmergencyContactQuery ByEmployeeIds(params int[] employeeIds);

        /// <summary>
        /// Filters by emergency contacts that have been approved by the company admin.
        /// </summary>
        /// <returns></returns>
        IEmployeeEmergencyContactQuery ByApprovedEmergencyContactsOnly();
    }
}
