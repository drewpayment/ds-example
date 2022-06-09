using Dominion.Benefits.Dto.Employee;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Entity representation of the dbo.bpSalaryMethodType table.
    /// Holds the different types of methods used to calculate an employee's salary during enrollment.
    /// </summary>
    public class SalaryMethodTypeInfo : Entity<SalaryMethodTypeInfo>
    {
        public SalaryMethodType SalaryMethodTypeId { get; set; }
        public string           Description        { get; set; }
    }
}
