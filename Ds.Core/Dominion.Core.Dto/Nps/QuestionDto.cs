using System;

namespace Dominion.Core.Dto.Nps
{
    public class QuestionDto
    {
        public int      QuestionId        { get; set; }
        public string   QuestionText      { get; set; }
        public string   LowestScoreLabel  { get; set; }
        public string   HighestScoreLabel { get; set; }
        public DateTime Modified          { get; set; }
        public int      ModifiedBy        { get; set; }
    }
}
