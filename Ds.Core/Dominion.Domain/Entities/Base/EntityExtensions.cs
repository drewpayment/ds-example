using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Utility.Containers;
using Dominion.Utility.Messaging;

namespace Dominion.Domain.Entities.Base
{
    public static class EntityExtensions
    {
        /// <summary>
        /// Copy the property values from the source entity to this entity.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity that we're dealing with.</typeparam>
        /// <param name="entity">The entity who's properties are to be set.</param>
        /// <param name="sourceEntity">The entity who's properties are to be copied.</param>
        public static void CopyFrom<TEntity>(this Entity<TEntity> entity, Entity<TEntity> sourceEntity)
            where TEntity : class
        {
            var properties = new PropertyList<TEntity>().IncludeAllProperties();

            foreach (var property in properties)
            {
                var sourceValue = property.GetPropertyInfo().GetValue(sourceEntity);
                property.GetPropertyInfo().SetValue(entity, sourceValue);
            }
        }

        // CopyFrom<TEntity>()
        #region STATIC VALIDATION HELPERS

        /// <summary>
        /// Determine whether the specified date property is within the desired range.
        /// </summary>
        /// <param name="entity">The entity whose date property is to be validated.</param>
        /// <param name="property">The date property to evaluate.</param>
        /// <param name="minDate">Minimum valid date.</param>
        /// <param name="maxDate">Maximum valid date.</param>
        /// <param name="validationErrors">List to which validation errors are added.</param>
        /// <returns>A validation error if the property is out of range, otherwise null.</returns>
        internal static void ValidateDateProperty<TEntity>(this Entity<TEntity> entity, 
            Expression<Func<TEntity, object>> property, 
            DateTime minDate, 
            DateTime maxDate, 
            List<ValidationResult> validationErrors)
            where TEntity : class, IEntity<TEntity>
        {
            // only perform the validation if required
            if (entity.ShouldValidateProperty(property))
            {
                // get the property value.
                PropertyInfo propertyInfo = property.GetPropertyInfo();
                string propertyName = propertyInfo.Name;
                DateTime? propertyValue = null;
                if (propertyInfo != null)
                {
                    propertyValue = propertyInfo.GetValue(entity) as DateTime?;
                }

                // check the property value.
                if ((propertyValue != null) && (propertyValue < minDate) || (propertyValue > maxDate))
                {
                    string msg = string.Format("The date must be between {0} and {1}.", 
                        minDate.ToShortDateString(), maxDate.ToShortDateString());

                    validationErrors.Add(new ValidationStatusMessage(
                        msg, 
                        ValidationStatusMessageType.OutOfRange, 
                        new[] {propertyName.ToString()}));
                }
            }
        }

        // ValidateDateProperty()


        /// <summary>
        /// Validate that none of the specified properties are null for the given entity.
        /// </summary>
        /// <param name="entity">The entity whose properties are to be checked.</param>
        /// <param name="properties">List of properties to check.</param>
        /// <returns>A list of validation errors that is populated when one or more property values are null.</returns>
        internal static IEnumerable<ValidationResult> ValidateRequiredProperties<TEntity>(
            this Entity<TEntity> entity, 
            PropertyList<TEntity> properties)
            where TEntity : class, IEntity<TEntity>
        {
            var validationErrors = new List<ValidationResult>();
            var nullProperties = new List<string>();

            // check each of the given properties for a null value.
            foreach (Expression<Func<TEntity, object>> property in properties)
            {
                // only perform the validation if required
                if (entity.ShouldValidateProperty(property))
                {
                    // get the value for the current property. if it's null, note it.
                    PropertyInfo propertyInfo = property.GetPropertyInfo();
                    string propertyName = propertyInfo.Name;
                    if ((propertyInfo != null) && (propertyInfo.GetValue(entity) == null))
                    {
                        nullProperties.Add(propertyName);
                    }
                }
            }

            // if there were any null values, generate a validation error.
            if (nullProperties.Count > 0)
            {
                validationErrors.Add(new ValidationStatusMessage(
                    ValidationStatusMessage.DEFAULT_REQUIRED_VALUE_MESSAGE, 
                    ValidationStatusMessageType.Required, 
                    nullProperties));
            }

            return validationErrors;
        }

