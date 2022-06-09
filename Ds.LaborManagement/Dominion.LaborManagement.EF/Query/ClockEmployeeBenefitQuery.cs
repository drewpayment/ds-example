using System;
using System.Collections.Generic;
using System.Linq;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public class ClockEmployeeBenefitQuery : Query<ClockEmployeeBenefit, IClockEmployeeBenefitQuery>, IClockEmployeeBenefitQuery
    {
        public ClockEmployeeBenefitQuery(IEnumerable<ClockEmployeeBenefit> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        IClockEmployeeBenefitQuery IClockEmployeeBenefitQuery.ByClockEmployeeBenefit(int clockEmployeeBenefitId)
        {
            FilterBy(c => c.ClockEmployeeBenefitId == clockEmployeeBenefitId);
            return this;
        }

        IClockEmployeeBenefitQuery IClockEmployeeBenefitQuery.ByHolidayDetailList(List<int> holidayDetailList)
        {
            FilterBy(x => holidayDetailList.Contains(x.ClockClientHolidayDetailId ?? 0));
            return this;
        }

        public IClockEmployeeBenefitQuery ByClientId(int clientId)
        {
            FilterBy(x => x.ClientId.Equals(clientId));
            return this;
        }

        public IClockEmployeeBenefitQuery ByClientIds(int[] clientIds)
        {
            FilterBy(x => clientIds.Contains(x.ClientId));
            return this;
        }

        public IClockEmployeeBenefitQuery ByDateRange(DateTime startDate, DateTime endDate)
        {
            this.FilterBy(x => x.EventDate >= startDate && x.EventDate <= endDate);
            return this;
        }

        public IClockEmployeeBenefitQuery ByEmployeeId(int employeeId)
        {
            FilterBy(x => x.EmployeeId.Equals(employeeId));
            return this;
        }

        public IClockEmployeeBenefitQuery ByEmployeeIds(int[] employeeIds)
        {
            FilterBy(x => employeeIds.Contains(x.EmployeeId));
            return this;
        }

 		public IClockEmployeeBenefitQuery ByClockClientHolidayDetail(int clockClientHolidayDetailId)
        {
            FilterBy(x => x.ClockClientHolidayDetailId == clockClientHolidayDetailId);
            return this;
        }

        public IClockEmployeeBenefitQuery ByClockClientHolidayDetailIdExists()
        {
            FilterBy(x => x.ClockClientHolidayDetailId != null);
            return this;
        }

        IClockEmployeeBenefitQuery IClockEmployeeBenefitQuery.ByCostCenterId(int? costCenterId)
        {
            FilterBy(x => x.ClientCostCenterId == costCenterId);
            return this;
        }

        public IClockEmployeeBenefitQuery ByClientEarningId(int clientEarningId)
        {
            FilterBy(x => x.ClientEarningId == clientEarningId);
            return this;
        }

        public IClockEmployeeBenefitQuery IsBenefitHours(bool isBenefitHours)
        {
            if (isBenefitHours)
            {
                FilterBy(x => (x.IsWorkedHours.GetValueOrDefault() == false));
                return this;
            }
            FilterBy(x => (x.IsWorkedHours == true));
            return this;
        }

        IClockEmployeeBenefitQuery IClockEmployeeBenefitQuery.ByRequestTimeOffDetailId(int timeOffRequestDetailId)
        {
            FilterBy(x => x.RequestTimeOffDetailId == timeOffRequestDetailId);
            return this;
        }
        
        public IClockEmployeeBenefitQuery ByClientDepartmentIds(List<int?> clientDepartmentIds)
        {
            FilterBy(x => clientDepartmentIds.Contains(x.ClientDepartmentId));
            return this;
        }
    }
}