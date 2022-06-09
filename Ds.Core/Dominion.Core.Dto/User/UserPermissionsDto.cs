using System;
using System.Collections.Generic;
using System.Text;

namespace Dominion.Core.Dto.User
{
    public class UserPermissionsDto
    {
        public int UserId { get; set; }
        public bool IsEmployeeNavigatorAdmin { get; set; }
    }
}