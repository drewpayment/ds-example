using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicationSectionInstruction : Entity<ApplicationSectionInstruction>
    {
        public virtual int SectionInstructionId { get; set; }
        public virtual int SectionId { get; set; }
        public virtual int ClientId { get; set; }
        public virtual string Instruction { get; set; }

        //FOREIGN KEYS
        public virtual Client Client { get; set; }
        public virtual ApplicationQuestionSection ApplicationQuestionSection { get; set; }

        public ApplicationSectionInstruction()
        {
        }
    }
}