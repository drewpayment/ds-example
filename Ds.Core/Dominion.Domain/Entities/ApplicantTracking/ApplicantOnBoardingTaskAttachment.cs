using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public class ApplicantOnBoardingTaskAttachment : Entity<ApplicantOnBoardingTaskAttachment>, IHasModifiedData
    {
        public virtual int ApplicantOnBoardingTaskAttachmentId { get; set; }
        public virtual int ApplicantOnBoardingTaskId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string Description { get; set; }
        public virtual string FileLocation { get; set; }
        public virtual DateTime Modified { get; set; }
        public virtual int ModifiedBy { get; set; }
        public virtual string FileType { get; set; }

        public virtual ApplicantOnBoardingTask ApplicantOnBoardingTask { get; set; }
    }
}
