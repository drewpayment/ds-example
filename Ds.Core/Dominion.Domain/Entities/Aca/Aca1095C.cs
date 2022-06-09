using System;
using System.Collections.Generic;
using Dominion.Aca.Dto.IRS;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// Top-level employee 1095-C record for a given ACA reporting year. (Entity for [dbo].[CompanyAca1095CSummary] table)
    /// </summary>
    public class Aca1095C : Entity<Aca1095C>, IHasModifiedData
    {
        public virtual short               Year                   { get; set; } 
        public virtual int                 EmployeeId             { get; set; } 
        public virtual int                 ClientId               { get; set; } 
        public virtual bool                Is1095CExempt          { get; set; } 
        public virtual int?                LastTransmissionId     { get; set; } 
        public virtual int?                LastSubmissionId       { get; set; } 
        public virtual int?                LastRecordId           { get; set; }
        public virtual TransmitStatusType? TransmitStatusTypeId   { get; set; } 
        public virtual DateTime?           PrintDate              { get; set; }
        public virtual bool                IsValidationWaived     { get; set; }
        public virtual DateTime?           ValidationWaiverDate   { get; set; }
        public virtual int?                ValidationWaivedById   { get; set; }
        public virtual string              ValidationWaiverNotes  { get; set; }
        public virtual DateTime            Modified               { get; set; } 
        public virtual int                 ModifiedBy             { get; set; } 

        public virtual Employee.Employee Employee           { get; set; } 
        public virtual Client            Client             { get; set; } 
        public virtual User.User         ModifiedByUser     { get; set; } 
        public virtual User.User         ValidationWaivedBy { get; set; }

        public virtual ICollection<Aca1095CLineItem>          LineItems           { get; set; }
        public virtual ICollection<Aca1095CCoveredIndividual> CoveredIndividuals  { get; set; }
        public virtual ICollection<AcaCorrection>             Corrections         { get; set; } 
        public virtual ICollection<AcaTransmissionRecord>     TransmissionRecords { get; set; } 
        public virtual ICollection<Aca1095CResource>          Resources           { get; set; } 

        public virtual AcaTransmission           LastTransmission { get; set; }
        public virtual AcaTransmissionSubmission LastSubmission   { get; set; }
        public virtual AcaTransmissionRecord     LastRecord       { get; set; }
    }
}
