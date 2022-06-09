using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Employee
{
    public partial class EmployeeCostCenterPercentage : Entity<EmployeeCostCenterPercentage>, IHasModifiedOptionalData
    {
        public virtual int       EmployeeCostCenterPercentageId { get; set; } 
        public virtual int       ClientCostCenterId             { get; set; } 
        public virtual int       EmployeeId                     { get; set; } 
        public virtual double    Percent                        { get; set; } 
        public virtual DateTime? Modified                       { get; set; } 
        public virtual int?      ModifiedBy                     { get; set; } 
        public virtual int       ClientId                       { get; set; } 

        public virtual Employee Employee { get; set; }
    }
}
