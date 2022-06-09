using System;

using Dominion.Core.Dto.Common;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// Entity representation of the dbo.CompanyAcaAleMemberMonthDetail table.  Hold's per-month 1094-C ALE Member
    /// employer data.
    /// </summary>
    public partial class AcaAleMemberMonthDetail : Entity<AcaAleMemberMonthDetail>, IHasModifiedData
    {
        public virtual int       CompanyAcaAleMemberId           { get; set; } 
        public virtual MonthType MonthType                       { get; set; } 
        public virtual bool?     IsMinimumEssentialCoverageOffer { get; set; } 
        public virtual int?      FullTimeEmployeeCount           { get; set; } 
        public virtual int?      TotalEmployeeCount              { get; set; } 
        public virtual bool      IsAggregatedGroup               { get; set; } 
        public virtual string    Section4980HCode                { get; set; } 
        public virtual int       ModifiedBy                      { get; set; }
        public virtual DateTime  Modified                        { get; set; }

        //FOREIGN KEYS
        public virtual AcaAleMember AcaAleMember { get; set; } 
    }
}
