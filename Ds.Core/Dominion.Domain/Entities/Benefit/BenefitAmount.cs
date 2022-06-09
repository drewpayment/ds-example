using System.Collections.Generic;

using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Benefit
{
    public partial class BenefitAmount : Entity<BenefitAmount>
    {
        public virtual int     BenefitAmountId { get; set; }
        public virtual int     PlanOptionId    { get; set; }
        public virtual decimal AmountValue     { get; set; }
        public virtual bool    IsSelectable    { get; set; }

        public PlanOption PlanOption { get; set; }

        public ICollection<EmployeeOpenEnrollmentSelection> EmployeeSelections { get; set; }
    }
}
