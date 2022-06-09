using System;



namespace Dominion.Core.Dto.Onboarding
{
    public partial class Employee1095CConsentDto
    {
        public int EmployeeId { get; set; }
        public int ClientId { get; set; }
        public DateTime? ConsentDate { get; set; }
        public DateTime? WithdrawalDate { get; set; }
        public string PrimaryEmailAddress { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
    }
}
