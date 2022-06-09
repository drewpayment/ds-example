using System.Collections.Generic;
using Dominion.Domain.Entities.Forms;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Forms
{
    /// <summary>
    /// Query on <see cref="Form"/>(s).
    /// </summary>
    public interface IFormQuery : IQuery<Form, IFormQuery>
    {
        /// <summary>
        /// Filters by a single form.
        /// </summary>
        /// <param name="formId">ID of form to get.</param>
        /// <returns></returns>
        IFormQuery ByFormId(int formId);

        /// <summary>
        /// Filters forms belonging to a particular employee.
        /// </summary>
        /// <param name="employeeId">ID of employee.</param>
        /// <returns></returns>
        IFormQuery ByEmployeeId(int employeeId);

        /// <summary>
        /// Filters forms belonging to the specified employees.
        /// </summary>
        /// <param name="employeeIds"></param>
        /// <returns></returns>
        IFormQuery ByEmployeeIds(IEnumerable<int> employeeIds);

        /// <summary>
        /// Finds form resources by client id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        IFormQuery ByClientId(int clientId);

        /// <summary>
        /// Filters forms by those created during the employee-onboarding process.
        /// </summary>
        /// <param name="isOnboarding">If true (default), will only include forms results that have been created
        /// during onboarding.</param>
        /// <returns></returns>
        IFormQuery ByIsOnboardingForm(bool isOnboarding = true);
    }
}
