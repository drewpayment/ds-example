using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Onboarding
{
    [Serializable]
    public class EmployeeOnboardingEmergencyContactDto
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleInitial { get; set; }
        //public string AddressLine1 { get; set; }
        //public string AddressLine2 { get; set; }
        //public string City { get; set; }
        public int StateId { get; set; }
        //public string PostalCode { get; set; }
        public int CountryId { get; set; }
        public string HomePhoneNumber { get; set; }
        public string Relation { get; set; }
        //public DateTime Modified { get; set; }
        //public string ModifiedBy { get; set; }
        public int EmployeeEmergencyContactId { get; set; }
        //public string WorkPhoneNumber { get; set; }
        //public string CellPhoneNumber { get; set; }
        public byte? InsertApproved { get; set; }
        //public int ClientId { get; set; }
        public string EmailAddress { get; set; } 
    }
}
