using Dominion.Benefits.Dto.Plans;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Benefit
{
    public partial class BenefitAmountSelectionTypeInfo : Entity<BenefitAmountSelectionTypeInfo>
    {
        public virtual BenefitAmountSelectionType BenefitAmountSelectionTypeId { get; set; }
        public virtual string                     Name                         { get; set; }
        public virtual bool                       IsDerivedFromSalary          { get; set; }
    }
}
