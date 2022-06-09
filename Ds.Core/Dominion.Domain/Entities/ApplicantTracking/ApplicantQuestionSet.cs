using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dominion.Domain.Entities.Base;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public class ApplicantQuestionSet : Entity<ApplicantQuestionSet>
    {
        public int ApplicationId { get; set; }
        public int QuestionId { get; set; }
        public int OrderId { get; set; }
        public ApplicantQuestionControl Question { get; set; }
        public ApplicantCompanyApplication Application { get; set; }
    }
}
