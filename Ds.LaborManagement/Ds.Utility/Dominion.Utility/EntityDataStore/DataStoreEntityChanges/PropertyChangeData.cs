using System;

namespace Dominion.Utility.EntityDataStore.DataStoreEntityChanges
{
    /// <summary>
    /// Contains data about property changes.
    /// </summary>
    public class PropertyChangeData
    {
        /// <summary>
        /// If the data store was a database, this would be the table name.
        /// </summary>
        public string DataStoreName { get; set; }

        /// <summary>
        /// If the datastore was a database, this would be the column name.
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// The C# Entity object's name.
        /// </summary>
        public string PocoName { get; set; }

        /// <summary>
        /// The original value before the property change.
        /// </summary>
        public object OldValue { get; set; }

        /// <summary>
        /// The proposed value for the property change.
        /// </summary>
        public object NewValue { get; set; }

        /// <summary>
        /// The c# data type's name.
        /// </summary>
        public string DataTypeName { get; set; }

        /// <summary>
        /// The c# type object that represents the data type.
        /// </summary>
        public Type DataType { get; set; }

        /// <summary>
        /// Tells us if the data type is nullable.
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Tells us if the data type represents a currency value.
        /// Used for formatting in the legacy system.
        /// </summary>
        public bool IsCurrency { get; set; }
    }
}