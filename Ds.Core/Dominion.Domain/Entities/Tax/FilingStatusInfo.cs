using Dominion.Domain.Entities.Base;
using Dominion.Taxes.Dto.TaxOptions;

namespace Dominion.Domain.Entities.Tax
{
    public class FilingStatusInfo : Entity<FilingStatusInfo>
    {
        public virtual FilingStatus FilingStatus { get; set; }
        public virtual string       Description  { get; set; }
        public virtual int          DisplayOrder { get; set; }
    }
}