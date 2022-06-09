using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantJobType : Entity<ApplicantJobType>
    {
        public virtual int ApplicantJobTypeId { get; set; }
        public virtual string Description { get; set; }

        public ApplicantJobType()
        {

        }
    }
}