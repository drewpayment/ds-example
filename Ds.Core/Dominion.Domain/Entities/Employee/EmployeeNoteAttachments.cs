using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Entities.Misc;

namespace Dominion.Domain.Entities.Employee
{
    public class EmployeeNoteAttachments : Entity<EmployeeNoteAttachments>
    {
        public virtual int NoteAttachmentId     { get; set; }
        public virtual int AttachmentId         { get; set; }
        public virtual int EmployeeNoteRemarkId { get; set; }

        public virtual Remark Remark            { get; set; }
    }
}
