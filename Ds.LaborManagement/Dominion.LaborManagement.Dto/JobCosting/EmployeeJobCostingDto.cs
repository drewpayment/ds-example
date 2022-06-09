using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.JobCosting
{
    /// <summary>
    /// Dto that represents an EmployeeJobCosting object as found in DataServices
    /// </summary>
    public class EmployeeJobCostingDto
    {
        public int? ClientJobCostingAssignmentId1 { get; set; }
        public int? ClientJobCostingAssignmentId2 { get; set; }
        public int? ClientJobCostingAssignmentId3 { get; set; }
        public int? ClientJobCostingAssignmentId4 { get; set; }
        public int? ClientJobCostingAssignmentId5 { get; set; }
        public int? ClientJobCostingAssignmentId6 { get; set; }
        public int ClientJobCostingId1 { get; set; }
        public int ClientJobCostingId2 { get; set; }
        public int ClientJobCostingId3 { get; set; }
        public int ClientJobCostingId4 { get; set; }
        public int ClientJobCostingId5 { get; set; }
        public int ClientJobCostingId6 { get; set; }
        public int ClientCostCenterId { get; set; }
        public int ClientDepartmentId { get; set; }
        public int ClientDivisionId { get; set; }
        public int ClientGroupId { get; set; }
        public bool UsesCostCenter { get; set; } = false;
        public bool UsesDepartment { get; set; } = false;
        public bool UsesDivision { get; set; } = false;
        public bool UsesGroup { get; set; } = false;
        public bool UsesCustom { get; set; } = false;
    }
}