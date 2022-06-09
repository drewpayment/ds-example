using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Benefit;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Query.Benefits
{
    public interface ICustomBenefitFieldValueQuery : IQuery<CustomBenefitFieldValue, ICustomBenefitFieldValueQuery>
    {
        /// <summary>
        /// Filters by values of fields with the specified archive status.
        /// </summary>
        /// <param name="isArchived">If true, will only return archived field values. 
        /// If false, will only return non-archived field values.</param>
        /// <returns></returns>
        ICustomBenefitFieldValueQuery ByIsFieldArchived(bool isArchived);

        /// <summary>
        /// Filters by values for a single employee.
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        ICustomBenefitFieldValueQuery ByEmployeeId(int employeeId);

        /// <summary>
        /// Filters by values for a particular dependent OR if null will 
        /// return values not applied to a dependent (e.g. employee-only).
        /// </summary>
        /// <param name="dependentId">Dependent to filter by OR null to 
        /// return values not associated with an employee.</param>
        /// <returns></returns>
        ICustomBenefitFieldValueQuery ByDependentId(int? dependentId);

        /// <summary>
        /// Filters by values for a single client.
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns></returns>
        ICustomBenefitFieldValueQuery ByClientId(int clientId);

        /// <summary>
        /// Filters by values of fields that have NOT been archived.
        /// </summary>
        /// <returns></returns>
        ICustomBenefitFieldValueQuery ExcludeArchivedFields();

        /// <summary>
        /// Filters results by a single value record.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ICustomBenefitFieldValueQuery ByCustomFieldValueId(int id);
    }
}
