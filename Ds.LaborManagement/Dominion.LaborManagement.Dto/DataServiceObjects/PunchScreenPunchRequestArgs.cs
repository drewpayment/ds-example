using System;
using Dominion.LaborManagement.Dto.JobCosting;

namespace Dominion.LaborManagement.Dto.DataServiceObjects
{
    /// <summary>
    /// Args classed used for making punch requests using the 
    /// <see cref="IDsDataServicesClockService"/>
    /// </summary>
    public class PunchScreenPunchRequestArgs
    {
        public bool IsPaid { get; set; }
        public bool ShouldStopAutoLunch { get; set; }
        public int ClientCostCenterId { get; set; }
        public int ClientDepartmentId { get; set; }
        public int ClientDivisionId { get; set; }
        public int ClientShiftId { get; set; }
        public int ClockClientHardwareId { get; set; } = 0;
        public int ClockClientLunchId { get; set; }
        public int EmployeeId { get; set; }
        public int ModifiedBy { get; set; }
        public int TimeZoneId { get; set; }
        public int TransferOption { get; set; }
        public string Comment { get; set; }
        public string EmployeeComment { get; set; }
        public string ClockName { get; set; }
        public DateTime RawPunchTime { get; set; }
        public DateTime ModifiedPunch { get; set; }
        public DateTime ShiftDate { get; set; }
        public EmployeeJobCostingDto EmployeeJobCosting { get; set; }
        public int? PunchLocationID { get; set; }
        public int? ClientMachineId { get; set; }
    }
}