using Dominion.Aca.Dto.IRS;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// Entity for the aca.TransmissionStatusType table.
    /// </summary>
    public class AcaTransmissionStatusTypeInfo : Entity<AcaTransmissionStatusTypeInfo>
    {
        public virtual TransmissionStatusType TransmissionStatusType { get; set; } 
        public virtual string Description { get; set; } 
    }
}
