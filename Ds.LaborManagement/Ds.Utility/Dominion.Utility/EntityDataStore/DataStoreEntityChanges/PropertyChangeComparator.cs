using System.Collections.Generic;
using System.Linq;
using Dominion.Utility.EntityDataStore.EntityMetadata.Interfaces;
using Dominion.Utility.ExtensionMethods;

namespace Dominion.Utility.EntityDataStore.DataStoreEntityChanges
{
    /// <summary>
    /// This will find the differences between two entities of the same type.
    /// </summary>
    /// <typeparam name="T">The entity type.</typeparam>
    public class PropertyChangeComparator<T> : IPropertyChangeComparator<T> where T : class
    {
        #region Variables and Properties

        /// <summary>
        /// The current entity object.
        /// </summary>
        public T CurrentEntity { get; private set; }

        /// <summary>
        /// The proposed entity object.
        /// </summary>
        public T ProposedEntity { get; private set; }

        /// <summary>
        /// The metadata for the entire entity.
        /// </summary>
        private IEntityDataStoreMetadata _entityMetadata;

        /// <summary>
        /// If you want to ignore some properties.
        /// </summary>
        public List<string> DataStoreFieldNamesToIgnore { get; set; }

        /// <summary>
        /// Allows per-field <see cref="IPropertyComparer"/>s to be defined. If a comparer does not exist for a given
        /// field, the <see cref="PropertyComparers.DefaultComparer"/> will be used.
        /// </summary>
        public IDictionary<string, IPropertyComparer> FieldComparisonOverrides { get; set; }

        #endregion

        #region Constructors and Initializers

        /// <summary>
        /// Finds changes when comparing two like objects.
        /// 
        /// </summary>
        /// <param name="currentEntity">
        /// The existing entity (if applicable). 
        /// If adding there will not be an existing entity.
        /// Pass in null if no existing entity. eg: proposed entity is new.
        /// </param>
        /// <param name="proposedEntity">The entity with your desired changes.</param>
        /// <param name="metaData">Metadata for the entire entity.</param>
        public PropertyChangeComparator(
            T currentEntity, 
            T proposedEntity, 
            IEntityDataStoreMetadata metaData)
        {
            DataStoreFieldNamesToIgnore = new List<string>();
            FieldComparisonOverrides = new Dictionary<string, IPropertyComparer>();
            CurrentEntity = currentEntity;
            ProposedEntity = proposedEntity;
            _entityMetadata = metaData;
        }

        #endregion

        #region Methods

        /// <summary>
        /// This will add a property change object to the list of changes.
        /// </summary>
        /// <param name="fieldMetaData">Metadata about the field/property.</param>
        /// <param name="currentPropertyValue">The current property value.</param>
        /// <param name="proposedPropertyValue">The proposed property value.</param>
        private PropertyChangeData CreatePropertyChange(
            IEntityDataStoreFieldMetadata fieldMetaData, 
            object currentPropertyValue, 
            object proposedPropertyValue)
        {
            PropertyChangeData pc = new PropertyChangeData();
            pc.DataStoreName = _entityMetadata.DataStoreName;
            pc.FieldName = fieldMetaData.DataStoreFieldName;
            pc.PocoName = fieldMetaData.PocoName;
            pc.OldValue = currentPropertyValue.ToStringWithNullCheck();
            pc.NewValue = proposedPropertyValue.ToStringWithNullCheck();
            pc.DataTypeName = fieldMetaData.GetPropertyType().GetTypeName();
            pc.DataType = fieldMetaData.GetPropertyType();
            pc.IsNullable = fieldMetaData.IsPropertyNullable();
            pc.IsCurrency = fieldMetaData.IsCurrency;

            return pc;
        }

        /// <summary>
        /// This is what compares the entities and determines what has changed.
        /// It will populate this objects 'Changes' property containing the results.
        /// </summary>
        /// <returns>List of detected changes.</returns>
        public IEnumerable<PropertyChangeData> FindChanges()
        {
            List<PropertyChangeData> detectedChanges = new List<PropertyChangeData>();

            var fieldNamesToCompare = _entityMetadata.FieldMetaData
                .Where(x => !DataStoreFieldNamesToIgnore.Contains(x.DataStoreFieldName));

            foreach (var md in fieldNamesToCompare)
            {
                object current =
                    (CurrentEntity == null)
                        ? null
                        : md.GetValue(CurrentEntity);

                object proposed = md.GetValue(ProposedEntity);

                var comparer = FieldComparisonOverrides.ContainsKey(md.DataStoreFieldName) ? 
                    FieldComparisonOverrides[md.DataStoreFieldName] :
                    PropertyComparers.DefaultComparer;

                if(!comparer.AreEqual(current, proposed))
                    detectedChanges.Add(CreatePropertyChange(md, current, proposed));
            }

            return detectedChanges;
        }

        #endregion
    }
}