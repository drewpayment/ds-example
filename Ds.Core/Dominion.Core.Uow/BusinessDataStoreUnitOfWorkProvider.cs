using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Diagnostics;
using System.Linq;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Domain.Interfaces.UnitOfWork;
using Dominion.Utility.Constants;
using Ninject;

namespace Dominion.Core.Uow
{
    /// <summary>
    /// An object that contains references to all of the unit of work objects.
    /// The unit of work objects are associated with entity types.
    /// </summary>
    public class BusinessDataStoreUnitOfWorkProvider : IDataStoreUnitOfWorkProvider
    {
        #region Variables and Properties

        /// <summary>
        /// The unit of work entity mappings.
        /// Used to get correlating unit of work for a specific entity type.
        /// </summary>
        protected IUnitOfWorkTypeMapping[] _mappings;

        #endregion

        #region Constructors and Initializers

        /// <summary>
        /// Builds the provider.
        /// </summary>
        /// <param name="maps">The Unit of work entity mappings.</param>
        [Inject]
        public BusinessDataStoreUnitOfWorkProvider(params IUnitOfWorkTypeMapping[] maps)
        {
            _mappings = maps;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get all of the registered units of work.
        /// </summary>
        /// <returns>
        /// List of all of the registered units of work.
        /// </returns>
        public IEnumerable<IDataStoreUnitOfWork> GetUnitsOfWork()
        {
            var qry = _mappings.Select(u => u.UnitOfWork);
            return qry;
        }

        /// <summary>
        /// Find a registered unit of work based on the entity type passed in.
        /// </summary>
        /// <param name="entityType">An entity class type.</param>
        /// <returns>
        /// A unit of work for the specified entity type.
        /// </returns>
        /// <exception cref="System.Exception">
        /// Data Store Unit of Work Mapping: No uow found for current entity type:  + 
        ///                             entityType.FullName + .
        /// or
        /// Data Store Unit of Work Mapping: More than one Uow found for entity type:  + 
        ///                             entityType.FullName + .
        /// or
        /// Data Store Unit of Work Mapping: No mappings defined.
        /// </exception>
        public IDataStoreUnitOfWork GetUnitOfWork<TEntity>(IEntity<TEntity> entity)
            where TEntity : class, IEntity<TEntity>
        {
            if (_mappings.Count() > 0)
            {
                Type entityType = ObjectContext.GetObjectType(entity.GetType());

                if (_mappings.Count() == 1)
                {
                    // only one Unit of Work, no need to check type just return the one and only uow
                    return _mappings[0].UnitOfWork;
                }
                else
                {
                    // find the Unit of Work based on type
                    var qry = _mappings
                        .Where(m => m.EntityTypes.Contains(entityType))
                        .Select(m => m.UnitOfWork);

                    if (!qry.Any())
                    {
                        throw new NoUnitOfWorkMappedToEntityException(entityType);


// throw new Exception(
                        // NoUnitOfWorkMappedToEntityException.NO_MAPPING_FOUND_BASE_MESSAGE +
                        // entityType.FullName + ".");
                    }

                    if (qry.Count() > 1)
                    {
                        throw new MultipleUnitOfWorksMappedToEntityException(entityType);
                    }

                    return qry.First();
                }
            }
            else
            {
                // throw new Exception("Data Store Unit of Work Mapping: No mappings defined.");
                throw new NoUnitOfWorkMappingsException();
            }
        }

        /// <summary>
        /// Releases Resources
        /// </summary>
        public void Dispose()
        {
            Debug.WriteLine("BusinessDataStoreUnitOfWorkProvider.Dispose()");
            _mappings = null;
        }

        #endregion
    }

    /// <summary>
    /// Expcetion to throw if there are more than one unit of work's mapped to one entity.
    /// </summary>
    public class MultipleUnitOfWorksMappedToEntityException : Exception
    {
        private const string MORE_THAN_ONE_UNIT_OF_WORK_FOUND_FOR_ENTITY_BASE_MESSAGE =
            "Data Store Unit of Work Mapping: More than one Unit of Work found for entity type: ";

        public static string GetExceptionMessage(Type entityType)
        {
            return MORE_THAN_ONE_UNIT_OF_WORK_FOUND_FOR_ENTITY_BASE_MESSAGE +
                   entityType.FullName + CommonConstants.PERIOD;
        }

        public MultipleUnitOfWorksMappedToEntityException(Type entityType) : base(GetExceptionMessage(entityType))
        {
        }
    }

    /// <summary>
    /// Expcetion to throw if there are no unit of works mapped to the entity type.
    /// </summary>
    public class NoUnitOfWorkMappedToEntityException : Exception
    {
        private const string NO_MAPPING_FOUND_BASE_MESSAGE =
            "Data Store Unit of Work Mapping: No Unit of Work found for current entity type: ";

        public static string GetExceptionMessage(Type entityType)
        {
            return NO_MAPPING_FOUND_BASE_MESSAGE +
                   entityType.FullName + CommonConstants.PERIOD;
        }

        public NoUnitOfWorkMappedToEntityException(Type entityType) : base(GetExceptionMessage(entityType))
        {
        }
    }

    /// <summary>
    /// Exception to throw when a unit of work isn't found for an entity.
    /// </summary>
    public class NoUnitOfWorkMappingsException : Exception
    {
        public const string NO_MAPPINGS_DEFINED = "Data Store Unit of Work Mapping: No mappings defined.";

        public NoUnitOfWorkMappingsException() : base(NO_MAPPINGS_DEFINED)
        {
        }
    }
}