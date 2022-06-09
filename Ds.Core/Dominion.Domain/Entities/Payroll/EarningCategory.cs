using System.Collections.Generic;
using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Payroll
{
    public class EarningCategory : Entity<EarningCategory>
    {
        public virtual ClientEarningCategory EarningCategoryId { get; set; }
        public virtual string Description { get; set; }
        public virtual int? Sequence { get; set; }
        public virtual bool IsAdjustToNet { get; set; }

        // REVERSE NAVIGATION
        public virtual ICollection<ClientEarning> ClientEarning { get; set; } // many-to-one;
    }
}