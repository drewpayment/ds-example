using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantCorrespondenceTypeInfo : Entity<ApplicantCorrespondenceTypeInfo>
    {
        public virtual LaborManagement.Dto.ApplicantTracking.ApplicantCorrespondenceType ApplicantCorrespondenceTypeId { get; set; }
        public virtual string Description { get; set; }

        public ApplicantCorrespondenceTypeInfo()
        {

        }
    }
}