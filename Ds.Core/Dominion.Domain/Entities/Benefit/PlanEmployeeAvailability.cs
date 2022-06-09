using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Entity link between a benefit plan and an employee. Maps to [dbo].[bpPlanEmployeeAvailability] table.
    /// </summary>
    public class PlanEmployeeAvailability : Entity<PlanEmployeeAvailability>
    {
        public virtual int PlanId { get; set; }
        public virtual int EmployeeId { get; set; }

        public virtual Plan Plan { get; set; }
        public virtual Employee.Employee Employee { get; set; }
    }
}