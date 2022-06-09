using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class ListByDateAndFilter
    {
        public class Punch
        {
            public int? TimeZoneID;
            public int? ClientCostCenterID;
            public int EmployeeID { get; set; }

            public string EmployeeNumber { get; set; }

            public DateTime ModifiedPunch { get; set; }

            public int? ClockEmployeePunchID { get; set; }

            public int? ClockClientLunchID { get; set; }

            public string DateOfPunch { get; set; }

            public string EmployeeName { get; set; }

            public string ShiftDateTime { get; set; }

            public string ShiftDateString { get; set; }

            public DateTime? OriginalShiftDate { get; set; }

            public string Comment { get; set; }

            public string ClockName { get; set; }

            public bool IsPendingBenefit { get; set; }

            public string EmployeeComment { get; set; }

            public string CostCenterDesc { get; set; }

            public string DepartmentDesc { get; set; }
            public string DivisionDesc { get; set; }

            public int? ClientJobCostingAssignmentID_1 { get; set; }
            public int? ClientJobCostingAssignmentID_2 { get; set; }
            public int? ClientJobCostingAssignmentID_3 { get; set; }
            public int? ClientJobCostingAssignmentID_4 { get; set; }
            public int? ClientJobCostingAssignmentID_5 { get; set; }
            public int? ClientJobCostingAssignmentID_6 { get; set; }
        }

        public class TCAPagingInfo
        {
            public int EmployeeCount { get; set; }
            public int TotalPages { get; set; }
        }

        public class TCAEmployeeIDAndAccess
        {
            public int EmployeeID { get; set; }
            public bool AccessFlag { get; set; }
        }

        public IEnumerable<Employee> Employees { get; set; }

        public IEnumerable<Punch> Punches { get; set; }

        public TCAPagingInfo PagingInfo { get; set; }
    }
}
