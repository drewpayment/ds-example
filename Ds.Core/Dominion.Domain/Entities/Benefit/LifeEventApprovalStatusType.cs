using Dominion.Benefits.Dto.Enrollment;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Benefit
{
    public class LifeEventApprovalStatusType : Entity<LifeEventApprovalStatusType>
    {
        public virtual LifeEventApprovalType ApprovalStatusType { get; set; }
        public virtual string Name { get; set; }
    }
}
