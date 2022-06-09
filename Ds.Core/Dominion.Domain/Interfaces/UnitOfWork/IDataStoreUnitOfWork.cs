using System;
using System.Collections.Generic;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Utility.Containers;
using Dominion.Utility.Messaging;
using Dominion.Utility.OpResult;

namespace Dominion.Domain.Interfaces.UnitOfWork
{
    public interface IDataStoreUnitOfWork : IDisposable
    {
        /// <summary>
        /// Set a unique string that represents this UOW.
        /// </summary>
        string UniqueIdentifier { get; }

        /// <summary>
        /// Register an entity to the unit of work.
        /// </summary>
        /// <typeparam name="T">An entity type.</typeparam>
        /// <param name="obj">An entity</param>
        void Register<T>(T obj) where T : class, IEntity<T>;

        /// <summary>
        /// Unregister an entity to the unit of work.
        /// </summary>
        /// <typeparam name="T">An entity type.</typeparam>
        /// <param name="obj">An entity</param>
        void UnRegister<T>(T obj) where T : class, IEntity<T>;

        /// <summary>
        /// Register an entity to the unit of work new (insert).
        /// </summary>
        /// <typeparam name="T">An entity type.</typeparam>
        /// <param name="obj">An entity</param>        
        void RegisterNew<T>(T obj) where T : class, IEntity<T>;

        /// <summary>
        /// Register an entity to the unit of work modified (update).
        /// </summary>
        /// <typeparam name="T">An entity type.</typeparam>
        /// <param name="obj">An entity</param>
        void RegisterModified<T>(T obj, PropertyList<T> modifiedProperties = null) where T : class, IEntity<T>;

        /// <summary>
        /// Register an entity to the unit of work deleted (removeicated).
        /// </summary>
        /// <typeparam name="T">An entity type.</typeparam>
        /// <param name="obj">An entity</param>
        void RegisterDeleted<T>(T obj) where T : class, IEntity<T>;

        /// <summary>
        /// Allows the changes to be written to the DB.
        /// </summary>
        /// <param name="messages">Message list to add any commit messages to.</param>
        /// <returns>True if all was successful.</returns>
        bool Commit(IValidationStatusMessageList messages);


        void RegisterNewOrExisting<T>(T obj, Func<T, bool> isNewCheck) where T : class, IEntity<T>;
        void RegisterNewOrUpdate<T>(T obj, Func<T, bool> isNewCheck) where T : class, IEntity<T>;
        void RegisterNewOrModified<T>(T obj, Func<T, bool> isNewCheck) where T : class, IEntity<T>;


        /// <summary>
        /// Commit changes.
        /// </summary>
        /// <returns>An operation result object.</returns>
        IOpResult Commit();

        /// <summary>
        /// Sets the context auto detect changes enabled.
        /// </summary>
        /// <param name="value">Set the context auto detect changes enabled property.</param>
        /// 
        void AutoDetectChanges(bool value);

        /// <summary>
        /// Josh sql debugging magic to help Drew's dumbness
        /// </summary>
        /// <returns></returns>
        List<string> LogSql();

        /// <summary>
        /// Forces a RECOMPILE on the specified EF query.
        /// SHOULD ONLY BE USED WHEN DEBUGGING LOCALLY.  SHOULD NEVER BE LEFT
        /// ON IN PRODUCTION.
        /// See:https://stackoverflow.com/a/40387038
        /// </summary>
        /// <param name="query"></param>
        void RecompileSqlQuery(Action query);
    }
}