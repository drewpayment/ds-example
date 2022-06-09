using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Onboarding;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IOnboardingMenuClientQuery : IQuery<EmployeeOnboardingMenuClient, IOnboardingMenuClientQuery>
    {
        IOnboardingMenuClientQuery ByClient(int clientId);
    }
}
