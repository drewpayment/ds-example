using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dominion.Domain.Entities.Base;
using Dominion.Utility.Containers;

namespace Dominion.Domain.Interfaces.Entities
{
    public static class IModifiableEntityExtensions
    {
        public static IEnumerable<ValidationResult> ValidateModifiableEntityByUserIdProperties<TEntity>(
            this TEntity entity)
            where TEntity : Entity<TEntity>, IModifiableEntity<TEntity>
        {
            var validationResults = new List<ValidationResult>();

            // LastModifiedByUserId must be > 0
            if (entity.LastModifiedByUserId <= 0)
            {
                validationResults.Add(new ValidationResult("A required value is missing.", 
                    new List<string> {entity.GetPropertyInfo(x => x.LastModifiedByUserId).Name}));
            }

            // last mod date must be realistic
            DateTime minDate = new DateTime(2000, 1, 1); // this date was selected somewhat arbitrarily
            DateTime maxDate = DateTime.Now.AddDays(1.0);
            entity.ValidateDateProperty(x => x.LastModifiedDate, minDate, maxDate, validationResults);

            return validationResults;
        }
    }
}