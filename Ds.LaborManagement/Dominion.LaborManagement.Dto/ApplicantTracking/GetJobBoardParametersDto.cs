using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    [Serializable]
    public partial class GetJobBoardParametersDto
    {
        public int ClientId { get; set; }
        public int ApplicantId { get; set; }
        public bool IsAdmin { get; set; }
        public int UserId { get; set; }
        public bool ShowOpenPostings { get; set; }
        public bool ShowAllPostings { get; set; }
    }
}