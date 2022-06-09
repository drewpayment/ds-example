using System;
using System.Collections.Generic;

using Dominion.Aca.Dto.IRS;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// Entity for the aca.TransmissionRecord table.
    /// </summary>
    public partial class AcaTransmissionRecord : Entity<AcaTransmissionRecord>, IHasModifiedData
    {
        public virtual short                         Year                       { get; set; } 
        public virtual int                           TransmissionId             { get; set; } 
        public virtual int                           SubmissionId               { get; set; } 
        public virtual int                           RecordId                   { get; set; } 
        public virtual int                           EmployeeId                 { get; set; }
        public virtual TransmissionRecordStatusType  RecordStatus               { get; set; } 
        public virtual int?                          PreviousTransmissionId     { get; set; } 
        public virtual int?                          PreviousSubmissionId       { get; set; } 
        public virtual int?                          PreviousRecordId           { get; set; } 
        public virtual DateTime                      Modified                   { get; set; } 
        public virtual int                           ModifiedBy                 { get; set; } 

        //FOREIGN KEYS
        public virtual AcaTransmission Transmission { get; set; }
        public virtual AcaTransmission PreviousTransmission { get; set; }
        public virtual AcaTransmissionSubmission Submission { get; set; }
        public virtual Employee.Employee Employee { get; set; } 
        public virtual Aca1095C Aca1095C { get; set; }
        public virtual AcaTransmissionRecordStatusTypeInfo RecordStatusInfo { get; set; } 
        public virtual User.User User { get; set; } 
        public virtual ICollection<AcaTransmissionError> TransmissionErrors { get; set; } 
    }
}
