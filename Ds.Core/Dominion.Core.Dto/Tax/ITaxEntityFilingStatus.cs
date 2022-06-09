using Dominion.Taxes.Dto.TaxOptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Tax
{
    public interface ITaxEntityFilingStatus
    {
        int TaxId { get; set; }
        LegacyTaxType TaxTypeId { get; set; }
        FilingStatus FilingStatusId { get; set; }
        bool IsEnabled { get; set; }
    }
}
