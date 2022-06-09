using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dominion.Core.Dto.Tax;
using Dominion.Domain.Entities.Tax;
using Dominion.Utility.Query;

namespace Dominion.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Interface to a repository used to manipulate various Employee Tax  related Entities in a data store.
    /// </summary>
    public interface ITaxRepository : IRepository, IDisposable
    {
        #region MISC

        /// <summary>
        /// Returns the information for the filing statuses that meet the specified query.
        /// </summary>
        /// <typeparam name="TDest">Type of object to return. Typically, a DTO representation of the entity.</typeparam>
        /// <param name="qb">Query representing the filing statuses to retrieve.</param>
        /// <returns></returns>
        IEnumerable<TDest> GetFilingStatusDetails<TDest>(QueryBuilder<FilingStatusInfo, TDest> qb) where TDest : class;

        #endregion

        #region EMPLOYEE TAXES

        /// <summary>
        /// Gets the specified employee tax.
        /// </summary>
        /// <param name="employeeTaxId">ID of the employee tax to retrieve.</param>
        /// <returns>An employee tax entity with the specified ID.</returns>
        EmployeeTax GetEmployeeTax(int employeeTaxId);

        /// <summary>
        /// Gets the specified employee tax.
        /// </summary>
        /// <typeparam name="TResult">Type of object the resulting entity will be translated into.</typeparam>
        /// <param name="employeeTaxId">ID of the employee tax to retrieve.</param>
        /// <param name="selector">Expression describing how to translate the tax entity into the desired type.</param>
        /// <returns>An employee tax entity with the specified ID.</returns>
        TResult GetEmployeeTax<TResult>(int employeeTaxId, Expression<Func<EmployeeTax, TResult>> selector)
            where TResult : class;

        /// <summary>
        /// Returns the Employee Taxes for the given employee that satisfy the provided base query.
        /// </summary>
        /// <typeparam name="TResult">A strongly-typed object containing the desired Employee Tax properties to be returned.</typeparam>
        /// <param name="employeeId">The ID of the employee to get taxs for.</param>
        /// <param name="query">The query parameters to apply to the employee tax result set including the object type to translate the results to.</param>
        /// <returns></returns>
        IEnumerable<TResult> GetEmployeeTaxes<TResult>(int employeeId, QueryBuilder<EmployeeTax, TResult> query)
            where TResult : class;

        /// <summary>
        /// Returns the Employee Tax that satisfies the provided base query.
        /// </summary>
        /// <typeparam name="TResult">A strongly-typed object containing the desired Employee Tax properties to be returned.</typeparam>
        /// <param name="query">The query parameters to apply to the employee tax result set including the object type to translate the results to.</param>
        /// <returns></returns>
        IEnumerable<TResult> GetEmployeeTaxes<TResult>(QueryBuilder<EmployeeTax, TResult> query) where TResult : class;

        IEnumerable<FilingStatusDto> GetMaritalStatuses(int employeeTaxId); 

        #endregion
    }
}