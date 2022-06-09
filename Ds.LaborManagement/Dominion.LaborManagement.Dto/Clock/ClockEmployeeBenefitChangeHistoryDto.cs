using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Clock
{
    public class ClockEmployeeBenefitChangeHistoryDto
    {
        public int ChangeId { get; set; }
        public int ClockEmployeeBenefitId { get; set; }
        public int EmployeeId { get; set; }
        public int ClientEarningId { get; set; }
        public double? Hours { get; set; }
        public DateTime? EventDate { get; set; }
        public int? ClientCostCenterId { get; set; }
        public int? ClientDivisionId { get; set; }
        public int? ClientDepartmentId { get; set; }
        public int? ClientShiftId { get; set; }
        public bool? IsApproved { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
        public byte? ChangeMode { get; set; }
        public string Comment { get; set; }
        public int? ClockClientHolidayDetailId { get; set; }
        public bool? IsWorkedHours { get; set; }
        public int? RequestTimeOffDetailId { get; set; }
        public int ClientId { get; set; }
        public string Subcheck { get; set; }
        public int? ClientJobCostingAssignmentId1 { get; set; }
        public int? ClientJobCostingAssignmentId2 { get; set; }
        public int? ClientJobCostingAssignmentId3 { get; set; }
        public int? ClientJobCostingAssignmentId4 { get; set; }
        public int? ClientJobCostingAssignmentId5 { get; set; }
        public int? ClientJobCostingAssignmentId6 { get; set; }
        public string EmployeeComment { get; set; }
        public int? EmployeeClientRateId { get; set; }
        public decimal? EmployeeBenefitPay { get; set; }
        public int? ApprovedBy { get; set; }
    }
}
