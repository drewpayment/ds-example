using Dominion.Domain.Entities.Employee.ClockEmployeeInfo;
using Dominion.Utility.Query;
using System;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockEmployeePunchAttemptQuery : IQuery<ClockEmployeePunchAttempt, IClockEmployeePunchAttemptQuery>
    {
        /// <summary>
        /// Filters entities by client id.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IClockEmployeePunchAttemptQuery ByClient(int clientId);

        /// <summary>
        /// Filter entities by clock employee punch attempt id.
        /// </summary>
        /// <param name="clockEmployeePunchAttemptID"></param>
        /// <returns></returns>
        IClockEmployeePunchAttemptQuery ByClockEmployeePunchAttemptID(int clockEmployeePunchAttemptID);

        /// <summary>
        /// Filter entities by employee id.
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        IClockEmployeePunchAttemptQuery ByEmployeeID(int employeeID);

        /// <summary>
        /// Filter entities by clock employee punch location id.
        /// </summary>
        /// <param name="ClockEmployeePunchLocationID"></param>
        /// <returns></returns>
        IClockEmployeePunchAttemptQuery ByClockEmployeePunchLocationID(int clockEmployeePunchLocationID);

        /// <summary>
        /// Filter entities by startDate and endDate.
        /// </summary>
        /// <param name="startDate, endDate"></param>
        /// <returns></returns>
        IClockEmployeePunchAttemptQuery ByDates(DateTime? startDate, DateTime? endDate);

        /// <summary>
        /// Filter entities by non-null punch locations
        /// </summary>
        /// <returns></returns>
        IClockEmployeePunchAttemptQuery ByNonNullPunchLocations();
    }
}