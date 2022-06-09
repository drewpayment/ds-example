using System;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Tax
{
    [Serializable]
    public class TaxRate : Entity<TaxRate>, IHasModifiedData
    {
        public virtual int       TaxRateId        { get; set; } //because ef needs a pk
        public virtual int       TaxRateHeaderId  { get; set; }
        public virtual decimal?  Amount           { get; set; }
        public virtual decimal?  Rate             { get; set; }
        public virtual decimal?  Limit            { get; set; }
        public virtual bool      IsActive         { get; set; }
        public virtual int       ModifiedBy       { get; set; }
        public virtual DateTime  Modified         { get; set; }

        public virtual TaxRateHeader TaxRateHeader { get; set; }
    }
}
