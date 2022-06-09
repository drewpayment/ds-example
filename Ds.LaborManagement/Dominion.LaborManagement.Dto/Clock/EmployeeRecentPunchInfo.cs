using System;
using Dominion.Core.Dto.Labor;

namespace Dominion.LaborManagement.Dto.Clock
{
    public class EmployeeRecentPunchInfo : IHasPunchShiftDateInfo
    {
        public int       ClockEmployeePunchId          { get; set; }
        public DateTime  ModifiedPunch                 { get; set; }
        public DateTime  RawPunch                      { get; set; }
        public DateTime? ShiftDate                     { get; set; }
        public int?      ClientCostCenterId            { get; set; }
        public int?      ClientDepartmentId            { get; set; }
        public int?      ClientDivisionId              { get; set; }
        public int?      ClockClientLunchId            { get; set; }
        public byte?     TransferOption                { get; set; }
        public int?      ClientJobCostingAssignmentId1 { get; set; }
        public int?      ClientJobCostingAssignmentId2 { get; set; }
        public int?      ClientJobCostingAssignmentId3 { get; set; }
        public int?      ClientJobCostingAssignmentId4 { get; set; }
        public int?      ClientJobCostingAssignmentId5 { get; set; }
        public int?      ClientJobCostingAssignmentId6 { get; set; }
    }
}