using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Contact;
using Dominion.Core.Dto.Payroll;
using Dominion.Taxes.Dto;

namespace Dominion.Core.Dto.Employee
{
    public class EmployeeNoteAttachmentsDto
    {
        public int NoteAttachmentId { get; set; }
        public int AttachmentId { get; set; }
        public int EmployeeNoteRemarkId { get; set; }
        public int RemarkId { get; set; }
        public string FileName { get; set; }
        public string Change { get; set; }
    }
} 