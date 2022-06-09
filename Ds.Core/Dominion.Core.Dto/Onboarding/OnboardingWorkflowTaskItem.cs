using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Onboarding
{
    public class OnboardingWorkflowTaskItem
    {
        public virtual int WorkflowTaskId { get; set; }
        public virtual string WorkflowTitle { get; set; }
        public virtual int Type { get; set; }
        public virtual string Description { get; set; }
        public virtual string AdminDescription { get; set; }
        public virtual string SignatureDescription { get; set; }
        public virtual int ClientId { get; set; }
        public virtual List<int> Resources { get; set; }
        public virtual string UploadDescription { get; set; }
        public virtual bool UserMustUpload { get; set; }
        public virtual bool IsDeleted { get; set; }

    }
}
