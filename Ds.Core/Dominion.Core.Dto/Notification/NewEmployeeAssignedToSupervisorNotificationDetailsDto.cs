﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Notification
{
    public class NewEmployeeAssignedToSupervisorNotificationDetailsDto
    {
        public int?   DirectSupervisorId { get; set; }
        public int    EmployeeId         { get; set; }
        public string FirstName          { get; set; }
        public string LastName           { get; set; }
    }
}
