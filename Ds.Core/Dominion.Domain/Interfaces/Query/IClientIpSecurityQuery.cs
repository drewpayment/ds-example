using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Clients;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientIpSecurityQuery : IQuery<ClientIpSecurity, IClientIpSecurityQuery>
    {
        IClientIpSecurityQuery ByClientId(int clientId);
        IClientIpSecurityQuery ByEmployeeId(int employeeId);
    }
}
