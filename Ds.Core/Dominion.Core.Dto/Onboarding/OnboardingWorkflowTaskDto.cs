using Dominion.Core.Dto.Client;
using Dominion.Core.Dto.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Onboarding
{
    public class OnboardingWorkflowTaskDto
    {
        public virtual int      OnboardingWorkflowTaskId       { get; set; }
        public virtual int?    MainTaskId                     { get; set; }
        public virtual string   WorkflowTitle                  { get; set; }
        public virtual string   LinkToState                    { get; set; }
        public virtual string   Route                          { get; set; }

        public virtual string   Route1                         { get; set; }
        public virtual bool?    IsHeader                       { get; set; }
        public virtual bool?    IsRequired                     { get; set; }
        public virtual byte?    Sequence                       { get; set; }
        public virtual int?     FormTypeId                     { get; set; }
        public virtual string   Description                    { get; set; }
        public virtual string   AdminDescription               { get; set; }
        public virtual bool     AdminMustSelect                { get; set; }
        public virtual int      ModifiedBy                     { get; set; }
        public virtual DateTime Modified                       { get; set; } 
        public virtual int?     FormDefinitionId               { get; set; }
        public virtual string   SignatureDescription           { get; set; }
        public int?             ClientId                       { get; set; }
        public bool             IsReferred                     { get; set; }
        public bool             HasActiveWorkflowReference     { get; set; }
        public bool             RequireWorkFlowTaskId          { get; set; }
        public virtual string   UploadDescription              { get; set; }
        public virtual bool     UserMustUpload                 { get; set; }
        public virtual List<CompanyResourceDto> Resources       { get; set; }
   	    public ResourceDto      UserMustUploadResource          { get; set; }
        public virtual bool     IsDeleted                      { get; set; }

    }
}
