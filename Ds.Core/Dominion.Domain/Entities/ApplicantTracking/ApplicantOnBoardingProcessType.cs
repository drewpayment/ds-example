using Dominion.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public class ApplicantOnBoardingProcessType : Entity<Applicant>
    {
        public virtual int ApplicantOnBoardingProcessTypeId { get; set; }
        public virtual string Description { get; set; }

        public virtual ICollection<ApplicantOnBoardingProcess> ApplicantOnBoardingProcess { get; set; }
    }
}
