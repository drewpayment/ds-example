using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class ApplicantQuestionSetsDto
    {
        public int ApplicationId { get; set; }
        public int QuestionId    { get; set; }
        public int OrderId       { get; set; }

        public ApplicantQuestionControlDto    Question      { get; set; }
        public ApplicantCompanyApplicationDto Application   { get; set; }
    }
}
