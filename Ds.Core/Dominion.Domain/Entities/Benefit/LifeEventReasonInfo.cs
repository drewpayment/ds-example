using Dominion.Benefits.Dto.Employee;
using Dominion.Benefits.Dto.Enrollment;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Benefit
{
    // Life Event
    public class LifeEventReasonInfo : Entity<LifeEventReasonInfo>
    {
        public virtual LifeEventReasonType LifeEventReasonType { get; set; }
        public virtual string Description { get; set; }
    }
}


