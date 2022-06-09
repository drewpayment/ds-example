using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.PerformanceReviews
{
    public partial class EmployeePerformanceConfiguration : Entity<EmployeePerformanceConfiguration>, IHasModifiedData
    {
        public int      EmployeeId        { get; set; } 
        public int?     CompetencyModelId { get; set; } 
        public DateTime Modified          { get; set; } 
        public int      ModifiedBy        { get; set; } 

        public virtual CompetencyModel CompetencyModel { get; set; } 
        public virtual Employee.Employee Employee { get; set; }
    }
}