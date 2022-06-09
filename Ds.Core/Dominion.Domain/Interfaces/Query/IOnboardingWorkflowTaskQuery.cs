using Dominion.Domain.Entities.Onboarding;
using Dominion.Utility.Query;


namespace Dominion.Domain.Interfaces.Query
{
    public interface IOnboardingWorkflowTaskQuery : IQuery<OnboardingWorkflowTask, IOnboardingWorkflowTaskQuery>
    {
        /// <summary>
        /// Filters tasks for the specified client ID or for null.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IOnboardingWorkflowTaskQuery ByClientId(int clientId);
        IOnboardingWorkflowTaskQuery ByWorkflowTaskClientId(int clientId);
        IOnboardingWorkflowTaskQuery ByTaskId(int taskId);
        IOnboardingWorkflowTaskQuery ByMainTaskId(int mainTaskId);
        IOnboardingWorkflowTaskQuery ByIsDeleted(bool isDeleted);
        /// <summary>
        /// orders the tasks by their sequence
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        IOnboardingWorkflowTaskQuery OrderBySequence();
    }
}
