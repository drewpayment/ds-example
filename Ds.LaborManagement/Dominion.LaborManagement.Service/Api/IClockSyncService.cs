using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Services.Dto.Employee;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.LaborManagement.Dto.Department;
using Dominion.LaborManagement.Dto.JobCosting;
using Dominion.LaborManagement.Dto.Scheduling;
using Dominion.Pay.Dto.Earnings;
using Dominion.Utility.OpResult;
using System;
using System.Collections.Generic;

namespace Dominion.LaborManagement.Service.Api
{
    public interface IClockSyncService
    {

        IOpResult<IEnumerable<TimeClockClientDto>> GetAccessibleClients();

        /// <summary>
        /// returns all active departments for a client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClientDepartmentDto>> GetClientDepartments(int? clientId);

        /// <summary>
        /// returns all active department for a client
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClientDivisionDto>> GetClientDivisions(int? clientId);

        IOpResult<IEnumerable<ClockClientHolidayDto>> GetClientHolidays();

        /// <summary>
        /// returns all active job costing levels 
        /// </summary>
        /// <returns></returns>
        IOpResult<IEnumerable<ClientJobCostingDto>> GetClientJobCostings(int? clientId);

        /// <summary>
        /// returns all enabled job costing assignments
        /// </summary>
        /// <returns></returns>
        IOpResult<IEnumerable<ClientJobCostingAssignmentDto>> GetClientJobCostingAssignments(int? clientId);

        /// <summary>
        /// returns all enabled client job costing assignment selecteds
        /// </summary>
        /// <returns></returns>
        IOpResult<IEnumerable<ClientJobCostingAssignmentSelectedDto>> GetClientJobCostingAssignmentSelected(int? clientId);

        IOpResult<IEnumerable<ClockClientLunchDto>> GetClientLunches();

        /// <summary>
        /// returns a list of ClockClientSelected entities.  While the clientId is not part of the table, 
        /// it can be used to verify the requestor is the owner of the time policy
        /// </summary>
        /// 
        /// <returns></returns>
        IOpResult<IEnumerable<ClockClientLunchSelectedDto>> GetClockClientLunchesSelected();

        IOpResult<IEnumerable<ClockClientRulesDto>> GetClientRules();

        /// <summary>
        /// Returns an enumerable of Dto's that contain client Id, employee Id, and clockclientScheduleid 
        /// for active shifts.  If only the client id is provided, selected schedules for all employees will be
        /// returned
        /// </summary>
        /// <param name="employeeId">employee Id</param>
        /// 
        /// <returns></returns>
        IOpResult<IEnumerable<ClockClientScheduleSelectedDto>> GetClockClientScheduleSelected(int? employeeId = null);

        IOpResult<IEnumerable<ClockClientScheduleDto>> GetClockClientSchedules();

        IOpResult<IEnumerable<ClockClientTimePolicyDtos.ClockClientTimePolicyDto>> GetClockClientTimePolicies();

        IOpResult<IEnumerable<ClockEmployeeDto>> GetClockEmployees();

        IOpResult<IEnumerable<ClockEmployeePunchDto>> GetClockEmployeePunches(int count);

        IOpResult<IEnumerable<ClockEmployeeScheduleDto>> GetClockEmployeeSchedules(DateTime startDate, DateTime endDate);

        IOpResult<IEnumerable<ClientCostCenterDto>> GetAvailableCostCenters(int? clientId = null);

        /// <summary>
        /// This replaces Sproc : [dbo].[spGetClockEmployeeScheduleListByDate]
        /// Returns a collection of ClockEmployeeScheduleListDto's based on parameters provider. 
        /// If an employeeId is not provided, the collection will contain schedlues for all employees
        /// </summary>
        /// <param name="clientId">client Id</param>
        /// <param name="employeeId">employee Id</param>
        /// <param name="startDate">start date</param>
        /// <param name="endDate">end date</param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClockEmployeeScheduleListDto>> GetClockEmployeeScheduleListByDate(DateTime startDate, DateTime endDate, int? employeeId = null);

        /// <summary>
        /// Replaces Sproc : [dbo].[spGetClockEmployeeBenefitListByDate]
        /// returns a collection of ClockEmployeeBenefit based on the dates
        /// and parameters provided.  If no employeeId is provided, results for all employees will appear.
        /// For the dates, the start date provided is rounded down to the very start of the day, while the 
        /// end date is rounded up to 23:59:59
        /// 
        /// NOTE: As of 11/01/2016 this implementation will sort by date, but the order may not
        /// be identical to the order returned from the SPROC
        /// </summary>
        /// <param name="startDate">start date</param>
        /// <param name="endDate">end date</param>
        /// <param name="employeeId">employee id</param>
        /// <param name="isWorked"></param>
        /// 
        /// <returns></returns>
        IOpResult<IEnumerable<ClockEmployeeBenefitListDto>> GetClockEmployeeBenefitListByDate(DateTime startDate,
            DateTime endDate, int? employeeId = null, int isWorked = 2);

        IOpResult<IEnumerable<EmployeeBasicDto>> GetEmployees();

        IOpResult<IEnumerable<ClientEarningCompleteDto>> GetEmployeeClientEarnings(int clientId);

        IOpResult<IEnumerable<ClockTimePolicyEmployeeDto>> GetEmployeesByTimePolicy(IEnumerable<int> timePolicyIds);
        IOpResult<IEnumerable<EmployeeDto>> UpdateClockEmployeesGeofence(IEnumerable<EmployeeDto> employees);
    }
}