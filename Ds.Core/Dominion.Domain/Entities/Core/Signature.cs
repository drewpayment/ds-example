using System;
using System.Collections.Generic;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.PerformanceReviews;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Core
{
    public class Signature : Entity<Signature>, IHasModifiedData
    {
        public virtual int      SignatureId     { get; set; }
        public virtual DateTime SignatureDate   { get; set; }
        public virtual string   SignatureText   { get; set; }
        public virtual string   SigneeFirstName { get; set; }
        public virtual string   SigneeLastName  { get; set; }
        public virtual string   SigneeMiddle    { get; set; }
        public virtual string   SigneeInitials  { get; set; }
        public virtual string   SigneeTitle     { get; set; }
        public virtual int      ModifiedBy      { get; set; }
        public virtual DateTime Modified        { get; set; }

        public virtual ICollection<Evaluation> Evaluations { get; set; }
    }
}