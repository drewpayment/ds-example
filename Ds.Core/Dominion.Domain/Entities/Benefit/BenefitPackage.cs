using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Entity representation of a benefit package (see dbo.bpPackage table)
    /// </summary>
    public partial class BenefitPackage : Entity<BenefitPackage>, IHasModifiedData
    {
        public virtual int      BenefitPackageId { get; set; }
        public virtual int      ClientId         { get; set; }
        public virtual string   Name             { get; set; }
        public virtual int      ModifiedBy       { get; set; }
        public virtual DateTime Modified         { get; set; }

        public virtual Client Client { get; set; }
        
        public virtual ICollection<EmployeeBenefitInfo> EmployeeBenefitInformation { get; set; }
        public virtual ICollection<PlanPackage>         PlanPackages               { get; set; }
    }
}
