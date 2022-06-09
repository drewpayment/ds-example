using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;

namespace Dominion.Domain.Entities.Onboarding
{
    public partial class EmployeeW2Consent : Entity<EmployeeW2Consent>, IHasModifiedData
    {
        public virtual int EmployeeW2ConsentId { get; set; }
        public virtual int EmployeeId { get; set; }
        public virtual DateTime? ConsentDate { get; set; }
        public virtual DateTime? WithdrawalDate { get; set; }
        public virtual DateTime TaxYear { get; set; }
        public virtual string PrimaryEmailAddress { get; set; }
        public virtual string SecondaryEmailAddress { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual bool IsEmailVerified { get; set; }
        public virtual int ClientId { get; set; }

        public EmployeeW2Consent()
        {
        }
    }
}
