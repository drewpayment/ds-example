using System;
using System.Collections.Generic;

using Dominion.Benefits.Dto.Plans;
using Dominion.Benefits.Dto.Validation;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Benefit
{
    /// <summary>
    /// Benefit Plan Provider Entity. Maps to the [dbo].[bpPlanProvider] table.
    /// </summary>
    public class PlanProvider : Entity<PlanProvider>, IHasModifiedData, IValidatablePlanProvider
    {
        public virtual int      PlanProviderId      { get; set; }
        public virtual int      ClientId            { get; set; }
        public virtual string   Name                { get; set; }
        public virtual string   AddressLine1        { get; set; }
        public virtual string   AddressLine2        { get; set; }
        public virtual string   City                { get; set; }
        public virtual int?     StateId             { get; set; }
        public virtual string   PostalCode          { get; set; }
        public virtual string   PhoneNumber         { get; set; }
        public virtual string   Fax                 { get; set; }
        public virtual DateTime Modified            { get; set; }
        public virtual int      ModifiedBy          { get; set; }
        public virtual bool     IsActive            { get; set; }
        public virtual bool     IsCarrierConnection { get; set; }

        public virtual State State { get; set; }

        public virtual ICollection<Plan> Plans { get; set; }
    }
}