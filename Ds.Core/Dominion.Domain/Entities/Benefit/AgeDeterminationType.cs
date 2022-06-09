using Dominion.Benefits.Dto.Plans;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Benefit
{
    public partial class AgeDeterminationType : Entity<AgeDeterminationType>
    {
        public virtual AgeDetermination AgeDeterminationTypeId { get; set; }
        public virtual string           Name                   { get; set; }
    }
}
