using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Forms;
using Dominion.Domain.Interfaces.Entities;
using System.Collections.Generic;

namespace Dominion.Domain.Entities.Onboarding
{
    public class OnboardingWorkflowTask : Entity<OnboardingWorkflowTask>, IHasModifiedData
    {
        public virtual int      OnboardingWorkflowTaskId         { get; set; }
        public virtual int?     MainTaskId                       { get; set; }
        public virtual int?     ClientId                         { get; set; }
        public virtual string   WorkflowTitle                    { get; set; }
        public virtual string   LinkToState                      { get; set; }
        public virtual string   Route                            { get; set; }
        public virtual bool?    IsHeader                         { get; set; }
        public virtual bool?    IsRequired                       { get; set; }
        public virtual byte?    Sequence                         { get; set; }
        public virtual int?     FormTypeId                       { get; set; }
        public virtual int      ModifiedBy                       { get; set; }
        public virtual DateTime Modified                         { get; set; }
        public virtual string   Description                      { get; set; }
        public virtual string   AdminDescription                 { get; set; }
        public virtual bool     AdminMustSelect                  { get; set; }
        public virtual string   SignatureDescription             { get; set; }
        public virtual bool     UserMustUpload                   { get; set; }
        public virtual string   UploadDescription                { get; set; }
        public virtual bool     IsDeleted                        { get; set; }

        public virtual OnboardingWorkflowTask MainTask { get; set; }

        public virtual ICollection<OnboardingWorkflowResources> OnboardingWorkflowResources { get; set; }
    }
}
