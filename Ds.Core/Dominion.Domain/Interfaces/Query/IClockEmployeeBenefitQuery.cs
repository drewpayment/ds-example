using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClockEmployeeBenefitQuery : IQuery<ClockEmployeeBenefit, IClockEmployeeBenefitQuery>
    {
        IClockEmployeeBenefitQuery ByClientId(int clientId);

        IClockEmployeeBenefitQuery ByClientIds(int[] clientIds);

        IClockEmployeeBenefitQuery ByDateRange(DateTime startDate, DateTime endDate);

        IClockEmployeeBenefitQuery ByEmployeeId(int employeeId);
        IClockEmployeeBenefitQuery ByEmployeeIds(int[] employeeIds);

        IClockEmployeeBenefitQuery ByClockClientHolidayDetail(int clockClientHolidayDetailId);

        /// <summary>
        /// Filters a list of clock employee benefit entities by a list of ClockClientHolidayDetail entities.
        /// </summary>
        /// <param name="holidayDetailList"></param>
        /// <returns></returns>
        IClockEmployeeBenefitQuery ByHolidayDetailList(List<int> holidayDetailList);

        /// <summary>
        /// Filters a list of clock employee benefit entities to only benefit entities that have ClockClientHolidayDetailIds
        /// </summary>
        /// <returns></returns>
        IClockEmployeeBenefitQuery ByClockClientHolidayDetailIdExists();

        /// <summary>
        /// Filters a list of clock employee benefits by primary key.
        /// </summary>
        /// <param name="clockEmployeeBenefitId"></param>
        /// <returns></returns>
        IClockEmployeeBenefitQuery ByClockEmployeeBenefit(int clockEmployeeBenefitId);
		
        IClockEmployeeBenefitQuery ByCostCenterId(int? costCenterId);

        /// <summary>
        /// Filters a list of clock employee benefit entities by the client earning ID.
        /// </summary>
        /// <param name="clientEarningId">The ID of the client earning to filter by.</param>
        /// <returns></returns>
        IClockEmployeeBenefitQuery ByClientEarningId(int clientEarningId);

        IClockEmployeeBenefitQuery IsBenefitHours(bool isBenefitHours);

        IClockEmployeeBenefitQuery ByRequestTimeOffDetailId(int timeOffRequestDetailId);
        IClockEmployeeBenefitQuery ByClientDepartmentIds(List<int?> clientDepartmentIds);
    }
}