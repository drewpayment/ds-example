using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class ListByDate
    {
        public class PunchRequest
        {
            public int EmployeeID { get; set; }

            public string DateOfPunch { get; set; }

            public string EmployeeComment { get; set; }

            public DateTime ModifiedPunch { get; set; }

            public int ClockEmployeePunchRequestID { get; set; }

            public int? ClockClientLunchID { get; set; }

            public int? TimeZoneID { get; set; }

            public string CostCenterDesc { get; set; }

            public string EmployeeName { get; set; }

            public string EmployeeNumber { get; set; }
        }

        public IEnumerable<PunchRequest> Requests { get; set; }

        public class Earning
        {
            public double Hours { get; set; }

            public string Description { get; set; }

            public DateTime EventDate { get; set; }

            public byte ClientEarningCategoryID { get; set; }

            public bool IsBenefit { get; set; }

            public bool IsWorkedHours { get; set; }

            public string Comment { get; set; }
            public string EmployeeComment { get; set; }
            public int? ClientCostCenterID { get; set; }
            public string Department { get; set; }
            public string CostCenter { get; set; }
            public string Division { get; set; }
            public string ShiftDescription { get; set; }
            public int EmployeeID { get; set; }
            public int UniqueID { get; set; }

            /// <summary>
            ///             ''' The cost center description that should be used for job costing info.
            ///             ''' </summary>
            ///             ''' <returns></returns>
            public string CostCenterDescription { get; set; }

            /// <summary>
            ///             ''' The department description that should be used for job costing info.
            ///             ''' </summary>
            ///             ''' <returns></returns>
            public string DepartmentDescription { get; set; }
            /// <summary>
            ///             ''' The division description that should be used for job costing info.
            ///             ''' </summary>
            ///             ''' <returns></returns>
            public string DivisionDescription { get; set; }

            public int? ClientJobCostingAssignmentID_1 { get; set; }
            public int? ClientJobCostingAssignmentID_2 { get; set; }
            public int? ClientJobCostingAssignmentID_3 { get; set; }
            public int? ClientJobCostingAssignmentID_4 { get; set; }
            public int? ClientJobCostingAssignmentID_5 { get; set; }
            public int? ClientJobCostingAssignmentID_6 { get; set; }
            public virtual double? EmployeeRate { get; set; }
        }

        public IEnumerable<Earning> Earnings { get; set; }
    }
}
