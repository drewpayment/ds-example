using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantCoverLetterRequired : Entity<ApplicantCoverLetterRequired>
    {
        public virtual int ApplicantCoverLetterRequiredId { get; set; }
        public virtual string Description { get; set; }

        public ApplicantCoverLetterRequired()
        {

        }
    }
}
