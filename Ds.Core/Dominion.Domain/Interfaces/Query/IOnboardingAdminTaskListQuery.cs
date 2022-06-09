using Dominion.Domain.Entities.Onboarding;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IOnboardingAdminTaskListQuery : IQuery<OnboardingAdminTaskList, IOnboardingAdminTaskListQuery>
    {
        IOnboardingAdminTaskListQuery ByClientId(int clientId);
        IOnboardingAdminTaskListQuery ByClientIds(List<int> clientIds);
        IOnboardingAdminTaskListQuery ByTaskListId(int taskListId);
        IOnboardingAdminTaskListQuery OrderByName();
    }
}
