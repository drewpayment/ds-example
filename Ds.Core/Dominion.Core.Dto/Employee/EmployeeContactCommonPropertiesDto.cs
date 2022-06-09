using System;

using Dominion.Core.Dto.Contact.Legacy;
using Dominion.Core.Dto.Interfaces;
using Dominion.Core.Dto.Utility.Extensions;

namespace Dominion.Core.Dto.Employee
{
    /// <summary>
    /// Contains the phone numbers for the employee contact.
    /// Also the normalization method for the phone numbers.
    /// </summary>
    public class EmployeeContactCommonPropertiesDto : ContactNameDto, IEmployeePhoneNumbersDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public int? StateId { get; set; }
        public string StateName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public string PostalCode { get; set; }
        public int? CountyId { get; set;}
        public string CountyName { get; set; }
        public string Gender { get; set;}
        public MaritalStatus MaritalStatusId { get; set; }
        public DateTime? BirthDate { get; set; }
        public int JobProfileId { get; set; }
        public string JobTitleInfoDescription { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public string HomePhoneNumber { get; set; }
        public string CellPhoneNumber { get; set; }
        public string EmailAddress { get; set; }

        public DateTime? DriversLicenseExpirationDate { get; set;}
        public string   DriversLicenseNumber {get; set;}
        public int?     DriversLicenseIssuingStateId { get; set;}

        public bool? NoDriversLicense { get; set; }
        public override void Normalize()
        {
            this.NormalizePhoneNumbers();
            base.Normalize();
        }
    }
}