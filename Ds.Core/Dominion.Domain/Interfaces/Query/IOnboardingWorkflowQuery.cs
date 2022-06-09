using Dominion.Domain.Entities.Onboarding;
using Dominion.Utility.Query;


namespace Dominion.Domain.Interfaces.Query
{
    public interface IOnboardingWorkflowQuery : IQuery<OnboardingWorkflow, IOnboardingWorkflowQuery>
    {
        /// <summary>
        /// Filters employees by the specified ID.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOnboardingWorkflowQuery ByClientId(int clientId);

        //IOnboardingWorkflowQuery ByWorkflowId(int workflowId);
    }
}
