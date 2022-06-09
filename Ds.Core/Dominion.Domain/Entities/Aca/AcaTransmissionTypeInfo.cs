using Dominion.Aca.Dto.IRS;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Aca
{
    public partial class AcaTransmissionTypeInfo : Entity<AcaTransmissionTypeInfo>
    {
        public virtual TransmissionType TransmissionType { get; set; } 
        public virtual string           IrsCode          { get; set; } 
        public virtual string           Description      { get; set; } 
    }
}
