using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;
using Dominion.Domain.Entities.Misc;
using System.Linq.Expressions;
using System.Linq;
using Dominion.LaborManagement.Dto.ApplicantTracking;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class Applicant : Entity<Applicant>
    {
        public int ApplicantId { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public int? State { get; set; }
        public string Zip { get; set; }
        public string PhoneNumber { get; set; }
        public string CellPhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime? Dob { get; set; }
        public int? EmployeeId { get; set; }
        public int? UserId { get; set; }
        public int ClientId { get; set; }
        public bool IsDenied { get; set; }
        public string WorkPhoneNumber { get; set; }
        public string WorkExtension { get; set; }
        public int? CountryId { get; set; }
        // indicates whether the text messaging for the applicant is enabled
        public bool? IsTextEnabled { get; set; }

        public virtual State StateDetails { get; set; }
        public virtual Country Country { get; set; }
        public virtual Client Client { get; set; }

        /// <summary>
        /// The user account associated with the Applicant.
        /// </summary>
        public virtual User.User User { get; set; }

        public virtual ICollection<ApplicantApplicationHeader> ApplicantApplicationHeaders { get; set; }
        public virtual ICollection<ApplicantEmploymentHistory> ApplicantEmploymentHistories { get; set; }
        public virtual ICollection<ApplicantEducationHistory> ApplicantEducationHistories { get; set; }
        public virtual ICollection<ApplicantSkill> ApplicantSkills { get; set; }
        public virtual ICollection<ApplicantLicense> ApplicantLicenses { get; set; }
        public virtual ICollection<ApplicantNote> ApplicantNote { get; set; }
        public virtual ICollection<ApplicantResume> ApplicantResume { get; set; }

        public Applicant()
        {
        }
        public static Expression<Func<Applicant, ApplicantApplicationHeader>> HiredApplicationHeader()
        {
            return x => x.ApplicantApplicationHeaders.Any() ? x.ApplicantApplicationHeaders.Where( y => y.ApplicantStatusTypeId == ApplicantStatusType.Hired
                && !y.ApplicantRejectionReasonId.HasValue).FirstOrDefault() : null ;
        }
    }
}
