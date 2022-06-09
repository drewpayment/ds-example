using System;

namespace Dominion.Core.Dto.Performance
{
    public class EvaluationSetupDto
    {
        public EvaluationRoleType Role                      { get; set; }
        public int?               EvaluatedByUserId         { get; set; }
        public DateTime?          StartDate                 { get; set; }
        public DateTime           DueDate                   { get; set; }
        public bool?              IsViewableByEmployee      { get; set; }
        public bool               AllowGoalWeightAssignment { get; set; }
    }
}