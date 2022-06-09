using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeeSchedulePreviewQuery : IQuery<EmployeeSchedulePreview, IEmployeeSchedulePreviewQuery>
    {
        IEmployeeSchedulePreviewQuery ByEventDateRange(DateTime startDate, DateTime endDate);
        IEmployeeSchedulePreviewQuery ByScheduleGroupId(int scheduleGroupId);
        IEmployeeSchedulePreviewQuery ByScheduleGroupsOtherThanId(int scheduleGroupIdToExclude);
        IEmployeeSchedulePreviewQuery ByEmployeeIds(IEnumerable<int> employeeIds);
    }
}
