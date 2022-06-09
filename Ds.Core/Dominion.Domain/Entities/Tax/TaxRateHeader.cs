using System;
using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Taxes.Dto.TaxOptions;
using Dominion.Taxes.Dto.TaxRates;

namespace Dominion.Domain.Entities.Tax
{
    [Serializable]
    public class TaxRateHeader : Entity<TaxRateHeader>, IHasModifiedData, ITaxRateHeaderKeyDto
    {
        public virtual int                  TaxRateHeaderId       { get; set; }
        public virtual int                  TaxId                 { get; set; }
        public virtual Tax                  Tax                   { get; set; }
        public virtual DateTime             EffectiveDate         { get; set; }
        public virtual TaxRateType          TaxRateType           { get; set; }
        public virtual TaxRateTypeInfo      TaxRateTypeInfo       { get; set; }
        public virtual FilingStatus?        FilingStatus          { get; set; }
        public virtual FilingStatusInfo     FilingStatusInfo      { get; set; }
        public virtual ICollection<TaxRate> TaxRates              { get; set; }
        public virtual int                  ModifiedBy            { get; set; }
        public virtual DateTime             Modified              { get; set; }
    }
}
