using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Labor;

namespace Dominion.LaborManagement.Dto.Scheduling
{
    [Serializable]
    public partial class ClockEmployeePunchDto : IHasPunchShiftDateInfo
    {
        public int ClockEmployeePunchId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
        public DateTime ModifiedPunch { get; set; }
        public DateTime RawPunch { get; set; }
        public int? ClientCostCenterId { get; set; }
        public int? ClientDivisionId { get; set; }
        public int? ClientDepartmentId { get; set; }
        public int? ClientShiftId { get; set; }
        public int RawPunchBy { get; set; }
        public int? ClockEmployeePunchTypeId { get; set; }
        public int? ClockClientLunchId { get; set; }
        public bool IsPaid { get; set; }
        public string Comment { get; set; }
        public DateTime? ShiftDate { get; set; }
        public bool? IsManualShiftOverride { get; set; }
        public byte? TimeZoneId { get; set; }
        public string ClockName { get; set; }
        public byte? TransferOption { get; set; }
        public bool IsStopAutoLunch { get; set; }
        public int ClientId { get; set; }
        public int? ClockEmployeeScheduleId { get; set; }
        public byte? ScheduleNumber { get; set; }
        public int? ClientJobCostingAssignmentId1 { get; set; }
        public int? ClientJobCostingAssignmentId2 { get; set; }
        public int? ClientJobCostingAssignmentId3 { get; set; }
        public int? ClientJobCostingAssignmentId4 { get; set; }
        public int? ClientJobCostingAssignmentId5 { get; set; }
        public int? ClientJobCostingAssignmentId6 { get; set; }
        public string EmployeeComment { get; set; }
        public int? ClockEmployeePunchLocationId { get; set; }
        public decimal? ClockEmployeePunchLocationLat { get; set; }
        public decimal? ClockEmployeePunchLocationLng { get; set; }

    }
}
