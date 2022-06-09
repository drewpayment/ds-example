using System;
using System.Collections.Generic;

using Dominion.Aca.Dto.IRS;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// Entity for the aca.Transmission table.
    /// </summary>
    public partial class AcaTransmission :Entity<AcaTransmission>, IHasModifiedData
    {
        public virtual short                  Year                   { get; set; } 
        public virtual int                    TransmissionId         { get; set; } 
        public virtual string                 UniqueTranId           { get; set; } 
        public virtual string                 Tcc                    { get; set; } 
        public virtual string                 ShipmentNumber         { get; set; } 
        public virtual TransmissionType       TransmissionType       { get; set; } 
        public virtual string                 ReceiptId              { get; set; } 
        public virtual TransmissionStatusType TransmissionStatus     { get; set; } 
        public virtual DateTime               TransmissionStatusDate { get; set; } 
        public virtual string                 FileNameSent           { get; set; } 
        public virtual string                 FileNameError          { get; set; } 
        public virtual int                    RecordCount1094        { get; set; } 
        public virtual int?                   PreviousTransmissionId { get; set; }
        public virtual DateTime               CreationDate           { get; set; } 
        public virtual string                 Notes                  { get; set; }
        public virtual DateTime               Modified               { get; set; } 
        public virtual int                    ModifiedBy             { get; set; } 

        //FOREIGN KEYS
        public virtual AcaTransmissionTypeInfo TransmissionTypeInfo { get; set; } 
        public virtual AcaTransmissionStatusTypeInfo TransmissionStatusInfo { get; set; } 
        public virtual User.User User { get; set; } 

        public virtual ICollection<AcaTransmissionSubmission> Submissions { get; set; }
        public virtual ICollection<AcaTransmissionError> TransmissionErrors { get; set; }
        public virtual ICollection<AcaTransmission> NextTransmissions { get; set; } 
    }
}
