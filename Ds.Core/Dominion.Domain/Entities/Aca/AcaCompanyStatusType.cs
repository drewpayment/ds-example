using Dominion.Domain.Entities.Base;
using Dominion.Aca.Dto.Forms;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// ACA status type information for a client. Currently on 'Approval' type exists. May be expanded to include others
    /// in the future (e.g. Print, E-File, etc). (Entity for [dbo].[CompanyAcaStatusType] table)
    /// </summary>
    public class AcaCompanyStatusType : Entity<AcaCompanyStatusType>
    {
        public virtual AcaCompanyStatusTypes StatusType  { get; set; }
        public virtual string                Description { get; set; }
    }
}