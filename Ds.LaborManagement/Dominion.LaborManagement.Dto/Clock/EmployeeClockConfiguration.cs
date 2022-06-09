using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.Clock
{
    public class EmployeeClockConfiguration
    {
        public int    ClientId           { get; set; }
        public int    EmployeeId         { get; set; }
        public string EmployeeNumber     { get; set; }
        public string FirstName          { get; set; }
        public string LastName           { get; set; }
        public string MiddleInitial      { get; set; }
        public int?   HomeDivisionId     { get; set; }
        public int?   HomeDepartmentId   { get; set; }
        public int?   HomeCostCenterId   { get; set; }
        public EmployeeClockSetupConfiguration ClockEmployee { get; set; }
    }
}