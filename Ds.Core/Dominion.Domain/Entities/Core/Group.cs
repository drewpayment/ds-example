using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.Core
{
    public class Group : Entity<Group>, IHasModifiedData
    {
        public int GroupId { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string JsonData { get; set; }
        public virtual ICollection<CompetencyModelGroup> CompetencyModelGroups { get; set;}
        public virtual ICollection<ReviewTemplateGroup> ReviewTemplateGroups { get; set; }
        public virtual Client Client { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime Modified { get; set; }
    }
}
