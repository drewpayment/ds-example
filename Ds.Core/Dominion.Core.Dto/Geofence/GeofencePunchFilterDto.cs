using System;

namespace Dominion.Core.Dto.Geofence
{
    public partial class GeofencePunchFilterDto
    {
        public int ClientID { get; set; } 
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? IsValid { get; set; }
        public int? EmployeeID { get; set; }
        public int? ClientCostCenterID { get; set; }
        public int? ClientEarningID { get; set; }
        public int? ClientShiftID { get; set; }
        public int? EmployeeClientRateID { get; set; }
        public int? ClientDepartmentID { get; set; }
        public int? ClientDivisionID { get; set; }
        public int? ClockEmployeeBenefitID { get; set; }
        public int? ClockClientHolidayDetailID { get; set; }

    }
}
