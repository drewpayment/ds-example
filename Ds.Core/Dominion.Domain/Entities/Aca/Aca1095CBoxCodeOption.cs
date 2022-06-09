using Dominion.Aca.Dto.Forms;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// 1095-C box code value info. (Entity for [dbo].[CompanyAca1095CBoxCodeOption] table)
    /// </summary>
    public partial class Aca1095CBoxCodeOption : Entity<Aca1095CBoxCodeOption>
    {
        public virtual int                 BoxCodeOptionId   { get; set; }
        public virtual Aca1095CBoxCodeType BoxCodeType       { get; set; }
        public virtual string              OptionValue       { get; set; }
        public virtual bool                IndicatesCoverage { get; set; }
        public virtual short?              YearFrom          { get; set; }
        public virtual short?              YearTo            { get; set; }


        public virtual Aca1095CBoxCodeTypeInfo BoxCodeTypeInfo { get; set; }
    }
}
