using System.Collections.Generic;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Interfaces.UnitOfWork
{
    /// <summary>
    /// An object that contains references to all of the unit of work objects.
    /// The unit of work objects are associated with entity types.
    /// </summary>
    public interface IDataStoreUnitOfWorkProvider
    {
        /// <summary>
        /// Find a registered unit of work based on the entity type passed in.
        /// </summary>
        /// <param name="entityType">An entity class type.</param>
        /// <returns>A unit of work for the specified entity.</returns>
        IDataStoreUnitOfWork GetUnitOfWork<TEntity>(IEntity<TEntity> entity)
            where TEntity : class, IEntity<TEntity>;

        /// <summary>
        /// Get all of the registered units of work.
        /// </summary>
        /// <returns>List of all of the registered units of work.</returns>
        IEnumerable<IDataStoreUnitOfWork> GetUnitsOfWork();
    }
}