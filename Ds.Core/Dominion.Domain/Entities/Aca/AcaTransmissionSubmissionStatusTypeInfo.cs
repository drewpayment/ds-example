using Dominion.Aca.Dto.IRS;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// Entity for the aca.TransmissionSubmissionStatusType table.
    /// </summary>
    public class AcaTransmissionSubmissionStatusTypeInfo : Entity<AcaTransmissionSubmissionStatusTypeInfo>
    {
        public virtual TransmissionSubmissionStatusType SubmissionStatusType { get; set; } 
        public virtual string Description { get; set; } 
    }
}
