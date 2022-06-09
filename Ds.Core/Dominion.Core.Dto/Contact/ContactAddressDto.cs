using System;

namespace Dominion.Core.Dto.Contact
{
    [Serializable]
    public class ContactAddressDto2
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
        public string PostalCode { get; set; }
    }

    public class ContactAddressIdsDto2
    {
        public string   AddressLine1        { get; set; }
        public string   AddressLine2        { get; set; }
        public string   City                { get; set; }
        public int      StateId             { get; set; }
        public int      CountryId           { get; set; }
        public string   PostalCode          { get; set; }
    }
}