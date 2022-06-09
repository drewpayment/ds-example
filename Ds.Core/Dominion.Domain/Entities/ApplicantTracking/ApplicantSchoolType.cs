using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantSchoolType : Entity<ApplicantSchoolType>
    {
        public virtual int ApplicantSchoolTypeId { get; set; }
        public virtual string Description { get; set; }
        public virtual int ApplicationOrder { get; set; }

        public ApplicantSchoolType()
        {

        }
    }
}