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
    public class PerformanceCompetency : Entity<PerformanceCompetency>, IHasModifiedData
    {
        public int PerformanceCompetencyId { get; set; }
        public int? ClientId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? CompetencyGroupId { get; set; }
        public bool Inactive { get; set; }
        public bool IsCore { get; set; }
        public int? DifficultyLevel { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }

        // RELATIONSHIPS

        public virtual Client Client { get; set; }
        public virtual CompetencyGroup CompetencyGroup { get; set; }
    }
}
