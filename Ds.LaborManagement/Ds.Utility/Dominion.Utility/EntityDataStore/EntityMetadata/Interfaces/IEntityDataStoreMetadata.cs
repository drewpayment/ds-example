using System.Collections.Generic;

namespace Dominion.Utility.EntityDataStore.EntityMetadata.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEntityDataStoreMetadata
    {
        /// <summary>
        /// If the data store was a database, this would be the table name.
        /// </summary>
        string DataStoreName { get; }

        /// <summary>
        /// If the data store was a database, this would a list of metadata object for all the columns.
        /// </summary>
        IEnumerable<IEntityDataStoreFieldMetadata> FieldMetaData { get; }
    }
}