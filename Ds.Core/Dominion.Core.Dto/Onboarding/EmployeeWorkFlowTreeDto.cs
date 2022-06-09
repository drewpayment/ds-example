using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.Core.Dto.Onboarding
{
    public class EmployeeWorkFlowTreeDto
    {
        public byte WorkFlowTaskId { get; set; }
        public byte? MainTaskId { get; set; }
        public string Title { get; set; }
        public string LinkToState { get; set; }
        public string Route { get; set; }
        public bool? IsHeader { get; set; }

        public List<EmployeeWorkFlowTreeDto> Children { get; set; }
    }
}
