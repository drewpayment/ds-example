using System;

namespace Dominion.Utility.EntityDataStore.EntityMetadata.Interfaces
{
    /// <summary>
    /// Data store metadata for an entities properties.
    /// </summary>
    public interface IEntityDataStoreFieldMetadata
    {
        #region Variables and Properties

        /// <summary>
        /// True if the field is the primary key.
        /// </summary>
        bool IsPrimaryKey { get; }

        /// <summary>
        /// If the data store was a database, this would be the column name.
        /// </summary>
        string DataStoreFieldName { get; }

        /// <summary>
        /// The C# Entity object's name.
        /// </summary>
        string PocoName { get; }

        /// <summary>
        /// If there is a max value for this field.
        /// </summary>
        int MaxLength { get; }

        /// <summary>
        /// If the data store is a database this would be the precision of the column.
        /// </summary>
        int Precision { get; }

        /// <summary>
        /// If the data store is a database this would be the scale of the column.
        /// </summary>
        int Scale { get; }

        /// <summary>
        /// is the field required?.
        /// </summary>
        bool IsRequired { get; }

        /// <summary>
        /// Does this field represent a currency value.
        /// </summary>
        bool IsCurrency { get; }

        /// <summary>
        /// Specifies a custom function to be used during property comparison.
        /// </summary>
        Func<object, object, bool> CustomComparator { get; } 

        #endregion

        #region Methods

        /// <summary>
        /// Gets the value from the entity that this metadata represents.
        /// </summary>
        /// <param name="obj">The entity.</param>
        /// <returns>The object value.</returns>
        object GetValue(object obj);

        /// <summary>
        /// Gets the c# data type for the property.
        /// </summary>
        /// <returns></returns>
        Type GetPropertyType();

        /// <summary>
        /// True if the c# datatype is System.Nullable.
        /// </summary>
        /// <returns></returns>
        bool IsPropertyNullable();

        #endregion
    }
}