using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;

namespace Dominion.Domain.Entities.Employee
{
    public class EmployeeNoteTags : Entity<EmployeeNoteTags>
    { 
        public virtual int NoteTagID    { get; set; }
        public virtual int TagID        { get; set; }
        public virtual int RemarkId     { get; set; }
    }
}
