using System;
using System.Collections.Generic;

namespace Dominion.Core.Dto.Performance
{
    public class CompetencyEvaluationJsonData
    {
        public class CompetencyInfo
        {
            public string    Name              { get; set; }
            public string    Description       { get; set; }
            public int?      DifficultyLevel   { get; set; }
            public int?      GroupId           { get; set; }
            public string    GroupName         { get; set; }
            public bool      IsCore            { get; set; }
            public DateTime  Modified          { get; set; }
            public IEnumerable<int> RatingsThatRequireComment { get; set; }
        }

        public CompetencyInfo ComptencyInfo { get; set; }
 
    }
}