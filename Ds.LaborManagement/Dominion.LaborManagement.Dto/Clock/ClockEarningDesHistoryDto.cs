using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Clock
{
    public partial class ClockEarningDesHistoryDto
    {
        public int      ClockEarningDesHistoryId      { get; set; }
        public int      EmployeeId                    { get; set; }
        public int      ClientEarningId               { get; set; }
        public double   Hours                         { get; set; }
        public DateTime EventDate                     { get; set; }
        public int?     ClientCostCenterId            { get; set; }
        public int?     ClientDivisionId              { get; set; }
        public int?     ClientDepartmentId            { get; set; }
        public int?     ClientShiftId                 { get; set; }
        public int?     ClockEmployeePunchTypeId      { get; set; }
        public int?     ClockEmployeePunchIdIn        { get; set; }
        public int?     ClockEmployeePunchIdOut       { get; set; }
        public int      ClientId                      { get; set; }
        public int?     ClientJobCostingAssignmentId1 { get; set; }
        public int?     ClientJobCostingAssignmentId2 { get; set; }
        public int?     ClientJobCostingAssignmentId3 { get; set; }
        public int?     ClientJobCostingAssignmentId4 { get; set; }
        public int?     ClientJobCostingAssignmentId5 { get; set; }
        public int?     ClientJobCostingAssignmentId6 { get; set; }
    }
}
