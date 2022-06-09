using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Employee;
using Dominion.Core.Dto.User;
using System;

namespace Dominion.Core.Dto.Contact.Search
{
    public class ContactSearchDto
    {
        public int?   UserId         { get; set; }
        public int?   EmployeeId     { get; set; }
        public string FirstName      { get; set; }
        public string LastName       { get; set; }
        public DateTime? BirthDate { get; set; }
        public string SupervisorName { get; set; }
        public string EmployeeNumber { get; set; }
        public UserType? UserType { get; set; }
        public EmployeeProfileImageDto ProfileImage { get; set; }
        public virtual EmployeeAvatarDto EmployeeAvatar { get; set; }
    }
}