using System;
using System.Collections.Generic;

namespace Dominion.Domain.Interfaces.UnitOfWork
{
    /// <summary>
    /// These are mapping objects that contain a Unit Of Work and what entity types they are responsible for.
    /// This object is held in an array by the IDataStoreUnitOfWorkProvider concrete instance.
    /// </summary>
    public interface IUnitOfWorkTypeMapping : IDisposable
    {
        /// <summary>
        /// The entity types you want the Unit of Work instance to handle.
        /// </summary>
        List<Type> EntityTypes { get; set; }

        /// <summary>
        /// Unit of Work instance.
        /// This will be injected with dependency injection.
        /// </summary>
        IDataStoreUnitOfWork UnitOfWork { get; set; }
    }
}