using System;

namespace Dominion.Core.Dto.Employee
{
    [Serializable]
    public partial class EmployeeSupervisor
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public bool EmailLeaveManagementRequests { get; set; }
        
    }
}
