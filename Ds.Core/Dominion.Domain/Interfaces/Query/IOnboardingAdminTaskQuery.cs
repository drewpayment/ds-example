using Dominion.Domain.Entities.Onboarding;
using Dominion.Utility.Query;


namespace Dominion.Domain.Interfaces.Query
{
    public interface IOnboardingAdminTaskQuery : IQuery<OnboardingAdminTask, IOnboardingAdminTaskQuery>
    {
        IOnboardingAdminTaskQuery ByTaskId(int taskId);
        IOnboardingAdminTaskQuery ByTaskListId(int taskListId);
    }
}
