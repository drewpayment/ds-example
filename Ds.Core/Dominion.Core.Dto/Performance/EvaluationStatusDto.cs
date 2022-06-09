namespace Dominion.Core.Dto.Performance
{
    public class EvaluationStatusDto
    {
        public int EvaluationId { get; set; }
        public EvaluationStatus Status { get; set; }
        public EvaluationRoleType Role { get; set; }
    }
}