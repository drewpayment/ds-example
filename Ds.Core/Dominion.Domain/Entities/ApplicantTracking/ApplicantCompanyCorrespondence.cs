using Dominion.Domain.Entities.Base;
using System;
using Dominion.Domain.Interfaces.Entities;
using System.Collections.Generic;
using Dominion.Domain.Entities.Clients;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantCompanyCorrespondence : Entity<ApplicantCompanyCorrespondence>, IHasModifiedOptionalData
    {
        public virtual int ApplicantCompanyCorrespondenceId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string Description { get; set; }
        public virtual LaborManagement.Dto.ApplicantTracking.ApplicantCorrespondenceType ApplicantCorrespondenceTypeId { get; set; }
        public virtual string Subject { get; set; }
        public virtual string Body { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual bool? IsText { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual int? ModifiedBy { get; set; }

        public virtual ApplicantCorrespondenceTypeInfo ApplicantCorrespondenceType { get; set; }
        public virtual Client Client { get; set; }

        public ApplicantCompanyCorrespondence()
        {
        }
    }
}