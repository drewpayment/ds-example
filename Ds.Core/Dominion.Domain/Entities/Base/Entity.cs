using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Xml.Serialization;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Domain.Validation;
using Dominion.Utility.Containers;
using Dominion.Utility.Msg.Specific;
using Dominion.Utility.OpResult;
using Dominion.Utility.Validation;
using FluentValidation;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace Dominion.Domain.Entities.Base
{
    /// <summary>
    /// Base class for all Entities to derive from.
    /// </summary>
    /// <typeparam name="TEntity">Type of entity.</typeparam>
    /// <remarks>
    /// Define a derived entity class as follows:
    /// 
    /// public partial class Employee : Entity&lt;Employee&gt;
    /// { ... }    
    /// </remarks>
    public abstract class Entity<TEntity> : IEntity<TEntity>
        where TEntity : class
    {
        [Obsolete("We no longer add validation on the entity. We will no longer use EntityValidator<T>. See: Dominion.Utility.Validation.FluentValidate.FluentValidator<T>. ")]
        protected IVerify<TEntity> _validator;

        #region PROPERTY LISTS

        /// <summary>
        /// List of properties to validate when Entity.Validate() is performed
        /// </summary>
        [NotMapped]
        [XmlIgnore]
        [Obsolete("We no longer add validation on the entity. We will no longer use EntityValidator<T>. See: Dominion.Utility.Validation.FluentValidate.FluentValidator<T>. ")]
        public PropertyList<TEntity> PropertiesToValidate
        {
            get
            {
                return
                    (_validator != null)
                        ? _validator.PropertiesToValidate
                        : null;
            }

            set
            {
                if(_validator != null)
                    _validator.PropertiesToValidate = value;
            }
        }

        /// <summary>
        /// List of the entity's key properties.  
        /// </summary>
        /// <remarks>
        /// This must be overridden in each derived entity implementation using 
        /// a 'new static' property of the same name.
        /// 
        ///    Example implementation in derived class:
        ///    
        ///        public partial class Employee
        ///        {
        ///            public new static EntityKeys
        ///            {
        ///                get 
        ///                { 
        ///                    return new PropertyList&lt;Employee&gt;
        ///                    {
        ///                        x => x.EmployeeId
        ///                    };
        ///                }
        ///            }
        ///        }
        /// </remarks>
        [XmlIgnore]
        public static PropertyList<TEntity> EntityKeyProperties
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// List of the entity's relationship Ids (foreign keys) used to 
        /// access Navigation Properties.
        /// </summary>
        /// <remarks>
        /// This must be overridden in each derived entity implementation using 
        /// a 'new static' property of the same name.
        /// 
        /// (see EntityKeys property remarks for example)
        /// </remarks>
        [XmlIgnore]
        public static PropertyList<TEntity> RequiredEntityRelationshipIdProperties
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// List of the entity's required properties that could be set to 
        /// null due to the property's type (eg. strings).
        /// </summary>
        /// <remarks>
        /// This must be overridden in each derived entity implementation using 
        /// a 'new static' property of the same name.
        /// 
        /// (see EntityKeys property remarks for example)
        /// </remarks>
        [XmlIgnore]
        public static PropertyList<TEntity> RequiredNullableProperties
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        /// <summary>
        /// Set the validation rules for this object.
        /// </summary>
        /// <param name="validator">The validation rules.</param>
        /// <param name="properties">The properties to validate. null or empty indicates that all properties
        /// should be validated.</param>
        [Obsolete("We no longer add validation on the entity. We will no longer use EntityValidator<T>. See: Dominion.Utility.Validation.FluentValidate.FluentValidator<T>. ")]
        public virtual void SetValidator(EntityValidator<TEntity> validator, PropertyList<TEntity> properties = null)
        {
            _validator = validator;
            PropertiesToValidate = properties;
        }

        #region IEntity IMPLEMENTATION

        /// <summary>
        /// Validate all properties.
        /// </summary>
        /// <returns>Set of validation errors, if there are any.</returns>
        [Obsolete("We no longer add validation on the entity. We will no longer use EntityValidator<T>. See: Dominion.Utility.Validation.FluentValidate.FluentValidator<T>. ")]
        public IEnumerable<ValidationResult> Validate()
        {
            return Validate(new ValidationContext(this, null, null));
        }

        /// <summary>
        /// Entity validation that is done at the domain level.
        /// Validation was re-done and until the older validation is phased out, we will be calling this function.
        /// </summary>
        /// <returns></returns>
        [Obsolete("We no longer add validation on the entity. We will no longer use EntityValidator<T>. See: Dominion.Utility.Validation.FluentValidate.FluentValidator<T>. ")]
        public IOpResult ConfirmValidity()
        {
            return _validator.Verify(this as TEntity);
        }

        #endregion

        #region IValidatableObject IMPLEMENTATION

        /// <summary>
        /// Validate the properties specified by PropertiesToValidate, or all if PropertiesToValidate is null or empty.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>Set of validation errors, if any.</returns>
        [Obsolete("We no longer add validation on the entity. We will no longer use EntityValidator<T>. See: Dominion.Utility.Validation.FluentValidate.FluentValidator<T>. ")]
        public virtual IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            var validationErrors = new List<ValidationResult>();
            var fluentAbstractValidator = _validator as AbstractValidator<TEntity>;

            if (this is TEntity)
            {
                if (_validator != null)
                {
                    if ((PropertiesToValidate == null) || (PropertiesToValidate.Count == 0))
                    {
                        validationErrors.AddRange(fluentAbstractValidator.Validate(this as TEntity).AsValidationStatusMessages());
                    }
                    else
                    {
                        validationErrors.AddRange(
                            fluentAbstractValidator.Validate(this as TEntity, PropertiesToValidate.GetPropertyNames().ToArray())
                                .AsValidationStatusMessages());
                    }
                }
            }

            return validationErrors;
        }

        #endregion

        #region HELPER METHODS

        /// <summary>
        /// Creates a shallow copy of the current entity object.
        /// </summary>
        /// <returns></returns>
        public TEntity ShallowClone()
        {
            return (TEntity) this.MemberwiseClone();
        }
        /// </summary>
        /// <typeparam name="TEntity">The entity type.</typeparam>
        /// <param name="source">The source entity.</param>
        public TEntity ShallowCopy<TEntity>(TEntity source) where TEntity : class, new() 
        {

            // Get properties from EF that are read/write and not marked witht he NotMappedAttribute
            var sourceProperties = typeof(TEntity)
                .GetProperties()
                .Where(p => p.CanRead && p.CanWrite &&
                            p.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute), true).Length == 0);
            var newObj = new TEntity();

            foreach (var property in sourceProperties)
            {

                // Copy value
                property.SetValue(newObj, property.GetValue(this, null), null);

            }

            return newObj;

        }

        #endregion

    }
}
