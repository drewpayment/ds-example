using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Sprocs;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Entities.TimeClock;
using Dominion.Domain.Interfaces.Query;
using Dominion.Domain.Interfaces.Query.Core;
using Dominion.LaborManagement.Dto.Sproc;
using Dominion.Utility.Query;
using TimeZone = Dominion.Domain.Entities.Misc.TimeZone;

namespace Dominion.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Repository providing methods to retrieve TimeClock entities from a datastore.
    /// </summary>
    public interface ITimeClockRepository
    {
        /// <summary>
        /// Retrieves the ClockEmployees which match the specified query criteria from the datastore.
        /// </summary>
        /// <typeparam name="TResult">Type the results will be mapped to.</typeparam>
        /// <param name="query">Query containing the filter, sort and result-map criteria to be performed.</param>
        /// <returns>Collection of objects which match the specified query.</returns>
        IEnumerable<TResult> GetClockEmployees<TResult>(QueryBuilder<ClockEmployee, TResult> query)
            where TResult : class;


        /// <summary>
        /// Query that will return employees punches.  If you need to add to the query, access the interface and implemenation
        /// that allow for inline fluent syntax.
        /// </summary>
        /// <returns></returns>
        IClockEmployeePunchQuery GetClockEmployeePunchQuery();


        /// <summary>
        /// Query that returns all clock employees.  This table contains a BadgeNumber field
        /// that can be used to lookup employees by their "Badge" or "Pin" number
        /// </summary>
        /// <returns></returns>
        IClockEmployeeQuery GetClockEmployeeQuery();

        /// <summary>
        /// Query that returns all Clock Holidays 
        /// </summary>
        /// <returns></returns>
        IClockClientHolidayQuery GetClockClientHolidayQuery();

        /// <summary>
        /// Query that returns the ClockClientHardwares.
        /// </summary>
        /// <returns></returns>
        IClockClientHardwareQuery QueryClockClientHardware();

        /// <summary>
        /// Query that returns the ClockClientLunches.
        /// </summary>
        /// <returns></returns>
        IClockClientLunchQuery GetClockClientLunchQuery();

        /// <summary>
        /// Query that returns ClockClientRules
        /// </summary>
        /// <returns></returns>
        IClockClientRulesQuery GetClockClientRules();

        /// <summary>
        /// Returns all time policies for clients.
        /// </summary>
        /// <returns></returns>
        IClockClientTimePolicyQuery GetClockClientTimePolicyQuery();

        /// <summary>
        ///  Query that returns all DayLightSavingsTime objects 
        /// </summary>
        /// <returns></returns>
        IDayListSavingsTimeQuery DayListSavingsTimeQuery();

        /// <summary>
        /// Query that returns all ClockRoundingTypes objects
        /// </summary>
        /// <returns></returns>
        IClockRoundingTypeQuery ClockRoundingTypeQuery();

        /// <summary>
        /// returns the most recent pay period ended.  If providing only a client ID, the result will contain the pay
        /// period ended for all employees- which is a large query.  By supplying and employee id, it will return only the
        /// pay period ended for the specified employee. 
        /// </summary>
        /// <param name="clientId">id for client</param>
        /// <param name="employeeId">id for employee</param>
        /// <returns></returns>
        IEnumerable<ClockEmployeePayPeriodEndedDto> ClockEmployeePayPeriodEndedSproc(int? clientId = null, int? employeeId = null);

        /// <summary>
        /// Returns the most recent / last punch for an employee.  This is needed to determine if a transfer punch is necessary. 
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="employeeId">Id for Employee</param>
        /// <param name="costCenterId">Id for ClientCostCenter</param>
        /// <param name="divisionId">Id for ClientDivision</param>
        /// <param name="departmentId">Id for ClientDepartment</param>
        /// <param name="jobCostingAssignment1Id">If for Job Costing Assignment, if applicable</param>
        /// <param name="jobCostingAssignment2Id">If for Job Costing Assignment, if applicable</param>
        /// <param name="jobCostingAssignment3Id">If for Job Costing Assignment, if applicable</param>
        /// <param name="jobCostingAssignment4Id">If for Job Costing Assignment, if applicable</param>
        /// <param name="jobCostingAssignment5Id">If for Job Costing Assignment, if applicable</param>
        /// <param name="jobCostingAssignment6Id">If for Job Costing Assignment, if applicable</param>
        /// <param name="clockClientLunchId"></param>
        /// name="clientId">Id for Client/param>
        /// <returns></returns>
        ClockEmployeeLastPunchDto ClockEmployeeLastPunchSproc(int clientId, int employeeId, DateTime punchDateTime,
            int? costCenterId = null, int? divisionId = null, int? departmentId = null,
            int? jobCostingAssignment1Id = null, int? jobCostingAssignment2Id = null,
            int? jobCostingAssignment3Id = null, int? jobCostingAssignment4Id = null,
            int? jobCostingAssignment5Id = null, int? jobCostingAssignment6Id = null,
            int? clockClientLunchId = null);

        /// <summary>
        /// returns the time as set on the Dominion Sql Server
        /// </summary>
        /// <returns></returns>
        SqlServerTimeDto GetSqlServerTimeSproc();

        /// <summary>
        /// returns an enumerable of all timezones
        /// </summary>
        /// <returns></returns>
        IEnumerable<TimeZone> GetTimeZones();
        IEnumerable<GetClockClientNoteListResultDto> GetClockClientNoteList(GetClockClientNoteListArgsDto args);

        /// <summary>
        /// Query that returns all ClientMachine objects
        /// </summary>
        /// <returns></returns>
        IClientMachineQuery QueryClientMachines();
    }
}