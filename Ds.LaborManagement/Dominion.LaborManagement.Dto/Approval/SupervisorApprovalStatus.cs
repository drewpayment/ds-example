using System.Collections.Generic;
using Dominion.Core.Dto.User;

namespace Dominion.LaborManagement.Dto.Approval
{
    public class SupervisorApprovalStatus
    {
        public int?   UserId        { get; set; }
        public int?   EmployeeId    { get; set; }
        public string FirstName     { get; set; }
        public string LastName      { get; set; }
        public string MiddleInitial { get; set; }
        public bool   IsApproved    { get; set; }
        public UserViewEmployeePayType EmployeePayTypeAccess { get; set; }
        public IEnumerable<int> EmulatedSupervisorIds { get; set; }
        public ICollection<EmployeeClockApprovalStatusDto> EmployeeApprovals { get; set; }
    }
}
