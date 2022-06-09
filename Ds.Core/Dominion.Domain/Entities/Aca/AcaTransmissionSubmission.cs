using System;
using System.Collections.Generic;

using Dominion.Aca.Dto.IRS;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// Entity for the aca.TransmissionSubmission table.
    /// </summary>
    public partial class AcaTransmissionSubmission : Entity<AcaTransmissionSubmission>, IHasModifiedData
    {
        public virtual short                             Year                      { get; set; } 
        public virtual int                               TransmissionId            { get; set; } 
        public virtual int                               SubmissionId              { get; set; } 
        public virtual int                               CompanyAleMemberId        { get; set; } 
        public virtual int                               ClientId                  { get; set; } 
        public virtual TransmissionSubmissionStatusType  SubmissionStatus          { get; set; } 
        public virtual int                               RecordCount1095           { get; set; }
        public virtual int?                              PreviousTransmissionId    { get; set; }
        public virtual int?                              PreviousSubmissionId      { get; set; } 
        public virtual DateTime                          Modified                  { get; set; } 
        public virtual int                               ModifiedBy                { get; set; } 

        //FOREIGN KEYS
        public virtual Client Client { get; set; } 
        public virtual AcaAleMemberClient AcaAleMemberClient { get; set; }
        public virtual AcaTransmissionSubmissionStatusTypeInfo SubmissionStatusInfo { get; set; } 
        public virtual User.User User { get; set; } 
        public virtual AcaTransmission Transmission { get; set; }
        public virtual AcaTransmission PreviousTransmission { get; set; }
        public virtual ICollection<AcaTransmissionRecord> TransmissionRecords { get; set; } 
        public virtual ICollection<AcaTransmissionError> TransmissionErrors { get; set; } 
    }
}