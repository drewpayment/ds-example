using System;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// ACA aggregate group members for a ALE Member. Used in 1094-C reporting. (Entity for [dbo].[CompanyAcaAleMemberGroup] table)
    /// </summary>
    public class AcaAleMemberGroup : Entity<AcaAleMemberGroup>, IHasModifiedData
    {
        public virtual int       CompanyAcaAleMemberGroupId          { get; set; } 
        public virtual int       CompanyAcaAleMemberId               { get; set; } 
        public virtual string    AleMemberName                       { get; set; } 
        public virtual string    FederalEmployerIdentificationNumber { get; set; } 
        public virtual int       ModifiedBy                          { get; set; }
        public virtual DateTime  Modified                            { get; set; }

        //FOREIGN KEYS
        public virtual AcaAleMember CompanyAcaAleMember { get; set; } 

    }
}
