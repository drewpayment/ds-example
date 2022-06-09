using System;
using Dominion.Core.Dto.Tax;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Taxes.Dto.TaxOptions;

namespace Dominion.Domain.Entities.Tax
{
    public class TaxEntityFilingStatus : Entity<TaxEntityFilingStatus>, IHasModifiedData, ITaxEntityFilingStatus
    {
        public virtual int TaxId { get; set; }
        public virtual LegacyTaxType TaxTypeId { get; set; }
        public virtual FilingStatus FilingStatusId { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }

        public virtual FilingStatusInfo FilingStatus { get; set; }
    }
}
