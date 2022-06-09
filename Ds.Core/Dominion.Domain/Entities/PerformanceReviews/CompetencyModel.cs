using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    /// <summary>
    /// A set of competencies that can be used to measure the performance of an employee.
    /// </summary>
    public partial class CompetencyModel : Entity<CompetencyModel>, IHasModifiedData
    {
        public int      CompetencyModelId { get; set; } 
        public int?     ClientId          { get; set; } 
        public string   Name              { get; set; } 
        public string   Description       { get; set; } 
        public DateTime Modified          { get; set; } 
        public int      ModifiedBy        { get; set; } 
        
        public virtual ICollection<Competency> Competencies { get; set; }
        public virtual ICollection<EmployeePerformanceConfiguration> Employees { get; set; }
        public virtual ICollection<JobProfile> JobProfiles { get; set; }
        public virtual ICollection<CompetencyModelGroup> CompetencyModelGroups { get; set; }
        public virtual Client Client { get; set; }
    }
}
