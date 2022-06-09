using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Labor
{
    public class EmployeeWithLeaveManagementGridDto
    {
        public int RequestTimeOffId { get; set; }
        public string Employee { get; set; }
        public int ClientAccrualId { get; set; }
        public int ClientEarningId { get; set; }
        public int EmployeeId { get; set; }
        public string PlanDescription { get; set; }
        public decimal? RequestBefore { get; set; }
        public decimal? RequestAfter { get; set; }
        public decimal? HoursInDay { get; set; }
        public int Units { get; set; }
        public DateTime? RequestFrom { get; set; }
        public DateTime Until { get; set; }
        public DateTime? DateRequested { get; set; }
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public decimal Hours { get; set; }
        public int? payrollId { get; set; }
        public DateTime PeriodEnded { get; set; }
        public int ModifiedBy { get; set; }
        public int? ClockEmployeeBenefitId { get; set; }
        public int LeaveManagementPendingAwardId { get; set; }
        public int PendingAwardType { get; set; }
        public int AwardOrder { get; set; }
    }

    public class EmployeeLeaveManagementInfoDto
    {
        public int RequestTimeOffId { get; set; }
        public string EmployeeName { get; set; }
        public int ClientAccrualId { get; set; }
        public int ClientEarningId { get; set; }
        public int EmployeeId { get; set; }
        public string PlanDescription { get; set; }
        public decimal? RequestHoursPrior { get; set; }
        public decimal? RequestHoursAfter { get; set; }
        public decimal? HoursInDay { get; set; }
        public int Units { get; set; }
        public DateTime? RequestFrom { get; set; }
        public DateTime? Until { get; set; }
        public DateTime? DateRequested { get; set; }
        public int Status { get; set; }
        public string StatusDescription { get; set; }
        public decimal Hours { get; set; }
        public int? PayrollId { get; set; }
        public DateTime PeriodEnded { get; set; }
        public int? ClockEmployeeBenefitId { get; set; }
        public int LeaveManagementPendingAwardId { get; set; }
        public int PendingAwardType { get; set; }
        public int AwardOrder { get; set; }
    }
}
