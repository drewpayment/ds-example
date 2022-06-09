using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Employee
{
    public class ActiveEmployeeDto : EmployeeHireDto
    {
        public int? ClientCostCenterId { get; set; }
        public int? ClientDepartmentId { get; set; }
        public int? ClientDivisionId { get; set; }
        public int? ClientId { get; set; }
        public string EmployeeNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        public string ClientDepartmentName { get; set; } 
        public string ClientCostCenterName { get; set; }
        public string EmployeeStatus { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string UserName { get; set; }
        public string UserTypeId { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? Modified { get; set; }
    }
}
