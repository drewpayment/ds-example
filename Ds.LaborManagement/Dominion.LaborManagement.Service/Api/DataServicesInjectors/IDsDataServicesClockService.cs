using System;
using System.ComponentModel;
using Dominion.Core.Services.Interfaces;
using Dominion.LaborManagement.Dto.DataServiceObjects;
using Dominion.LaborManagement.Dto.JobCosting;

namespace Dominion.LaborManagement.Service.Api.DataServicesInjectors
{
    /// <summary>
    /// Service used to access legacey code in the DominionSource.DataServices module
    /// </summary>
    public interface IDsDataServicesClockService
    {
        /// <summary>
        /// Method used after inserting or deleting a punch that recalculates an employee's hours
        /// for the week.  If a punch was added, the return bool value will indicate true if the
        /// punch was a duplicate punch
        /// </summary>
        /// <param name="requestArgs"></param>
        /// <returns></returns>
        bool CalculateWeeklyActivity(CalculateWeeklyActivityRequestArgs requestArgs, IClientService clientService);

        /// <summary>
        /// Method used after inserting Input Punch Hours or Benefits into the ClockEmployeeBenefit 
        /// table that calculates the earning types and totals. Returns bool as success.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="employeeId"></param>
        /// <param name="eventDate"></param>
        /// <returns></returns>
        bool CalculateWorkedHours(int clientId, int employeeId, DateTime eventDate);

        /// <summary>
        /// Deletes a punch
        /// </summary>
        /// <param name="punchId"></param>
        /// <param name="userId"></param>
        void DeleteClockEmployeePunch(int punchId, int userId);

        /// <summary>
        /// Deletes job costing data, if it exists
        /// </summary>
        /// <param name="punchId"></param>
        /// <param name="userId"></param>
        void DeleteEmployeeJobCosting(int punchId, int userId);

        /// <summary>
        /// Returns a <see cref="DataServicesGetScheduleResult"/> object containing an employee's
        /// schedule information
        /// </summary>
        /// <param name="requestArgs"></param>
        /// <returns></returns>
        DataServicesGetScheduleResult GetSchedule(DataServicesGetScheduleArgs requestArgs);

        /// <summary>
        /// Inserts a punch using the logic as if inserting a punch via the punch screen 
        /// </summary>
        /// <param name="punchArgs"></param>
        /// <returns></returns>
        int InsertClockEmployeePunch_PunchScreen(PunchScreenPunchRequestArgs punchArgs);

        /// <summary>
        /// Round a punch according to an employee's Time Policy and Rules
        /// </summary>
        /// <param name="requestArgs"></param>
        /// <param name="clockExceptionType"></param>
        /// <returns></returns>
        DateTime RoundPunch(RoundPunchRequestArgs requestArgs, int clockExceptionType);
    }
}