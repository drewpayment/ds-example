using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Reporting
{
    public class ReportHolding : Entity<ReportHolding>
    {
        public string UniqueId { get; set; }
        public int ClientId { get; set; }
        public int EmployeeId { get; set; }
    }
}
