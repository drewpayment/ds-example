using System;

namespace Dominion.Core.Dto.Performance
{
    public class GoalEvaluationJsonData
    {
        public class GoalInfoData
        {
            public string    Title          { get; set; }
            public string    Description    { get; set; }
            public DateTime? StartDate      { get; set; }
            public DateTime? DueDate        { get; set; }
            public DateTime? CompletionDate { get; set; }
            public decimal   Progress       { get; set; }
        }

        public GoalInfoData GoalInfo { get; set; }
    }
}