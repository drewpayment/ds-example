using System.Collections;
using System.Collections.Generic;

using Dominion.Aca.Dto.Forms;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Onboarding;
using Dominion.Domain.Interfaces.Repositories;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeeOnboardingTasksQuery : IQuery<EmployeeOnboardingTasks, IEmployeeOnboardingTasksQuery>
    {
        /// <summary>
        /// Filters employees by the specified ID.
        /// </summary>
        /// <param name="employeeId">ID of employee.</param>
        /// <returns></returns>
        IEmployeeOnboardingTasksQuery ByEmployeeId(int employeeId);

        /// <summary>
        /// returns all of the Employee Onboarding records for a given client
        /// </summary>
        /// <param name="isCompleted">id of the client</param>
        /// <returns></returns>
        IEmployeeOnboardingTasksQuery ByIsCompleted(bool isCompleted);
        /// <summary>
        /// Returns all of the tasks that are parents (Not dependent on another task)
        /// </summary>
        /// <returns></returns>
        IEmployeeOnboardingTasksQuery IsParentTask();
        /// <summary>
        /// Gets list of tasks by the parent task
        /// </summary>
        /// <param name="parentTaskId"></param>
        /// <returns></returns>
        IEmployeeOnboardingTasksQuery ByParentTaskId(int parentTaskId);

       
    }
}
