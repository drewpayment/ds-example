using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Onboarding;
using Dominion.Domain.Entities.Forms;

namespace Dominion.Domain.Entities.Misc
{
    /// <summary>
    /// EXAMPLE: Entity: 
    /// All entities used in the system will be defined in this project under the 'Entities' folder.
    /// Do not use partial classes with entities.
    /// Place all entity code in the enties file or make helpers or providers.
    /// Rules: 
    /// - Folder: Dominion.Domain\Entities\[category]\file.
    /// - If this is an Entity Framework entity you must add the following:
    ///     - An EntityTypeConfiguration (db mapping detail) 
    ///         - look for other 'Entity: (config)' example 'to-dos'.
    ///     - As a IDbSet (collection) to the <see cref="IDominionContext"/>
    ///         - look for other 'Entity: (dbSet)' example 'to-dos'.
    ///     - A schema test
    ///         - look for other 'Entity: (schema test)' example 'to-dos'.
    ///     - Lookup data for unit testing
    ///         - look for other 'Entity: (lookup)' example 'to-dos'. 
    /// </summary>
    [Serializable]
    public class State : Entity<State>
    {
        public virtual int StateId { get; set; }

        public virtual int CountryId { get; set; }
        public virtual Country Country { get; set; }

        public virtual string Name { get; set; }
        public virtual string Code { get; set; }
        public virtual string Abbreviation { get; set; }
        public virtual string PostalNumericCode { get; set; }

        public virtual ICollection<County> County { get; set; }
        public virtual ICollection<SchoolDistrict> SchoolDistrict { get; set; }
        public virtual ICollection<FormType> FormType { get; set; }
        public virtual ICollection<ClientDivision> ClientDivisions { get; set; }
        public virtual ICollection<ClientDivisionAddress> DivisionAddresses { get; set; }

        #region Filter

        /// <summary>
        /// Predicate definition used to limit based on a specific country.
        /// </summary>
        /// <param name="countryId">The country's id.</param>
        /// <returns>Predicate.</returns>
        public static Expression<Func<State, bool>> ForCountry(int countryId)
        {
            return x => x.CountryId == countryId;
        }

        #endregion
    }
}