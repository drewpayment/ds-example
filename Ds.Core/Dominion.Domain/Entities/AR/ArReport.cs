using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.AR
{
    public class ArReport : Entity<ArReport>
    {
        public virtual int    ArReportId { get; set; }
        public virtual string Name       { get; set; }
        public virtual string SqlName    { get; set; }
    }
}
