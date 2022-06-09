using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.ApplicantTracking;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.Core
{
    public partial class ApplicantAzure : Entity<ApplicantAzure>
    {
        public virtual int ApplicantId { get; set; }
        public virtual string ApplicantGuId { get; set; }

        //FOREIGN KEYS
        public virtual Applicant Applicant { get; set; }
    }
}
