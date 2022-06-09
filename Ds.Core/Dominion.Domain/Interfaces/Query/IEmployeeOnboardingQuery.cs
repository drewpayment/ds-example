using System;
using System.Collections;
using System.Collections.Generic;

using Dominion.Aca.Dto.Forms;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Onboarding;
using Dominion.Domain.Interfaces.Repositories;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query
{
    public interface IEmployeeOnboardingQuery : IQuery<EmployeeOnboarding, IEmployeeOnboardingQuery>
    {
        /// <summary>
        /// Filters employees by the specified ID.
        /// </summary>
        /// <param name="employeeId">ID of employee.</param>
        /// <returns></returns>
        IEmployeeOnboardingQuery ByEmployeeId(int employeeId);
        IEmployeeOnboardingQuery ByEmployeeIds(List<int> employeeIds);
        /// <summary>
        /// returns all of the Employee Onboarding records for a given client
        /// </summary>
        /// <param name="clientId">id of the client</param>
        /// <returns></returns>
        IEmployeeOnboardingQuery ByClientId(int clientId);
        IEmployeeOnboardingQuery ByClientIds(List<int> clientIds);

        /// <summary>
        /// Filters by the employee being active in onboarding
        /// (i.e. not complete ... no Onboarding End date)
        /// </summary>
        /// <returns></returns>
        IEmployeeOnboardingQuery ByIsActiveInOnboarding();
        /// <summary>
        /// Filters by the employees who have completed onboarding
        /// (i.e. have an Onboarding End date)
        /// </summary>
        /// <returns></returns>
        IEmployeeOnboardingQuery ByIsOnboardingComplete();
        IEmployeeOnboardingQuery ByHireDateRange(DateTime startDate, DateTime endDate);
        IEmployeeOnboardingQuery OrderByEmployeeName();
    }
}
