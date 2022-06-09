using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Onboarding;
using Dominion.Utility.Query;


namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeeOnboardingWorkflowQuery : IQuery<EmployeeOnboardingWorkflow, IEmployeeOnboardingWorkflowQuery> 
    {
        /// <summary>
        /// Filters employees by the specified ID.
        /// </summary>
        /// <param name="employeeId">ID of employee.</param>
        /// <returns></returns>
        IEmployeeOnboardingWorkflowQuery ByEmployeeId(int employeeId);
        IEmployeeOnboardingWorkflowQuery ByEmployeeId(IList<int> employeeIds);
        IEmployeeOnboardingWorkflowQuery ByTaskId(int taskId);
        IEmployeeOnboardingWorkflowQuery ByMainTaskId(int taskId);
        IEmployeeOnboardingWorkflowQuery OrderBySequence();
        IEmployeeOnboardingWorkflowQuery ByTasksId(List<int> tasksId);
        IEmployeeOnboardingWorkflowQuery ByClientId(int clientId);
        IEmployeeOnboardingWorkflowQuery ByUserMustUpload(bool userMustUpload);
        IEmployeeOnboardingWorkflowQuery ByIsDeleted(bool isDeleted);
    }
}
