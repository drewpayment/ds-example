using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.Labor.Enum;
using Dominion.LaborManagement.Dto.ApplicantTracking;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantApplicationEmailHistory : Entity<ApplicantApplicationEmailHistory>
    {
        public virtual int ApplicantApplicationEmailHistoryId { get; set; }
        public virtual int ApplicationHeaderId { get; set; }
        public virtual int? ApplicantCompanyCorrespondenceId { get; set; }
        public virtual ApplicantStatusType? ApplicantStatusTypeId { get; set; }
        public virtual int? SenderId { get; set; }
        public virtual DateTime? SentDate { get; set; }
        public virtual string SenderEmail { get; set; }
        public virtual string Subject { get; set; }
        public virtual string Body { get; set; }

        //FOREIGN KEYS
        public virtual ApplicantApplicationHeader ApplicantApplicationHeader { get; set; }
        public virtual ApplicantCompanyCorrespondence ApplicantCompanyCorrespondence { get; set; }
        public virtual User.User User { get; set; }

        public ApplicantApplicationEmailHistory()
        {
        }
    }
}