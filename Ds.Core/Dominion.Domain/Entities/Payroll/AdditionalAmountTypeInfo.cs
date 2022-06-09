using Dominion.Core.Dto.Payroll;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Payroll
{
    public partial class AdditionalAmountTypeInfo : Entity<AdditionalAmountTypeInfo>
    {
        public virtual AdditionalAmountType AdditionalAmountTypeId { get; set; } 
        public virtual string Description { get; set; } 
    }
}