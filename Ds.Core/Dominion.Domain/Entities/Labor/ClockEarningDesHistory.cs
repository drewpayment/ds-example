using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Employee;

namespace Dominion.Domain.Entities.Labor
{
    public partial class ClockEarningDesHistory : Entity<ClockEarningDesHistory>
    {
        public virtual int      ClockEarningDesHistoryId      { get; set; }
        public virtual int      EmployeeId                    { get; set; }
        public virtual int      ClientEarningId               { get; set; }
        public virtual double   Hours                         { get; set; }
        public virtual DateTime EventDate                     { get; set; }
        public virtual int?     ClientCostCenterId            { get; set; }
        public virtual int?     ClientDivisionId              { get; set; }
        public virtual int?     ClientDepartmentId            { get; set; }
        public virtual int?     ClientShiftId                 { get; set; }
        public virtual int?     ClockEmployeePunchTypeId      { get; set; }
        public virtual int?     ClockEmployeePunchIdIn        { get; set; }
        public virtual int?     ClockEmployeePunchIdOut       { get; set; }
        public virtual int      ClientId                      { get; set; }
        public virtual int?     ClientJobCostingAssignmentId1 { get; set; }
        public virtual int?     ClientJobCostingAssignmentId2 { get; set; }
        public virtual int?     ClientJobCostingAssignmentId3 { get; set; }
        public virtual int?     ClientJobCostingAssignmentId4 { get; set; }
        public virtual int?     ClientJobCostingAssignmentId5 { get; set; }
        public virtual int?     ClientJobCostingAssignmentId6 { get; set; }

        public virtual Employee.Employee Employee { get; set; }
        public virtual ClockEmployeePunch ClockEmployeePunchIn { get; set; }
        public virtual ClockEmployeePunch ClockEmployeePunchOut { get; set; }

        public ClockEarningDesHistory()
        {
        }
    }
}
