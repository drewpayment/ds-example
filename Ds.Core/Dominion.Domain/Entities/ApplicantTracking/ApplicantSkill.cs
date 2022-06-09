using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Misc;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public class ApplicantSkill : Entity<ApplicantSkill>
    {
        public virtual int ApplicantSkillId { get; set; }
        public virtual int ApplicantId { get; set; }
        public virtual string Name { get; set; }
        public virtual byte Rating { get; set; }
        public virtual decimal? Experience { get; set; }
        public virtual string ExperienceType { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual Applicant Applicant { get; set; }
    }
}
