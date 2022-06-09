using System;
using Dominion.Domain.Entities.User;

namespace Dominion.Domain.Interfaces.Entities
{
    [Obsolete("Use IHasModifiedData or similar instead.")]
    public interface IModifiableEntity<TEntity> : IEntity<TEntity> where TEntity : class
    {
        DateTime LastModifiedDate          { get; }
        string   LastModifiedByDescription { get; }
        int      LastModifiedByUserId      { get; }
        User     LastModifiedByUser        { get; }

        void SetLastModifiedValues(
            int      lastModifiedByUserId,
            string   lastModifiedByUserName,
            DateTime lastModifiedDate);
    }

}