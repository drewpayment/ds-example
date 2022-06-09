using System;
using System.ComponentModel.DataAnnotations.Schema;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using System.ComponentModel.DataAnnotations;

namespace Dominion.Domain.Entities.Onboarding
{
    public partial class Employee1095CConsent
    {
        
        public virtual int EmployeeId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual DateTime? ConsentDate { get; set; }
        public virtual DateTime? WithdrawalDate { get; set; }
        public virtual string PrimaryEmailAddress { get; set; }
        public virtual bool IsEmailVerified { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }
    }
}
