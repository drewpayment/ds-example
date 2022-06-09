using System;

namespace Dominion.Core.Dto.Contact
{
    [Serializable]
    public class ContactPersonalDto2
    {
        public string SocialSecurityNumber { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}