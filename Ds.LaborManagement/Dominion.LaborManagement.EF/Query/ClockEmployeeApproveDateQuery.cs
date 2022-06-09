using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Utility.Query;

namespace Dominion.LaborManagement.EF.Query
{
    public class ClockEmployeeApproveDateQuery : Query<ClockEmployeeApproveDate, IClockEmployeeApproveDateQuery>, IClockEmployeeApproveDateQuery
    {
        #region Constructor

        public ClockEmployeeApproveDateQuery(IEnumerable<ClockEmployeeApproveDate> data, IQueryResultFactory resultFactory = null) : base(data, resultFactory)
        {
        }

        #endregion

        /// <summary>
        /// Filters approval status for a single client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IClockEmployeeApproveDateQuery IClockEmployeeApproveDateQuery.ByClientId(int clientId)
        {
            FilterBy(x => x.ClientId == clientId);
            return this;
        }

        IClockEmployeeApproveDateQuery IClockEmployeeApproveDateQuery.ByEmployeeId(int employeeId)
        {
            FilterBy(x => x.EmployeeId == employeeId);
            return this;
        }

        /// <summary>
        /// Filters approval status by one or more employees.
        /// </summary>
        /// <param name="employeeIds"></param>
        /// <returns></returns>
        IClockEmployeeApproveDateQuery IClockEmployeeApproveDateQuery.ByEmployees(IEnumerable<int> employeeIds)
        {
            if(employeeIds != null)
            {
                var ids = employeeIds.ToArray();
                if(ids.Length == 1)
                {
                    var id = ids[0];
                    FilterBy(x => x.EmployeeId == id);
                }
                else
                {
                    FilterBy(x => ids.Contains(x.EmployeeId));
                }
            }
            return this;
        }

        IClockEmployeeApproveDateQuery IClockEmployeeApproveDateQuery.ByEventDate(DateTime eventDate)
        {
            FilterBy(x => DbFunctions.TruncateTime(x.EventDate) == DbFunctions.TruncateTime(eventDate));
            return this;
        }

        IClockEmployeeApproveDateQuery IClockEmployeeApproveDateQuery.ByCostCenterId(int costCenterId)
        {
            FilterBy(x => x.ClientCostCenterId == costCenterId);
            return this;
        }

        IClockEmployeeApproveDateQuery IClockEmployeeApproveDateQuery.ByNullCostCenter()
        {
            FilterBy(x => x.ClientCostCenterId == null || x.ClientCostCenterId == 0);
            return this;
        }

        /// <summary>
        /// Filters approval status for dates on or after a given day.
        /// </summary>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        IClockEmployeeApproveDateQuery IClockEmployeeApproveDateQuery.ByEventDateFrom(DateTime fromDate)
        {
            FilterBy(x => x.EventDate >= fromDate);
            return this;
        }

        /// <summary>
        /// Filters approval status for dates on or before a given day.
        /// </summary>
        /// <param name="toDate"></param>
        /// <returns></returns>
        IClockEmployeeApproveDateQuery IClockEmployeeApproveDateQuery.ByEventDateTo(DateTime toDate)
        {
            FilterBy(x => x.EventDate <= toDate);
            return this;
        }

        /// <summary>
        /// Filters records by clock client note id.
        /// </summary>
        /// <param name="clockClientNoteId"></param>
        /// <returns></returns>
        IClockEmployeeApproveDateQuery IClockEmployeeApproveDateQuery.ByClockClientNoteId(int clockClientNoteId)
        {
            FilterBy(x => x.ClockClientNoteId == clockClientNoteId);
            return this;
        }

        /// <inheritdoc/>
        IClockEmployeeApproveDateQuery IClockEmployeeApproveDateQuery.ByIsApproved(bool isApproved)
        {
            FilterBy(x => x.IsApproved == isApproved);
            return this;
        }
    }
}