using System;
using Dominion.Taxes.Dto.Taxes;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicantToEmployeePostDto
    {
        public int ApplicationHeaderId { get; set; }
        public int ApplicantId { get; set; }
        public int? EmployeeId { get; set; }
        public int? EmployeeNumber { get; set; }
        public string Gender { get; set; }
        public string SocialSecurityNumber { get; set; }
        public DateTime? HireDate { get; set; }
        public int PayFrequencyId { get; set; }
        public int EmployeePayTypeId { get; set; }
        public int EmployeeStatusId { get; set; }
        public decimal? Salary { get; set; }
        public int? HourlyRateTypeId { get; set; }
        public decimal? HourlyRate { get; set; }
        public int PostingId { get; set; }
        public int ClientId { get; set; }
        public int? UserId { get; set; }
        public bool StartedOnboarding { get; set; }
        public BasicTaxDto SutaState { get; set; }
        public int? DirectSupervisorId { get; set; }
    }
}