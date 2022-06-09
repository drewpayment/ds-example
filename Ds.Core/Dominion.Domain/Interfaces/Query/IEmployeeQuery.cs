using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dominion.Aca.Dto.Forms;
using Dominion.Core.Dto.Performance;
using Dominion.Domain.Entities.Employee;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Query on <see cref="Employee"/> data.
    /// </summary>
    public interface IEmployeeQuery : IQuery<Domain.Entities.Employee.Employee, IEmployeeQuery>
    {
        /// <summary>
        /// Filters employees by the specified ID.
        /// </summary>
        /// <param name="employeeId">ID of employee.</param>
        /// <returns></returns>
        IEmployeeQuery ByEmployeeId(int employeeId);

        IEmployeeQuery ByNotEmployeeId(int employeeId);

        /// <summary>
        /// Filters employees by the specified client.
        /// </summary>
        /// <param name="clientId">Client to filter employees by.</param>
        /// <returns></returns>
        IEmployeeQuery ByClientId(int clientId);

        /// <summary>
        /// Filters employees by the specified clients.
        /// </summary>
        /// <param name="clientIds">Clients to filter employees by.</param>
        /// <returns></returns>
        IEmployeeQuery ByClientIds(int[] clientIds);

        /// <summary>
        /// Filters employees by the given employee numbers.
        /// </summary>
        /// <param name="employeeNumbers">Employee numbers to query employees by.</param>
        /// <returns></returns>
        IEmployeeQuery ByEmployeeNumbers(params string[] employeeNumbers);

        /// <summary>
        /// Filters emploeyes by those with the given social security number(s).
        /// </summary>
        /// <param name="ssns">Social security numbers of employees to get.</param>
        /// <returns></returns>
        IEmployeeQuery BySocialSecurityNumber(params string[] ssns);

        /// <summary>
        /// Expects <paramref name="ssns"/> have already been formated to be strings of length=11, in the format of "AAA-BB-CCCC".
        /// </summary>
        /// <param name="source"></param>
        /// <param name="ssns">Expects <paramref name="ssns"/> have already been formated to be strings of length=11, in the format of "AAA-BB-CCCC".</param>
        /// <returns></returns>
        IEmployeeQuery BySocialSecurityNumberPreFormatted(params string[] ssns);

        /// <summary>
        /// Filters by the specified employees.
        /// </summary>
        /// <param name="empIds">IDs of employees to filter by.</param>
        /// <returns></returns>
        IEmployeeQuery ByEmployeeIds(IEnumerable<int> empIds);

        /// <summary>
        /// Filters by employees that do not have ACA 1095C covered individual data for the specified reporting year.
        /// </summary>
        /// <param name="year">ACA reporting year to check for covered individual data.</param>
        /// <param name="individualType">If specified, will only check for the specified type of individual. If null, all types will be queried.</param>
        /// <returns></returns>
        IEmployeeQuery ByDoesNotHave1095CCoveredIndividualDataForYear(int year, Aca1095CCoveredIndividualType? individualType = null);

        /// <summary>
        /// Filters Employees based on if they have schedules.
        /// By default, it will filter employees that do have a schedule
        /// </summary>
        /// <param name="hasSchedules">true returns employess with schedules,
        /// false returns employees without schedules</param>
        /// <returns></returns>
        IEmployeeQuery HasSchedules(bool hasSchedules = true);

        /// <summary>
        /// Filters by employees that belong to the client with the given code.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        IEmployeeQuery ByClientCode(string clientCode);

        IEmployeeQuery HiredBefore(DateTime date);
        IEmployeeQuery SeperatedBefore(DateTime startDate, DateTime endDate);

        IEmployeeQuery ByClockClientTimePolicyId(int clockClientTimePolicyId);

        /// <summary>
        /// Filter a list of employees by a list of time policy ids.
        /// </summary>
        /// <param name="timePolicyIds"></param>
        /// <returns></returns>
        IEmployeeQuery ByTimePolicyList(List<int> timePolicyIds);

        /// <summary>
        /// Filters by employees belonging to the specified home cost center.
        /// </summary>
        /// <param name="costCenterId"></param>
        /// <returns></returns>
        IEmployeeQuery ByCostCenter(int? costCenterId);
        IEmployeeQuery ByCostCenters(List<int> costCenterIds);

        /// <summary>
        /// Filter employees by department id.
        /// </summary>
        /// <param name="clientDepartmentId"></param>
        /// <returns></returns>
        IEmployeeQuery ByClientDepartment(int clientDepartmentId);
        IEmployeeQuery ByClientDepartments(List<int> clientDepartmentIds);

        /// <summary>
        /// Filter employee list by active status.
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        IEmployeeQuery ByActiveStatus(bool isActive = true);

        /// <summary>
        /// Filter employee list by those who have a direct supervisor
        /// </summary>
        /// <returns></returns>
        IEmployeeQuery ByHasDirectSupervisor();

        IEmployeeQuery ByHasDirectSupervisorWithActiveEmloyee();

        /// <summary>
        /// Filter employee list by those who have a date of birth within the given date range (inclusive).
        /// </summary>
        /// <param name="dateRangeStart">The start of the date range.</param>
        /// <param name="dateRangeEnd">The end of the date range</param>
        /// <returns></returns>
        IEmployeeQuery ByBirthdayInDateRange(DateTime dateRangeStart, DateTime dateRangeEnd);

        /// <summary>
        /// Filter employee list by those who have a work anniversary within the given date range (inclusive).
        /// </summary>
        /// <param name="dateRangeStart">The start of the date range.</param>
        /// <param name="dateRangeEnd">The end of the date range</param>
        /// <returns></returns>
        IEmployeeQuery ByHireDateInDateRange(DateTime dateRangeStart, DateTime dateRangeEnd);

        /// <summary>
        /// Filter employee list by those who have a work anniversary within the given date range (inclusive).
        /// </summary>
        /// <param name="dateRangeStart">The start of the date range.</param>
        /// <param name="dateRangeEnd">The end of the date range.</param>
        /// <returns></returns>
        IEmployeeQuery ByWorkAnniversaryInDateRange(DateTime dateRangeStart, DateTime dateRangeEnd);
        
        /// <summary>
        /// Filter employee list by those who have a 90 day work anniversary within the given month.
        /// </summary>
        /// <param name="dateRangeStart">The start of the date range.</param>
        /// <param name="dateRangeEnd">The end of the date range.</param>
        /// <returns></returns>
        IEmployeeQuery ByNinetyDayWorkAnniversaryInMonth(DateTime monthStartDate);
		
		IEmployeeQuery ByClientsThatHaveReviewTemplateType(ReferenceDate referenceDate);

        /// <summary>
        /// Filters by employees belonging to the specified division.
        /// </summary>
        /// <param name="divisionId"></param>
        /// <returns></returns>
        IEmployeeQuery ByClientDivisionId(int? divisionId);

        /// <summary>
        /// Filters by employees belonging to a certain job title.
        /// </summary>
        /// <param name="jobProfileId"></param>
        /// <returns></returns>
        IEmployeeQuery ByJobProfileId(int? jobProfileId);

        /// <summary>
        /// Filters by employees belonging to a certain group.
        /// </summary>
        /// <param name="clientGroupId"></param>
        /// <returns></returns>
        IEmployeeQuery ByClientGroupId(int? clientGroupId);

        /// <summary>
        /// Filters by employees belonging to a certain shift.
        /// </summary>
        /// <param name="shiftId"></param>
        /// <returns></returns>
        IEmployeeQuery ByClientShiftId(int? shiftId);

        /// <summary>
        /// Filters by employees belonging to a certain status.
        /// </summary>
        /// <param name="employeeStatusId"></param>
        /// <returns></returns>
        IEmployeeQuery ByEmployeeStatusId(int? employeeStatusId);

        /// <summary>
        /// Filters by employees belonging to a certain pay type.
        /// </summary>
        /// <param name="payTypeId"></param>
        /// <returns></returns>
        IEmployeeQuery ByPayTypeId(int? payTypeId);

        /// <summary>
        /// Filters by employees belonging to a certain supervisor.
        /// </summary>
        /// <param name="directSupervisorId"></param>
        /// <returns></returns>
        IEmployeeQuery ByDirectSupervisorId(int? directSupervisorId);

        /// <summary>
        /// Filters by employees belonging to a certain competency model.
        /// </summary>
        /// <param name="competencyModelId"></param>
        /// <returns></returns>
        IEmployeeQuery ByCompetencyModelId(int? competencyModelId);
        IEmployeeQuery ByAllEmployeesId(int? employeeId);
        IEmployeeQuery HasUserAccounts();
        IEmployeeQuery HasMissingEeocInformation();
        IEmployeeQuery HasNoUserAccount();
        IEmployeeQuery OrderByLastName();
        IEmployeeQuery ByIsActive(bool isActive);
    }
}
