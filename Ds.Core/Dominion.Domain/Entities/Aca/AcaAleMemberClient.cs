using System;
using System.Collections.Generic;
using Dominion.Aca.Dto.IRS;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Misc;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// Client 1094-C data for an ACA reporting year. (Entity for [dbo].[CompanyAcaAleMemberClient] table)
    /// </summary>
    public class AcaAleMemberClient : Entity<AcaAleMemberClient>, IHasModifiedData
    {
        public virtual int                             ClientId                   { get; set; } 
        public virtual int                             CompanyAcaAleMemberId      { get; set; } 
        public virtual short                           Year                       { get; set; }
        public virtual int                             NumberOfForms              { get; set; } 
        public virtual bool                            IsAuthoritativeTransmittal { get; set; } 
        public virtual string                          ContactFirstName           { get; set; } 
        public virtual string                          ContactLastName            { get; set; }
        public virtual string                          ContactMiddleInitial       { get; set; }
        public virtual string                          ContactTitle               { get; set; } 
        public virtual string                          ContactPhoneNumber         { get; set; } 
        public virtual string                          ContactEmailAddress        { get; set; } 
        public virtual string                          AddressLine1               { get; set; } 
        public virtual string                          AddressLine2               { get; set; } 
        public virtual string                          City                       { get; set; } 
        public virtual string                          Zip                        { get; set; } 
        public virtual int?                            StateId                    { get; set; } 
        public virtual int?                            CountryId                  { get; set; }
        public virtual int?                            LastTransmissionId         { get; set; }
        public virtual int?                            LastSubmissionId           { get; set; }
        public virtual TransmitStatusType?             TransmitStatusTypeId       { get; set; }
        public virtual int                             ModifiedBy                 { get; set; }
        public virtual DateTime              Modified                   { get; set; } 
        public virtual DateTime?             CorrectionEmailSentDt      { get; set; }

        //FOREIGN KEYS
        public virtual AcaAleMember              AcaAleMember           { get; set; } 
        public virtual Client                    Client                 { get; set; } 
        public virtual State                     State                  { get; set; }
        public virtual Country                   Country                { get; set; }
        public virtual AcaTransmissionSubmission LastSubmission         { get; set; }
        public virtual AcaTransmission           LastTransmission       { get; set; }

        /// <summary>
        /// All corrections ever associated with this client-1094 AND sub-sequent 1095-Cs. Filter out corrections
        /// with an employee ID if you want only corrections associated with the 1094.
        /// </summary>
        public virtual ICollection<AcaCorrection>             Corrections   { get; set; }

        /// <summary>
        /// All submissions this client-1094 has been sent in.
        /// </summary>
        public virtual ICollection<AcaTransmissionSubmission> Submissions   { get; set; }
    }
}
