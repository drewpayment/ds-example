using System.Collections.Generic;

using Dominion.Aca.Dto.Forms;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// 1095-C Box code / line type information. (Entity for [dbo].[CompanyAca1095CBoxCodes] table)
    /// </summary>
    public partial class Aca1095CBoxCodeTypeInfo : Entity<Aca1095CBoxCodeTypeInfo>
    {
        public virtual Aca1095CBoxCodeType BoxCodeId   { get; set; } 
        public virtual string              Description { get; set; } 
        public virtual int                 SortOrder   { get; set; } 
        public virtual bool                IsActive    { get; set; } 
        public virtual string              BoxCode     { get; set; } 

        public virtual ICollection<Aca1095CBoxCodeOption> BoxCodeOptions { get; set; } 
    }
}
