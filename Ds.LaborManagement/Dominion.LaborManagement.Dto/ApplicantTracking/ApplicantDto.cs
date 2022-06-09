using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Location;
using System;
using Dominion.Core.Dto.Location;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    [Serializable]
    public partial class ApplicantDto
    {
        public int ApplicantId { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public int State { get; set; }
        public string Zip { get; set; }
        public string PhoneNumber { get; set; }
        public string CellPhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? Dob { get; set; }
        public int? EmployeeId { get; set; }
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public int ClientId { get; set; }
        public bool IsDenied { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string WorkExtension { get; set; }
        public int? CountryId { get; set; }
        public bool IsTextEnabled { get; set; }
        public string StateAbbreviation { get; set; }
        public virtual StateDto StateDetails { get; set; }
        public virtual ClientDto Client { get; set; }
        public string CountryAbbreviation { get; set; }
    }
}
