using Dominion.Core.Dto.Performance;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public partial class EvaluationRoleTypeInfo : Entity<EvaluationRoleTypeInfo>
    {
        public EvaluationRoleType EvaluationRoleTypeId { get; set; } 
        public string             Name                 { get; set; }
    }
}