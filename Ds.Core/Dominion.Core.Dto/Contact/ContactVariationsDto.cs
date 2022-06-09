using System;

namespace Dominion.Core.Dto.Contact
{
    /// <summary>
    /// Name and personal data.
    /// </summary>
    [Serializable]
    public class CommonContactPersonalDto2 : ContactNameDto2
    {
        public string SocialSecurityNumber { get; set; }
        public string Gender { get; set; }
        public DateTime? BirthDate { get; set; }
       
    }

    /// <summary>
    /// Name, Address, Personal.
    /// todo: jay: there is an address dto. maybe I should use it as a property here. Right now I'm not sure how much work that will cause so I will wait.
    /// </summary>
    [Serializable]
    public class CommonContactPersonalAddressDto2 : CommonContactPersonalDto2
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public string CountyName { get; set; }
        public string PostalCode { get; set; }
    }
}