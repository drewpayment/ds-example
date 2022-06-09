using Dominion.Aca.Dto.IRS;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// Entity for the aca.TransmissionRecordStatusType table.
    /// </summary>
    public class AcaTransmissionRecordStatusTypeInfo : Entity<AcaTransmissionRecordStatusTypeInfo>
    {
        public virtual TransmissionRecordStatusType RecordStatusType { get; set; } 
        public virtual string Description { get; set; } 
    }
}
