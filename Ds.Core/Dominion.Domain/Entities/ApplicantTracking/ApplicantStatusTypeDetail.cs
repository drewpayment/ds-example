using System.Collections.Generic;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantStatusTypeDetail : Entity<ApplicantStatusTypeDetail>
    {
        public virtual LaborManagement.Dto.ApplicantTracking.ApplicantStatusType ApplicantStatusId { get; set; }
        public virtual string Description { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual int SortOrder { get; set; }

        public virtual ICollection<ApplicantApplicationHeader> ApplicantApplicationHeaders { get; set; }

        public ApplicantStatusTypeDetail()
        {

        }
    }
}