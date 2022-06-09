using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    /// <summary>
    /// Holds the settings/options for scoring for a particular client.  These
    /// determine how scores are displayed, calculated, and in what category
    /// <see cref="Evaluation"/> score results fall (see <see cref="ScoreRangeLimits"/>)
    /// </summary>
    public class ScoreModel : Entity<ScoreModel>, IHasModifiedData
    {
        public int ScoreModelId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public byte ScoreMethodTypeId { get; set; }
        public bool IsScoreEmployeeViewable { get; set; }
        public bool HasScoreRange { get; set; }
        public int? SubTotalEvaluationGroupId { get; set; }
        public bool IsActive { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
        public ScoreMethodType ScoreMethodType { get; set; }
        public virtual ICollection<ScoreRangeLimit> ScoreRangeLimits { get; set; }
        public virtual EvaluationGroup SubTotalEvaluationGroup { get; set; }
        public virtual User.User ModifiedByUser { get; set; }
        public virtual Client Client { get; set; }
        /// <summary>
        /// Determines whether users of the client can set up additional payments for 
        /// employees.  This feature is independent of payroll requests.
        /// </summary>
        public bool AdditionalEarnings { get; set; }
        public bool EvaluationGroupsAreCustom { get; set; }
    }
}
