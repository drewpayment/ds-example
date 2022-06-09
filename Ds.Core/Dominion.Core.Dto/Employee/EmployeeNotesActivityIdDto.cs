using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Contact;
using Dominion.Core.Dto.Payroll;
using Dominion.Taxes.Dto;

namespace Dominion.Core.Dto.Employee
{
    public class EmployeeNotesActivityIdDto
    {
        public int ActivityRemarkId         { get; set; }
        public int EmployeeNoteRemarkId    { get; set; }

        public String FirstName { get; set; }
        public String LastName { get; set; }

        public string ActivityDesc { get; set; }
        public DateTime ActivityDate { get; set; }
    }
} 