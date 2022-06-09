using Dominion.Core.Dto.Employee;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Employee
{
    public class EmployeeStatus : Entity<EmployeeStatus>
    {
        public virtual EmployeeStatusType EmployeeStatusId { get; set; }
        public virtual string Description { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string Code { get; set; }
    }
}