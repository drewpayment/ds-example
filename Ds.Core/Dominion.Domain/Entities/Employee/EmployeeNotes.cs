using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Entities.Misc;
using System;

namespace Dominion.Domain.Entities.Employee
{
    public class EmployeeNotes : Entity<EmployeeNotes>
    {
        public virtual int RemarkId         { get; set; }
        public virtual int EmployeeId       { get; set; }
        public virtual int NoteSourceId     { get; set; }
        public virtual int SecurityLevel    { get; set; }
        public virtual bool EmployeeViewable { get; set; }
        public virtual bool DirectSupervisorViewable { get; set; }
        public virtual String UsersViewable { get; set; }


        public virtual Remark                  Remark       { get; set; }
        public virtual Employee                Employee     { get; set; }
        public virtual NoteSourceEntity        NoteSource   { get; set; }
    }
}
