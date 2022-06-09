using System.Collections.Generic;
using Dominion.Core.Dto.Tax;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Tax;
using Dominion.Utility.Query;
using Dominion.Domain.Entities.Tax.Legacy;

namespace Dominion.Domain.Interfaces.Query
{
    /// <summary>
    /// Provides a way to query <see cref="EmployeePay"/> entities.
    /// </summary>
    public interface IEmployeeTaxQuery : IQuery<EmployeeTax, IEmployeeTaxQuery>
    {
        /// <summary>
        /// Filters change history by employee id.
        /// </summary>
        /// <param name="active">Defaults to true; send in false if you prefer otherwise.</param>
        /// <returns></returns>
        IEmployeeTaxQuery ByIsActive(bool active = true);

        IEmployeeTaxQuery IsEmployeeConfigurable();

        /// <summary>
        /// Filters employees by employee id.
        /// </summary>
        /// <param name="employeeId">ID of the employee to filter by.</param>
        /// <returns></returns>
        IEmployeeTaxQuery ByEmployeeId(int employeeId);

        /// <summary>
        /// Filters change history by employee ids.
        /// </summary>
        /// <param name="employeeIds">IDs of the employees to filter by.</param>
        /// <returns></returns>
        IEmployeeTaxQuery ByEmployeeIds(IEnumerable<int> employeeIds);

        /// <summary>
        /// Orders the query results by <see cref="EmployeeTax.EmployeeId"/>.
        /// </summary>
        /// <param name="direction">Defaults to ascending.</param>
        /// <returns></returns>
        IEmployeeTaxQuery OrderByEmployeeTaxId(SortDirection direction = SortDirection.Ascending);

        IEmployeeTaxQuery ByClientTaxId(int? clientTaxId);

        IEmployeeTaxQuery ByTaxtype(LegacyTaxType taxType);

        IEmployeeTaxQuery ByEmployeeTaxId(int employeeTaxId);
        IEmployeeTaxQuery ByTaxtypes(IEnumerable<LegacyTaxType> taxTypes);
        IEmployeeTaxQuery ByIsFederalTax(bool isFederal = true);
    }
}