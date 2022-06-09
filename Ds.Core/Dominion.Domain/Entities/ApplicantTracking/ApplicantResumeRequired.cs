using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantResumeRequired : Entity<ApplicantResumeRequired>
    {
        public virtual int ApplicantResumeRequiredId { get; set; }
        public virtual string Description { get; set; }

        public ApplicantResumeRequired()
        {

        }
    }
}
