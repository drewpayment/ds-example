using System;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Aca
{
    public partial class AcaTransmissionError : Entity<AcaTransmissionError>, IHasModifiedData
    {
        public virtual short    Year           { get; set; } 
        public virtual int      CorrectionId   { get; set; } 
        public virtual int      TransmissionId { get; set; } 
        public virtual int?     SubmissionId   { get; set; } 
        public virtual int?     RecordId       { get; set; } 
        public virtual int      ErrorCodeId    { get; set; } 
        public virtual DateTime Modified       { get; set; } 
        public virtual int      ModifiedBy     { get; set; } 

        //FOREIGN KEYS
        public virtual AcaErrorCode              ErrorCode      { get; set; } 
        public virtual User.User                 ModifiedByUser { get; set; }
        public virtual AcaCorrection             Correction     { get; set; }
        public virtual AcaTransmission           Transmission   { get; set; }
        public virtual AcaTransmissionSubmission Submission     { get; set; }
        public virtual AcaTransmissionRecord     Record         { get; set; }
    }
}
