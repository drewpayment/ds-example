using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Employee
{
    public class ClockEmployeeLostPunch : Entity<ClockEmployeeLostPunch>
    {
        public int ClockEmployeeLostPunchId { get; set; }
        public int? ClientId { get; set; }
        public string CompanyIdentifier { get; set; }
        public string ClockName { get; set; }
        public int? EmployeeId { get; set; }
        public string BadgeNumber { get; set; }
        public int? ClientCostCenterId { get; set; }
        public string CostCenterCode { get; set; }
        public DateTime RawPunch { get; set; }
        public DateTime Modified { get; set; }
    }
}