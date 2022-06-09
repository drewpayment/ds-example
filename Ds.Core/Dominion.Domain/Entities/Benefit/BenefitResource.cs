using System;
using System.Collections.Generic;

using Dominion.Benefits.Dto.Resources;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Entity representation of a benefit reasource file/URL.  Maps to [dbo].[bpReasource] table.
    /// </summary>
    public class BenefitResource : Entity<BenefitResource>, IHasModifiedData
    {
        public virtual int          ResourceId       { get; set; }
        public virtual int          ClientId         { get; set; }
        public virtual ResourceType ResourceTypeId   { get; set; }
        public virtual bool         IsAFile          { get; set; }
        public virtual string       ResourceLocation { get; set; }
        public virtual string       Name             { get; set; }
        public virtual DateTime     Modified         { get; set; }
        public virtual int          ModifiedBy       { get; set; }

        public virtual ICollection<Plan> Plans { get; set; }
    }
}