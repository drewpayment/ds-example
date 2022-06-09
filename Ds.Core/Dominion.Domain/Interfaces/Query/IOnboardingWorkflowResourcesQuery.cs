using Dominion.Domain.Entities.Onboarding;
using Dominion.Utility.Query;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IOnboardingWorkflowResourcesQuery : IQuery<OnboardingWorkflowResources, IOnboardingWorkflowResourcesQuery>
    {
        IOnboardingWorkflowResourcesQuery ByTaskId(int taskId);

        IOnboardingWorkflowResourcesQuery ByResourceId(int resourceId);

        IOnboardingWorkflowResourcesQuery ByResourceIds(IEnumerable<int> resourceIds);

        IOnboardingWorkflowResourcesQuery ByIsDeleted(bool isDeleted);
    }
}
