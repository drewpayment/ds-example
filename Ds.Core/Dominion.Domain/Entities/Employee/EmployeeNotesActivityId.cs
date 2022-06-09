using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Entities.Misc;

namespace Dominion.Domain.Entities.Employee
{
    public class EmployeeNotesActivityId : Entity<EmployeeNotesActivityId>
    {
        public virtual int ActivityId               { get; set; }
        public virtual int ActivityRemarkId         { get; set; }
        public virtual int EmployeeNoteRemarkId     { get; set; }

        public virtual Remark Remark { get; set; }
    }
}
