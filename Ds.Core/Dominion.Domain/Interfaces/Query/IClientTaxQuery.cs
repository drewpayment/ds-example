using Dominion.Domain.Entities.Tax;
using Dominion.Domain.Entities.Tax.Legacy;
using Dominion.Utility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Tax;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientTaxQuery  :IQuery<ClientTax, IClientTaxQuery>
    {
        IClientTaxQuery ByStateId(int? stateId);
        IClientTaxQuery ByClientId(int clientId);
        IClientTaxQuery ByStateTaxId(int stateTaxId);
        IClientTaxQuery ByClientTaxId(int clientTaxId);
        IClientTaxQuery GetClientTaxByStateTaxes();
        IClientTaxQuery GetClientTaxByStateIdClientIdAndTaxType(int stateId, int clientId, LegacyTaxType taxType);
        IClientTaxQuery GetClientTaxByLocalTaxId(int localTaxId);

        IClientTaxQuery LocalTaxByCountyId(int? countyId);
        IClientTaxQuery LocalTaxByTaxType( LegacyTaxType taxType);
        IClientTaxQuery GetClientTaxBySutaTax(bool includeObligation = true);
        IClientTaxQuery TaxIsActive(bool isActive = true);
        IClientTaxQuery GetClientTaxByOtherTaxByStateId(int stateId);
        IClientTaxQuery LocalTaxOnly();
        IClientTaxQuery StateTaxOnly();
        IClientTaxQuery State_DisabilityOnly();
        IClientTaxQuery GetClientTaxByDisabilityTaxByStateIdByAddWithState(int stateId);
        IClientTaxQuery GetClientTaxByDisabilityTaxByStateIdByAddWithSuta(int stateId);
        IClientTaxQuery ByIsUnemploymentTaxGroupCode(bool isUnemployment = true);
        IClientTaxQuery ByDisabilityTaxId(int disabilityTaxId);
    }
}
