using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Contact;
using Dominion.Core.Dto.Core;
using Dominion.Core.Dto.Payroll;
using Dominion.Taxes.Dto;
using Dominion.Utility.OpResult;

namespace Dominion.Core.Dto.Employee
{
    /// <summary>
    /// Represents a DTO that contains basic information about an employee note.
    /// </summary>
    /// <remarks>
    /// This DTO contains mostly a PK/FK RemarkID attached to core.Remark. FK to NoteSource table
    /// Stores information that is not fitting in the Remark table
    /// </remarks>
    public class EmployeeNotesDto
    {
        public int RemarkId { get; set; }
        public int EmployeeId { get; set; }
        public int NoteSourceId { get; set; }
        public int? ReviewId { get; set;}
        public int SecurityLevel { get; set; }
        public String Description { get; set; }
        public int AddedBy { get; set; }
        public string AddedByFirstName { get; set; }
        public string AddedByLastName { get; set; }
        public DateTime AddedDate { get; set; }
        public String NoteSource { get; set; }
		public bool IsArchived { get; set; }
		public String AttachmentIds { get; set; }
        public bool EmployeeViewable { get; set; }
        public bool DirectSupervisorViewable { get; set; }
        public String UsersViewable { get; set; }                             
        public string FirstName      { get; set; }
        public string LastName       { get; set; }

		public IOpResult<IEnumerable<EmployeeAttachmentFolderDetailDto>> EmployeeAttachmentFolderDetail { get; set; }
		public IEnumerable<EmployeeNotesActivityIdDto> Activity { get; set; }
        public IEnumerable<EmployeeNoteAttachmentsDto> Attachments { get; set; }
		public IEnumerable<EmployeeNoteTagsDto> Tags { get; set; }
        public IEnumerable<ReviewRemarkDto> Reviews { get; set; }
    }
}