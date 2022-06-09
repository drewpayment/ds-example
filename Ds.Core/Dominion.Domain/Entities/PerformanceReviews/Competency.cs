using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    /// <summary>
    /// A skill or area of knowledge to measure for an employee
    /// </summary>
    public class Competency : Entity<Competency>, IHasModifiedData
    {
        public int CompetencyId { get; set; }
        public int? ClientId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CompetencyGroupId { get; set; }
        public bool IsArchived { get; set; }
        /// <summary>
        /// Determines whether this should be included in all competency models
        /// </summary>
        public bool IsCore { get; set; }
        /// <summary>
        /// NOT USED
        /// </summary>
        public int? DifficultyLevel { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }

        // RELATIONSHIPS

        public virtual Client Client { get; set; }
        public virtual CompetencyGroup CompetencyGroup { get; set; }
        public virtual ICollection<CompetencyModel> CompetencyModels { get; set; }
        public virtual ICollection<CompetencyEvaluation> CompetencyEvaluations { get; set; }
    }
}
