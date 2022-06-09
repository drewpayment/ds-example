using System;
using System.Linq.Expressions;
using Dominion.Utility.Containers;
using Dominion.Utility.EntityDataStore.EntityMetadata.Interfaces;
using Dominion.Utility.ExtensionMethods;
using Dominion.Utility.ExtensionMethods.PropertyExpressions;

namespace Dominion.Utility.EntityDataStore.EntityMetadata
{
    /// <summary>
    /// Metadata for a datastore's field/column.
    /// </summary>
    /// <typeparam name="TEntity">The entity type.</typeparam>
    /// <typeparam name="TPropertyType">The property type.</typeparam>
    public class EntityDataStoreFieldMetadata<TEntity, TPropertyType> : IEntityDataStoreFieldMetadata
        where TEntity : class
    {
        #region Variables and Properties

        private bool _isPrimaryKey;
        private string _fieldName;
        private string _pocoName;
        private int _maxLength;
        private int _precision;
        private int _scale;
        private bool _isRequired;
        private bool _isCurrency;
        private Expression<Func<TEntity, TPropertyType>> _propertyExpr;
        private Func<TPropertyType, TPropertyType, bool> _comparator;

        /// <summary>
        /// True if the field is the primary key.
        /// </summary>
        public bool IsPrimaryKey
        {
            get { return _isPrimaryKey; }
        }

        /// <summary>
        /// If the data store was a database, this would be the column name.
        /// </summary>
        public string DataStoreFieldName
        {
            get { return _fieldName; }
        }

        /// <summary>
        /// The C# Entity object's name.
        /// </summary>
        public string PocoName
        {
            get { return _pocoName; }
        }

        /// <summary>
        /// If there is a max value for this field.
        /// </summary>
        public int MaxLength
        {
            get { return _maxLength; }
        }

        /// <summary>
        /// If the data store is a database this would be the precision of the column.
        /// </summary>
        public int Precision
        {
            get { return _precision; }
        }

        /// <summary>
        /// If the data store is a database this would be the scale of the column.
        /// </summary>
        public int Scale
        {
            get { return _scale; }
        }

        /// <summary>
        /// is the field required?.
        /// </summary>
        public bool IsRequired
        {
            get { return _isRequired; }
        }

        /// <summary>
        /// Does this field represent a currency value.
        /// </summary>
        public bool IsCurrency
        {
            get { return _isCurrency; }
        }

        /// <summary>
        /// Specifies a custom function to be used during property comparison.
        /// </summary>
        public Func<object, object, bool> CustomComparator
        {
            get
            {
                if(_comparator == null) return null;
                return (orig, proposed) => _comparator((TPropertyType)orig, (TPropertyType)proposed);
            }
        }

        /// <summary>
        /// Specifies a custom function to be used during property comparison.
        /// </summary>
        public Func<TPropertyType, TPropertyType, bool> Comparator
        {
            get { return _comparator; }
            set { _comparator = value; }
        }

        /// <summary>
        /// Gets the property expression.
        /// </summary>
        /// <value>
        /// The property expression.
        /// </value>
        public Expression<Func<TEntity, TPropertyType>> PropertyExpression
        {
            get { return _propertyExpr; }
        }

        #endregion

        #region Constructors and Initializers

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isPrimaryKey">Is the field a primary key.</param>
        /// <param name="required">Is the field required.</param>
        /// <param name="isCurrency">Does the field represent a currency value.</param>
        /// <param name="fieldName">If the date store is a database this would be the column name.</param>
        /// <param name="maxLength">The max value for the field.</param>
        /// <param name="precision">The precision of a numeric value (database).</param>
        /// <param name="scale">The scale of a numeric value (database).</param>
        /// <param name="propertyExpression">
        /// A property expression for this field.
        /// </param>
        public EntityDataStoreFieldMetadata(
            bool isPrimaryKey, 
            bool required, 
            bool isCurrency, 
            string fieldName, 
            int maxLength, 
            int precision, 
            int scale, 
            Expression<Func<TEntity, TPropertyType>> propertyExpression)
        {
            _isPrimaryKey = isPrimaryKey;
            _fieldName = fieldName;
            _isCurrency = isCurrency;
            _pocoName = propertyExpression.GetPropertyInfo().Name;
            _maxLength = 6;
            _precision = 1;
            _scale = 1;
            _isRequired = false;
            _propertyExpr = propertyExpression;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The the value from the entity based on the property expression.
        /// </summary>
        /// <param name="obj">The entity.</param>
        /// <returns>The object value based on property expression.</returns>
        public object GetValue(object obj)
        {
            return PropertyExpression.GetMemeberValue((TEntity) obj);
        }

        /// <summary>
        /// The the value from the field based on the property expression.
        /// </summary>
        /// <returns></returns>
        public Type GetPropertyType()
        {
            return PropertyExpression.GetPropertyInfo().PropertyType;
        }

        /// <summary>
        /// True if the values data type is a System.Nullable from the field based on the property expression.
        /// </summary>
        /// <returns></returns>
        public bool IsPropertyNullable()
        {
            return PropertyExpression.GetPropertyInfo().PropertyType.IsNullableType();
        }

        #endregion
    }
}