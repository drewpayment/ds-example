using Dominion.Domain.Entities.Base;
using System.Collections.Generic;

namespace Dominion.Domain.Entities.Contact
{
    /// <summary>
    /// Entity for a person in the contact schema
    /// </summary>
    public class Person : Entity<Person>
    {
        public const string GENDER_MALE = "M";
        public const string GENDER_FEMALE = "F";

        // Basic Properties
        public virtual int    PersonId { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string MiddleInitial { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Phone { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string Title { get; set; }

        public virtual ICollection<EEOC.EEOCOrganization> EEOCOrganizations { get; set; }
    }
}