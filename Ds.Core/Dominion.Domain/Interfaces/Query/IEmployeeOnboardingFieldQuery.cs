using Dominion.Domain.Entities.Onboarding;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeeOnboardingFieldQuery : IQuery<EmployeeOnboardingField, IEmployeeOnboardingFieldQuery>
    {
        IEmployeeOnboardingFieldQuery ByEmployeeId(int employeeId);

        IEmployeeOnboardingFieldQuery ByOnboardingFieldId(int onboardingFieldId);

        //IEmployeeOnboardingFieldQuery ByEmployeeOnboardingWorkflowTask(int employeeOnboardingWorkflowTask);
    }
}
