using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.Sproc
{
    public class ApprovalDto
    {
        public String ApprovingUser { get; set; } //GetClockEmployeeApprovalData SPROC
        public Boolean? IsApproved { get; set; } //GetClockEmployeeApprovalData SPROC
        public int? EmployeeId { get; set; } //User table
        public string FirstName { get; set; } //User table
        public string LastName { get; set; } //User table
        public int? DirectSupervisorId { get; set; }//Employee table
        public int UserId { get; set; } //User table
    }
}
