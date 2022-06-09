using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;

namespace Dominion.Domain.Entities.Employee
{
    public class EmployeeNoteTagsDetails : Entity<EmployeeNoteTagsDetails>
    { 
        public virtual int TagID { get; set; }
        public virtual int ClientID { get; set; }
        public virtual string TagName { get; set; }
    }
}
