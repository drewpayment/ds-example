using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Onboarding
{
    public partial class ElectronicConsents : Entity<ElectronicConsents>
        
    {
        public int EmployeeW2ConsentId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? ConsentDateW2 { get; set; }
        public DateTime? WithdrawalDateW2 { get; set; }
        public DateTime TaxYear { get; set; }
        public string PrimaryEmailAddress { get; set; }
        public string SecondaryEmailAddress { get; set; }
        public DateTime Modified { get; set; }
        public int ModifiedBy { get; set; }
        [Column("EmailVerified")]
        public bool IsEmailVerified { get; set; }
        public int ClientId { get; set; }

        // 1095 Properties
        public bool Has1095C { get; set; }
        public DateTime? ConsentDate1095C { get; set; }
        public DateTime? WithdrawalDate1095C { get; set; }

        public ElectronicConsents()
        {
        }
    }
}
