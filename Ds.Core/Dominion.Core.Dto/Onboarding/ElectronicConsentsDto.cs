using System;



namespace Dominion.Core.Dto.Onboarding
{
    public partial class ElectronicConsentsDto
    {
        // W2 Properties
        public int EmployeeW2ConsentId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime? ConsentDateW2 { get; set; }
        public DateTime? WithdrawalDateW2 { get; set; }
        public DateTime TaxYear { get; set; }
        public string PrimaryEmailAddress { get; set; }
        public string SecondaryEmailAddress { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        public bool IsEmailVerified { get; set; }
        public int ClientId { get; set; }

        // 1095 Properties
        public bool Has1095C { get; set; }
        public DateTime? ConsentDate1095C { get; set; }
        public DateTime? WithdrawalDate1095C { get; set; }


        public bool IsW2Insert { get; set; }
        public bool Is1095CInsert { get; set; }
        public bool IsAcaEnabled { get; set; }
        
        
        public bool HasOnlyW2 { get; set; }
        public bool HasOnly1095C { get; set; }
    }
}