        // ValidateRequiredProperties()


        /// <summary>
        /// Validate that all of the specified ID properties have a 'real' value (eg: greater than zero).
        /// </summary>
        /// <param name="entity">The entity whose properties are to be checked.</param>
        /// <param name="properties">List of properties to check.</param>
        /// <returns>A list of validation errors that is populated when one or more property values are invalid.</returns>
        internal static IEnumerable<ValidationResult> ValidateRequiredIdProperties<TEntity>(this Entity<TEntity> entity, 
            PropertyList<TEntity> properties)
            where TEntity : class, IEntity<TEntity>
        {
            var validationErrors = new List<ValidationResult>();
            var invalidProperties = new List<string>();

            // check each of the given properties for an invalid value.
            foreach (Expression<Func<TEntity, object>> property in properties)
            {
                // only perform the validation if required
                if (entity.ShouldValidateProperty(property))
                {
                    // get the value for the current property. if it's <= 0, note it.
                    PropertyInfo propertyInfo = property.GetPropertyInfo();
                    string propertyName = propertyInfo.Name;
                    if ((propertyInfo != null) && ((int) propertyInfo.GetValue(entity) <= 0))
                    {
                        invalidProperties.Add(propertyName);
                    }
                }
            }

            // if there were any invalid values, generate a validation error.
            if (invalidProperties.Count > 0)
            {
                validationErrors.Add(new ValidationStatusMessage(
                    ValidationStatusMessage.DEFAULT_REQUIRED_VALUE_MESSAGE, 
                    ValidationStatusMessageType.Required, 
                    invalidProperties));
            }

            return validationErrors;
        }

        // ValidateRequiredIdProperties()


        /// <summary>
        /// Checks to see if the specified property's value is found within the enumeration list.
        /// </summary>
        /// <typeparam name="TEntity">Entity type.</typeparam>
        /// <typeparam name="TEnum">Enum type.</typeparam>
        /// <param name="entity">Entity object to be checked.</param>
        /// <param name="property">Enum property to check.</param>
        /// <returns>Validation result collection.  Use .AddRange() when adding results to an existing validation result collection.</returns>
        internal static IEnumerable<ValidationResult> HasValidEnumValue<TEntity, TEnum>(this Entity<TEntity> entity, 
            Expression<Func<TEntity, TEnum>> property)
            where TEntity : class, IEntity<TEntity>
        {
            if (typeof (TEnum).IsEnum && ShouldValidateProperty(entity, property))
            {
                var typedEntity = entity as TEntity;

                TEnum enumValue = typedEntity.GetPropertyValue(property);

                if (! Enum.IsDefined(typeof (TEnum), enumValue))
                {
                    string msg = enumValue + " is an invalid " + typeof (TEnum).Name + " enum value.";

                    yield return
                        new ValidationStatusMessage(msg, ValidationStatusMessageType.OutOfRange, 
                            new[] {property.GetPropertyInfo().Name});
                }
            }
        }

        // HasValidEnumValue()


        /// <summary>
        /// Checks to see if the given property should be validated. Returns true if the PropertiesToValidate list 
        /// is null or empty, or the list contains the specified property.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity.</typeparam>
        /// <param name="entity">Entity to check for property level validation.</param>
        /// <param name="property">Property expression to check for validation.</param>
        /// <returns>Indication of whether validation should be performed on the given property.</returns>
        internal static bool ShouldValidateProperty<TEntity, TProperty>(this Entity<TEntity> entity, 
            Expression<Func<TEntity, TProperty>> property)
            where TEntity : class, IEntity<TEntity>
        {
            // if the PropertiesToValidate list has not been instantiated, is empty, or contains the given property, validate...
            if ((entity.PropertiesToValidate == null) ||
                (!entity.PropertiesToValidate.Any()) ||
                entity.PropertiesToValidate.ContainsProperty(property))
                return true;

            // ...otherwise, don't validate
            return false;
        }

        // ShouldValidateProperty()
        #endregion // STATIC VALIDATION HELPERS
    } // class EntityExtensions
}