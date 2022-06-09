using Dominion.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public class ApplicantOnBoardingProcessSet : Entity<ApplicantOnBoardingProcessSet>
    {
        public virtual int ApplicantOnboardingProcessId { get; set; }
        public virtual int ApplicantOnboardingTaskId { get; set; }
        public virtual int OrderId { get; set; }

        public virtual ApplicantOnBoardingProcess ApplicantOnBoardingProcess { get; set; }
        public virtual ApplicantOnBoardingTask ApplicantOnBoardingTask { get; set; }

    }
}
