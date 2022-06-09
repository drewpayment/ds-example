using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    public class PostingDetailDto
    {
        public int PostingId { get; set; }
        public int PostingNumber { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string DepartmentName { get; set; }
        public DateTime? PublishStart { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FilledDate { get; set; }
        public string Location { get; set; }
        public bool IsGeneralApplication { get; set; }
    }
    public class ApplicantDaysToHireDetailDto : PostingDetailDto
    {
        public double AverageDaysToHire { get; set; }
    }
    public class PostingApplicationsDetailDto : PostingDetailDto
    {
        public int NumOfPositions { get; set; }
        public int Applications { get; set; }
        public int Applicants { get; set; }
        public int ApplicantsHired { get; set; }
        public int Candidates { get; set; }
    }
}
