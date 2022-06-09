using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.TimeCard
{
    public class Employee
    {
        public string EmployeeName { get; set; }
        public int EmployeeID { get; set; }
        public string EmployeeNumber { get; set; }
        public bool EmployeeActivity { get; set; }
        public DateTime EmployeeHireDate { get; set; }
        public DateTime? EmployeeSeparationDate { get; set; }
        public DateTime? EmployeeRehireDate { get; set; }
        public string ClockClientTimePolicyName { get; set; }
        public int? ClockClientTimePolicyID { get; set; }
        public bool? HasGeofencing { get; set; }
        public Boolean? AccessFlag { get; set; }
        public override bool Equals(object other)
        {
            return other != null && other.GetType() == typeof(Employee) && ((Employee)other).EmployeeID == EmployeeID;
        }

        public override int GetHashCode()
        {
            return EmployeeID.GetHashCode();
        }
    }
}
