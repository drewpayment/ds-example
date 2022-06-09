using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    /// <summary>
    /// A dto containing the number of posts who do not have a submitted application
    /// </summary>
    public class PostsWithoutApplicantsDto
    {
        public int TotalPostingsWithoutApplicants { get; set; }
    }
}
