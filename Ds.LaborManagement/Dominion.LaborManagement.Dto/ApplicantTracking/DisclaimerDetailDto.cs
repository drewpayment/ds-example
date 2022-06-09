using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Core.Dto.Location;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class DisclaimerDetailDto
    {
        public int ApplicantId { get; set; }
        public int ApplicantApplicationHeaderId { get; set; }
        public int ApplicantPostingId { get; set; }
        public int ApplicantCompanyCorrespondenceId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string Body { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Boolean IsApplicationSubmitted { get; set; }
        public DateTime? ApplicationSubmittedOn { get; set; }
        public string Posting { get; set; }
        public DateTime Date { get; set; }
        public string ApplicantAddress1 { get; set; }
        public string ApplicantAddress2 { get; set; }
        public string ApplicantCity { get; set; }
        public StateDto ApplicantState { get; set; }
        public string ApplicantPostalCode { get; set; }
        public string ApplicantPhoneNumber { get; set; }
        public string CompanyAddress1 { get; set; }
        public string CompanyAddress2 { get; set; }
        public string CompanyCity { get; set; }
        public StateDto CompanyState { get; set; }
        public string CompanyPostalCode { get; set; }
        public int? CoverLetterId { get; set; }
        public string CoverLetter { get; set; }
    }
}
