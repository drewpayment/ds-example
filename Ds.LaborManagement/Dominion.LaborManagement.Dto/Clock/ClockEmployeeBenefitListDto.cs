using System;
using Dominion.Core.Dto.Payroll;

namespace Dominion.LaborManagement.Dto.Clock
{
    /// <summary>
    /// Dto replaces the dataset returned from Sproc:  [dbo].[spGetClockEmployeeBenefitListByDate]
    /// See ClockEmployeeBenefitMaps.ToClockEmployeeBenefitListDto for the logic that creates the Dto
    /// </summary>
    public class ClockEmployeeBenefitListDto
    {
        public int? ClientCostCenterId { get; set; }
        public int? ClientDepartmentId { get; set; }
        public int? ClientDivisionId { get; set; }
        public int? ClientEarningId { get; set; }
        public int? ClientShiftId { get; set; }
        public int? ClockEmployeeBenefitId { get; set; }
        public DateTime? EventDate { get; set; }
        public int? EmployeeId { get; set; }
        public double? Hours { get; set; }
        public bool IsApproved { get; set; }
        public string Comment { get; set; }
        public ClientEarningCategory? ClientEarningCategoryId { get; set; }
        public bool? IsWorkedHours { get; set; }
        public string Description { get; set; }
        public int? IsHoliday { get; set; }
    }
}