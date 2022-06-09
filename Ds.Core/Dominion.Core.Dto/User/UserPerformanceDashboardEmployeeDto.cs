using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.User
{
    public class UserPerformanceDashboardEmployeeDto
    {
        public int UserId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string Username { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool IsActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public UserType UserTypeId { get; set; }
        public int? SupervisorId { get; set; }
        public string Supervisor { get; set; }
        public string Department { get; set; }
        public DateTime? HireDate { get; set; }
        public string EmployeeStatus { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? TempEnableFromDate { get; set; }
        public DateTime? TempEnableToDate { get; set; }
        public string ViewEmployeePayTypes { get; set; }
        public string ViewEmployeeRateTypes { get; set; }
        public string AssignedEmployee { get; set; }
        public string EmailAddress { get; set; }
        public bool CertifyI9 { get; set; }
        public bool AddEmployee { get; set; }
        public bool ResetPassword { get; set; }
        public bool ApproveTimeCards { get; set; }
        public string IsSecurityEnabled { get; set; }
        public bool IsPasswordEnabled { get; set; }
        public string ClientDepartmentName { get; set; }
        public int? ClientDepartmentId { get; set; }
    }
}
