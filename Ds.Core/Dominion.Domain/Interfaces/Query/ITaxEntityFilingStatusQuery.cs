using Dominion.Core.Dto.Tax;
using Dominion.Domain.Entities.Tax;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Interfaces.Query
{
    public interface ITaxEntityFilingStatusQuery : IQuery<TaxEntityFilingStatus, ITaxEntityFilingStatusQuery>
    {
        ITaxEntityFilingStatusQuery ByIsEnabled(bool isEnabled = true);
        ITaxEntityFilingStatusQuery ByTaxTypeAndTaxId(LegacyTaxType taxType, int taxId);
        ITaxEntityFilingStatusQuery OrderByDisplayOrder();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="legacyTaxIdAndType"></param>
        /// <returns></returns>
        ITaxEntityFilingStatusQuery ByILegacyTaxIdAndType(ILegacyTaxIdAndType legacyTaxIdAndType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taxId"></param>
        /// <param name="taxTypeId"></param>
        /// <returns></returns>
        ITaxEntityFilingStatusQuery ByLegacyTaxIdAndType(int taxId, LegacyTaxType taxTypeId);
    }
}
