using Dominion.Core.Dto.Employee;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Employee
{
    /// <summary>
    /// Entity representation of the dbo.EmployeeType table.
    /// </summary>
    public class EmployeePayTypeInfo : Entity<EmployeePayTypeInfo>
    {
        public PayType PayTypeId { get; set; }
        public string Description { get; set; }
    }
}
