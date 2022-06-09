using System;
using System.Linq.Expressions;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Interfaces.Repositories
{
    public interface IRepository
    {
        /// <summary>
        /// Performs a query to ensure that the specified employee is the owner of the entity being updated.
        /// </summary>
        /// <typeparam name="TEntity">An entity.</typeparam>
        /// <param name="employeeId">Pass in the currently logged in employee id.</param>
        /// <param name="criteria">The criteria needed to track down the entity's id.</param>
        /// <returns>True if the employee id on the entity in the db matches the one passed in.</returns>
        bool VerifyEmployeeIsOwner<TEntity>(
            int employeeId, 
            Expression<Func<TEntity, bool>> criteria)
            where TEntity : class, IEntity<TEntity>, IEmployeeOwnedEntity<TEntity>;


        bool VerifyIsOwner<TEntity>(
            Expression<Func<TEntity, bool>> ownershipCriteria)
            where TEntity : class, IEntity<TEntity>;
    }
}