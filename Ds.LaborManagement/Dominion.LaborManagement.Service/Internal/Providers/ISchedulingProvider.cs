using Dominion.Core.Dto.Labor;
using Dominion.LaborManagement.Dto.Clock;
using Dominion.LaborManagement.Dto.Scheduling;
using Dominion.Utility.OpResult;
using System;
using System.Collections.Generic;

namespace Dominion.LaborManagement.Service.Internal.Providers
{
    public interface ISchedulingProvider
    {
        /// <summary>
        /// Get the scheduled information based on the parameters passed in.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="scheduleGroupId">The schedule group id.</param>
        /// <param name="scheduleGroupSourceId">The source id of the source group (cost center id).</param>
        /// <returns></returns>
        IOpResult<IEnumerable<EmployeeSchedulesDto>> GetEmployeeScheduleShifts(int clientId, DateTime startDate, DateTime endDate, int scheduleGroupId, int scheduleGroupSourceId);

        /// <summary>
        /// returns and enumerable of clock employee benefites.  This will include paid days off, etc
        /// </summary>
        /// <param name="clientId">client Id</param>
        /// <param name="startdate">start of date range</param>
        /// <param name="endDate">end of date range</param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClockEmployeeBenefitDto>> GetClockEmployeeBenefit(int clientId, DateTime startdate, DateTime endDate);

        /// <summary>
        /// returns an enumerable fo employee shifts with a specific date range.  
        /// </summary>
        /// <param name="clientId">client id</param>
        /// <param name="startDate">start date for schedules</param>
        /// <param name="endDate">end date for schedules</param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClockEmployeeScheduleDto>> GetClockEmployeeSchedules(int clientId, DateTime startDate, DateTime endDate);

        IOpResult<IEnumerable<ClockEmployeeScheduleDto>> GetClockEmployeeSchedules(DateTime startDate, DateTime endDate, int employeeId);

        /// <summary>
        /// returns an enumerable for employee schedule that a specific punch time will fall into.  
        /// </summary>
        /// <param name="employeeId">employee id</param>
        /// <param name="punchTime">punch time used to find schedules</param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClockEmployeeScheduleDto>> GetClockEmployeeSchedulesByPunchTime(DateTime punchTime, int employeeId, short startGrace, short endGrace);


        /// <summary>
        /// returns an enumerable of all schedules for a particular client
        /// </summary>
        /// <param name="clientId">id for the client</param>
        /// <returns></returns>
        IOpResult<IEnumerable<ClockClientScheduleDto>> GetClockClientSchedules(int clientId);

        /// <summary>
        /// Insert, Update or delete employee shifts.
        /// </summary>
        /// <param name="data">Information about the schedules that directs what actions take place.</param>
        /// <returns></returns>
        IOpResult SaveOrUpdateEmployeeScheduleShifts(EmployeeSchedulesPersistDto data);
        
        ///// <summary>
        ///// Get the default shifts for a specific week, scheduleGroup and/or specific employee(s).
        ///// </summary>
        ///// <param name="startDate">The start of the schedule week.</param>
        ///// <param name="groupScheduleId">The group schedule Id.</param>
        ///// <param name="scheduleGroupId">The schedule group id (can be null).</param>
        ///// <param name="employeeIds">Can be empty but if values are detected it will limit the data based on the employee ids in this list.</param>
        ///// <returns></returns>
        //IOpResult<IEnumerable<ScheduleShiftDto>> GetEmployeeDefaultShifts(
        //    DateTime startDate, 
        //    int? groupScheduleId, 
        //    int? scheduleGroupId, 
        //    IEnumerable<int> employeeIds = null);

        /// <summary>
        /// Returns a combination of the unapproved time-off and approved benefits for the specified employees.
        /// </summary>
        /// <param name="clientId">Client associated with the employees.</param>
        /// <param name="employeeIds">Employees to get benefits for.</param>
        /// <param name="fromDate">Start of date range to get benefits for. If null, no from-date filter will be applied.</param>
        /// <param name="toDate">End of date range to get benefits for. If null, no to-date filter will be applied.</param>
        /// <returns></returns>
        IOpResult<IEnumerable<ScheduledBenefitDto>> GetScheduledBenefits(int clientId, IEnumerable<int> employeeIds, DateTime? fromDate = null, DateTime? toDate = null);
    }
}