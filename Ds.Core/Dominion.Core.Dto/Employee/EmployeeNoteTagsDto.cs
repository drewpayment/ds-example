using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Contact;
using Dominion.Core.Dto.Payroll;
using Dominion.Taxes.Dto;

namespace Dominion.Core.Dto.Employee
{
    public class EmployeeNoteTagsDto
    {
        public int? NoteTagID { get; set; }
        public int TagID { get; set; }
        public int ClientID { get; set; }
        public string TagName { get; set; }
        public int RemarkId { get; set; }
        public string Change { get; set; } // 1 char 'r' or 'a' for remove and add
    }
}