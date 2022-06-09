using Dominion.Aca.Dto.Approval;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// ACA status type information for a processing state of a client. (Entity for [dbo].[CompanyAcaApprovalStatus] table)
    /// </summary>
    public class AcaCompanyApprovalStatusInfo : Entity<AcaCompanyApprovalStatusInfo>
    {
        public virtual AcaCompanyApprovalStatus AcaCompanyApprovalStatus { get; set; }
        public virtual string Name { get; set; }
    }

}
