using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Labor;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IClientAccrualQuery : IQuery<ClientAccrual, IClientAccrualQuery>
    {
        IClientAccrualQuery ByClientId(int clientId);
        IClientAccrualQuery ByDescription(string description);
        IClientAccrualQuery ExcludeCustomAccrual();
        IClientAccrualQuery IsActive();
        IClientAccrualQuery ByLeaveManagment(bool lm);
        IClientAccrualQuery IsDisplayFourDecimals(int clientAccrualId);
        IClientAccrualQuery ByIsActive(bool isActive);
        IClientAccrualQuery ByClientAccrualId(int clientAccrualId);
        IClientAccrualQuery ByClientEarningId(int clientEarningId);
        IClientAccrualQuery ByClientAccrualIds(IEnumerable<int> clientAccrualIds);
    }
}
