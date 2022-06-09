using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public class GoalOptions : Entity<GoalOptions>
    {
        public int OptionId { get; set; }
        /// <summary>
        /// Determines whether the user can set up goal weights when filling out an evaluation.
        /// </summary>
        public bool IsWeighted { get; set; }
        public bool IsDisabled { get; set; }
        /// <summary>
        /// Determines whether the required comments feature is enabled as a whole.  This overrides
        /// the item(s) listed in <see cref="GoalRateCommentRequired"/>
        /// </summary>
        public bool EnforceRequiredComments { get; set; }

        public virtual ICollection<GoalRateCommentRequired> GoalRateCommentRequired { get; set; }
    }
}
