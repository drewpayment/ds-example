using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public class ClockEarningDesHistoryQuery : Query<ClockEarningDesHistory, IClockEarningDesHistoryQuery>, IClockEarningDesHistoryQuery
    {
        #region Constructor

        public ClockEarningDesHistoryQuery(IEnumerable<ClockEarningDesHistory> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        #endregion

        /// <summary>
        /// Filters ClockEarningDesHistory records by ClientId
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IClockEarningDesHistoryQuery IClockEarningDesHistoryQuery.ByClient(int clientId)
        {
            FilterBy(x => x.ClientId == clientId);
            return this;
        }
        /// <summary>
        /// Filters ClockEarningDesHistory records by ClientIds
        /// </summary>
        /// <param name="clientIds"></param>
        /// <returns></returns>
        IClockEarningDesHistoryQuery IClockEarningDesHistoryQuery.ByClients(int[] clientIds)
        {
            FilterBy(x => clientIds.Contains(x.ClientId));
            return this;
        }
        /// <summary>
        /// Filters ClockEarningDesHistory records between two dates/times
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        IClockEarningDesHistoryQuery IClockEarningDesHistoryQuery.ByEventDateRange(DateTime startTime, DateTime endTime)
        {
            FilterBy(x => x.EventDate >= startTime && x.EventDate <= endTime);
            return this;
        }

        IClockEarningDesHistoryQuery IClockEarningDesHistoryQuery.ByEmployee(int employeeId)
        {
            FilterBy(x => x.EmployeeId == employeeId);
            return this;
        }

        IClockEarningDesHistoryQuery IClockEarningDesHistoryQuery.ByEmployeeIds(IEnumerable<int> empIds)
        {
            this.FilterBy(x => empIds.Contains(x.EmployeeId));
            return this;
        }
        IClockEarningDesHistoryQuery IClockEarningDesHistoryQuery.ByClientShiftIds(IEnumerable<int?> clientShiftIds)
        {
            this.FilterBy(x => clientShiftIds.Contains(x.ClientShiftId));
            return this;
        }

        IClockEarningDesHistoryQuery IClockEarningDesHistoryQuery.ByClientDepartmentIds(List<int?> clientDepartmentIds)
        {
            this.FilterBy(x => clientDepartmentIds.Contains(x.ClientDepartmentId));
            return this;
        }
        IClockEarningDesHistoryQuery IClockEarningDesHistoryQuery.ByDivisionIds(IEnumerable<int?> divisionIds)
        {
            this.FilterBy(x => divisionIds.Contains(x.ClientDivisionId));
            return this;
        }
    }
}
