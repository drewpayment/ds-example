using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public class ScoreRangeLimit : Entity<ScoreRangeLimit>, IHasModifiedData
    {
        public int ScoreModelRangeId { get; set; }
        public int ScoreModelId { get; set; }
        public string Label { get; set; }
        public string Description { get; set; }
        public decimal? MaxScore { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public ScoreModel ScoreModel { get; set; }
        public User.User ModifiedByUser { get; set; }
        public decimal? MeritPercent { get; set; }
    }
}
