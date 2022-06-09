using Dominion.Core.Dto.Security;

namespace Dominion.Core.Dto.User
{
    public class UserSupervisorInfoDto
    {
        public int UserId { get; set; }
        public int? AuthUserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public int EmployeeId { get; set; }
        public int? ClientEmployeeId { get; set; }
        public int? ClientDivisionId { get; set; }
        public int? ClientDepartmentId { get; set; }
        public int? ClientCostCenterId { get; set; }

        public string EmailAddress { get; set; }
        public bool IsEmployeeSelfServiceViewOnly { get; set; }
        public bool IsEmailLeaveMgmtRequests { get; set; }
        public bool HasUserSecurityGroups { get; set; }
        public UserSecurityGroupType GroupType { get; set; }
        public bool IsActive { get; set; }
    }
}