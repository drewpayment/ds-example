using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public class CompetencyOptions : Entity<CompetencyOptions>
    {
        public int OptionId { get; set; }
        public bool IsDisabled { get; set; }
        /// <summary>
        /// Determines whether the required comments feature is enabled as a whole.  This overrides
        /// the item(s) listed in <see cref="CompetencyRateCommentRequired"/>
        /// </summary>
        public bool EnforceRequiredComments { get; set; }

        public virtual ICollection<CompetencyRateCommentRequired> CompetencyRateCommentRequired { get; set; }
    }
}
