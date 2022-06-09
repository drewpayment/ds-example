using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientHRInfoQuery : IQuery<ClientHRInfo, IClientHRInfoQuery>
    {
        IClientHRInfoQuery ByClientId(int clientId);
        IClientHRInfoQuery ByClientIds(IEnumerable<int> clientIds);
        IClientHRInfoQuery ByFieldNames(IEnumerable<string> fieldNames);
        IClientHRInfoQuery ByFieldNamesCaseInsensitive(IEnumerable<string> fieldNames);
        IClientHRInfoQuery ByEmployeeEditableOnly(bool employeeEditableOnly);
    }
}
