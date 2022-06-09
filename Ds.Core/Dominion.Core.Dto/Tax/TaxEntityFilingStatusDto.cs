using Dominion.Taxes.Dto.TaxOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Tax
{
    public class TaxEntityFilingStatusDto : ITaxEntityFilingStatus
    {
        public virtual int TaxId { get; set; }
        public virtual LegacyTaxType TaxTypeId { get; set; }
        public virtual FilingStatus FilingStatusId { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }
    }
}
