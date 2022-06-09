using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Contact;
using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Payroll;
using Dominion.Taxes.Dto;
using Dominion.Utility.OpResult;

namespace Dominion.Core.Dto.Employee
{
    public class EmployeePointsDto
    {
        public int EmployeePointsId { get; set; }
        public int EmployeeId { get; set; }
        public int? EmployeePointsCodeId { get; set; }
        public double Hours { get; set; }
        public double Amount { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string Comments { get; set; }
        public DateTime DateOccured { get; set; }
        public int ClientId { get; set; }
        public int? ClockEmployeePointHistoryAdjustmentId { get; set; }
        public bool IsAutoPoint { get; set; }
        public int? ClockClientPointExceptionId { get; set; }
        public int? ClockEmployeeExceptionHistoryId { get; set; }
        public int? ClockClientPerfectAttendanceId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? ClientDepartmentId { get; set; }
        public string Department { get; set; }
        public string Supervisor { get; set; }

        public EmployeeFullDto Employee { get; set; }
        public ClientDto Client { get; set; }
    }
}