using System;

namespace Dominion.Core.Dto.Labor
{
    public class ClockEmployeeExceptionHistoryDto
    {
        public int ClockEmployeeExceptionHistoryId { get; set; }
        public int? EmployeeId { get; set; }
        public ClockExceptionType? ClockExceptionTypeId { get; set; }
        public double? Hours { get; set; }
        public DateTime? EventDate { get; set; }
        public int? ClockClientExceptionDetailId { get; set; }
        public int? ClockEmployeePunchId { get; set; }
        public int? ClockClientLunchId { get; set; }
        public int ClientId { get; set; }
        public int? ClockEmployeeBenefitId { get; set; }
        public ClockExceptionTypeInfoDto ExceptionTypeInfo { get; set; }
        public ClockClientExceptionDetailDto ExceptionRuleDetail { get; set; }
    }
}
