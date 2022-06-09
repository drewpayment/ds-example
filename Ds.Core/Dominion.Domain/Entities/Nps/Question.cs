using Dominion.Domain.Entities.Base;
using System;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Nps
{
    public class Question : Entity<Question>, IHasModifiedData
    {
        public int      QuestionId        { get; set; }
        public string   QuestionText   { get; set; }
        public string   LowestScoreLabel  { get; set; }
        public string   HighestScoreLabel { get; set; }
        public DateTime Modified          { get; set; }
        public int      ModifiedBy        { get; set; }
    }
}
