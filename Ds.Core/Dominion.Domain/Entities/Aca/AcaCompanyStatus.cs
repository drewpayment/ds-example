using System;

using Dominion.Aca.Dto.Approval;
using Dominion.Aca.Dto.Forms;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Aca
{
    /// <summary>
    /// Specifies the ACA processing state of a client. (Entity for [dbo].[CompanyAcaStatus] table)
    /// </summary>
    public class AcaCompanyStatus : Entity<AcaCompanyStatus>, IHasModifiedData
    {
        public virtual int                      ClientId   { get; set; }
        public virtual short                    Year       { get; set; }
        public virtual AcaCompanyStatusTypes    StatusType { get; set; }
        public virtual AcaCompanyApprovalStatus Status     { get; set; }
        public virtual DateTime                 Modified   { get; set; }
        public virtual int                      ModifiedBy { get; set; }

        //FOREIGN KEYS
        public virtual Client Client { get; set; }
    }
}
