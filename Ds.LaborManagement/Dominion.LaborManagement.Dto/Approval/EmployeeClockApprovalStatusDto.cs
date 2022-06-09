using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.Payroll;

namespace Dominion.LaborManagement.Dto.Approval
{
    public class EmployeeClockApprovalStatusDto
    {
        public int              EmployeeId           { get; set; }
        public string           FirstName            { get; set; }
        public string           LastName             { get; set; }
        public string           MiddleInitial        { get; set; }
        public int?             ClientCostCenterId   { get; set; }
        public int?             ClientDepartmentId   { get; set; }
        public string           ClientDepartmentCode { get; set; }
        public PayFrequencyType PayFrequency         { get; set; }
        public PayType?         PayType              { get; set; }
        public DateTime         PeriodStart          { get; set; }
        public DateTime         PeriodEnd            { get; set; }
        public bool             IsApproved           { get; set; }
        public bool             HasSupervisor        { get; set; }
        public List<ClockApprovalDayDto> ApprovalDates      { get; set; }
    }
}