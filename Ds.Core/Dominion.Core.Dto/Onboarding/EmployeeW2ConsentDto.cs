using System;



namespace Dominion.Core.Dto.Onboarding
{
    public partial class EmployeeW2ConsentDto
    {
        public int EmployeeW2ConsentId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? ConsentDate { get; set; }
        public DateTime? WithdrawalDate { get; set; }
        public DateTime TaxYear { get; set; }
        public string PrimaryEmailAddress { get; set; }
        public string SecondaryEmailAddress { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsEmailVerified { get; set; }
        public int ClientId { get; set; }



    }
}
