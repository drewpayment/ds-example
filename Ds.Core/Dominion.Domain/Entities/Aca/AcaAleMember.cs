using System;
using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// ACA ALE Member data for an employer (one per FEIN). Used in 1094-C report generation. (Entity for [dbo].[CompanyAcaAleMember] table)
    /// </summary>
    public class AcaAleMember : Entity<AcaAleMember>, IHasModifiedData
    {
        public virtual int       CompanyAcaAleMemberId                   { get; set; } 
        public virtual short     Year                                    { get; set; } 
        public virtual string    EmployerName                            { get; set; }
        public virtual string    FederalEmployerIdentificationNumber     { get; set; } 
        public virtual int       NumberOfForms                           { get; set; } 
        public virtual bool      IsAggregatedAleGroup                    { get; set; } 
        public virtual bool      IsQualifyingOfferMethod                 { get; set; } 
        public virtual bool      IsQualifyingOfferMethodTransitionRelief { get; set; } 
        public virtual bool      IsSection4980HTransitionRelief          { get; set; } 
        public virtual bool      IsNinetyEightPercentOfferMethod         { get; set; } 
        public virtual string    SignatureName                           { get; set; }
        public virtual string    SignatureTitle                          { get; set; }
        public virtual DateTime? SignatureDate                           { get; set; }
        public virtual int       ModifiedBy                              { get; set; }
        public virtual DateTime  Modified                                { get; set; }

        //REVERSE NAVIGATION
        public virtual ICollection<AcaAleMemberClient>      AleMemberClients      { get; set; } // many-to-many CompanyAcaAleMemberClient.FK_CompanyAcaAleMemberClient_CompanyAcaAleMember;
        public virtual ICollection<AcaAleMemberMonthDetail> AleMemberMonthDetails { get; set; }
        public virtual ICollection<AcaAleMemberGroup>       AcaAleMemberGroups    { get; set; } // many-to-one;


    }
}
