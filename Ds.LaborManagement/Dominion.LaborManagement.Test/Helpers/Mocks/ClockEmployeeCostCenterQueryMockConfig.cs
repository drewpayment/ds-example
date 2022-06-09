using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Query;
using Dominion.Testing.Util.Helpers.Mocks;
using Moq;

namespace Dominion.LaborManagement.Test.Helpers.Mocks
{
    public class ClockEmployeeCostCenterQueryMockConfig  : QueryMockConfig<IClockEmployeeCostCenterQuery, ClockEmployeeCostCenter>
    {
        public ClockEmployeeCostCenterQueryMockConfig()
        {
            this.DefineChainingMethodCall(x => x.ByCostCenterId(It.IsAny<int>()));
        }
    }

    public class EmployeeSchedulePreviewQueryMockConfig  : QueryMockConfig<IEmployeeSchedulePreviewQuery, EmployeeSchedulePreview>
    {
        public EmployeeSchedulePreviewQueryMockConfig()
        {
            this.DefineChainingMethodCall(x => x.ByEventDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()));
            this.DefineChainingMethodCall(x => x.ByScheduleGroupId(It.IsAny<int>()));
            this.DefineChainingMethodCall(x => x.ByScheduleGroupsOtherThanId(It.IsAny<int>()));
            this.DefineChainingMethodCall(x => x.ByEmployeeIds(It.IsAny<IEnumerable<int>>()));
        }
    }

    public class ClockEmployeeScheduleQueryMockConfig  : QueryMockConfig<IClockEmployeeScheduleQuery, ClockEmployeeSchedule>
    {
        public ClockEmployeeScheduleQueryMockConfig()
        {
            this.DefineChainingMethodCall(x => x.ByEventDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>()));
            this.DefineChainingMethodCall(x => x.ByEmployeeIds(It.IsAny<IEnumerable<int>>()));

            this.DefineChainingMethodCall(x => x.ByScheduleGroupId(It.IsAny<int>()));

            this.DefineChainingMethodCall(x => x.ByScheduleGroupsOtherThanId(It.IsAny<int>()));
        }
    }

    public class EmployeeDefaultShiftQueryMockConfig : QueryMockConfig<IEmployeeDefaultShiftQuery, EmployeeDefaultShift>
    {
        public EmployeeDefaultShiftQueryMockConfig()
        {
            this.DefineChainingMethodCall(x => x.ByEmployeesIds(It.IsAny<IEnumerable<int>>()));
            this.DefineChainingMethodCall(x => x.ByGroupScheduleId(It.IsAny<int>()));
            this.DefineChainingMethodCall(x => x.ByScheduleGroupId(It.IsAny<int>()));
        }
    }

}
