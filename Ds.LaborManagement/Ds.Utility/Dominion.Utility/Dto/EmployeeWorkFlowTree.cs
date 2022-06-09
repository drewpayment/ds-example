using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Utility.Dto
{
    public class EmployeeWorkFlowTree
    {
        public EmployeeWorkFlowTree()
        {
            this.Children = new List<EmployeeWorkFlowTree>();
        }

        public byte WorkFlowTaskId { get; set; }
        //public byte? MainTaskId { get; set; }

        public int OnboardingWorkFlowTaskId { get; set; }
        public int? MainTaskId { get; set; }
        public string Title { get; set; }
        public string LinkToState { get; set; }
        public string Route { get; set; }
        public bool? IsHeader { get; set; }

        public bool? IsCompleted { get; set; }

        public string Description { get; set; }

        public List<EmployeeWorkFlowTree> Children { get; set; }
    }
}
