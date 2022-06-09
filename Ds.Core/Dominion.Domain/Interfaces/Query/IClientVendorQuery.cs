using System.Collections.Generic;
using Dominion.Domain.Entities.Payroll;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientVendorQuery : IQuery<ClientVendor,IClientVendorQuery>
    {
        IClientVendorQuery ByClientId(int clientId);
        IClientVendorQuery ByClientVendorId(int clientVendorId);
        IClientVendorQuery ByClientVendorIds(IEnumerable<int> clientVendorIds);
        IClientVendorQuery ByCompanyVendor();
    }
}