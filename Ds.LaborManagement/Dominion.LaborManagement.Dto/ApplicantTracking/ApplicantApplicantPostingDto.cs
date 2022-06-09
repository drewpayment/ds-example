using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicantApplicantPostingDto
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
        public int? CountryId { get; set; }
        public string PhoneNumber { get; set; }
        public string CellPhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? Dob { get; set; }

        public string JobTitle { get; set; }
        public int? JobProfileId { get; set; }
        public int? ClientDivisionId { get; set; }
        public int? ClientDepartmentId { get; set; }
        public DateTime? HireDate { get; set; }
        public int? ClientGroupId { get; set; }
        public string JobClass { get; set; }
        public int? EeocJobCategoryId { get; set; }
        public int? EeocLocationId { get; set; }
        public int? ClientWorkersCompId { get; set; }
        public int? ClientCostCenterId { get; set; }
        public int? ClientShiftId { get; set; }
        public bool IsBenefitEligible { get; set; }
        public bool IsExternalApplicant { get; set; }
        public string JobSiteName { get; set; }
    }
}
