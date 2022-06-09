using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicationQuestionSection : Entity<ApplicationQuestionSection>
    {
        public virtual int SectionId { get; set; }
        public virtual string Description { get; set; }
        public virtual int DisplayOrder { get; set; }
        public virtual int ClientId { get; set; }
        public virtual bool IsEnabled { get; set; }

        //FOREIGN KEYS
        public virtual Client Client { get; set; }

        public virtual ICollection<ApplicantQuestionControl> ApplicantQuestionControl { get; set; }
        public virtual ICollection<ApplicationSectionInstruction> ApplicationSectionInstruction { get; set; }

        public ApplicationQuestionSection()
        {
        }
    }
}