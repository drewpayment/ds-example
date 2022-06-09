﻿﻿using System;
﻿using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Employee;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.User
{
    public class UserInfoWithNamesDto
    {
        public int UserId { get; set; }
        public int? AuthUserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public int? EmployeeId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public UserType UserTypeId { get; set; }
        public int? LastEmployeeId { get; set; }
        public string LastEmployeeFirstName { get; set; }
        public string LastEmployeeMiddleInitial { get; set; }
        public string LastEmployeeLastName { get; set; }

        public int? LastClientId { get; set; }
        public string LastClientName { get; set; }
        public string LastClientCode { get; set; }

        public string EmailAddress { get; set; }

        public int? TimeoutMinutes { get; set; }

        public bool? CertifyI9 { get; set; }
        public bool? AddEmployee { get; set; }
        public int? ClientDepartmentId { get; set; }
    }
}
