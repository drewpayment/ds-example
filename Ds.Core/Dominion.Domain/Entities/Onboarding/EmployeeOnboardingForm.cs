using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Core;
using Dominion.Domain.Entities.Forms;

namespace Dominion.Domain.Entities.Onboarding
{
    /// <summary>
    /// Linking entity between a form and onboarding employee.
    /// </summary>
    public partial class EmployeeOnboardingForm : Entity<EmployeeOnboardingForm>
    {
        public virtual int FormId     { get; set; } 
        public virtual int EmployeeId { get; set; } 

        public virtual EmployeeOnboarding EmployeeOnboarding { get; set; } 
        public virtual Form               Form               { get; set; } 
    }
}
