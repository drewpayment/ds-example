using Dominion.Core.Dto.Labor;
using System;
using System.Collections.Generic;

namespace Dominion.LaborManagement.Dto.ApplicantTracking
{
    [Serializable]
    public partial class ApplicantJobBoardPostingDto
    {
        public int PostingId { get; set; }
        public string Description { get; set; }
        public PostingType PostingTypeId { get; set; }
        public int PostingNumber { get; set; }
        public int ApplicationId { get; set; }
        public int? ClientDivisionId { get; set; }
        public int? ClientDepartmentId { get; set; }
        public int PostingCategoryId { get; set; }
        public int JobTypeId { get; set; }
        public string JobRequirements { get; set; }
        public string Location { get; set; }
        public string Salary { get; set; }
        public string HoursPerWeek { get; set; }
        public DateTime? StartDate { get; set; }
        public int NumOfPositions { get; set; }
        public int ApplicantsHired { get; set; }

        public bool IsEnabled { get; set; }
        public int ClientId { get; set; }
        public DateTime PublishedDate { get; set; }
        public int? JobProfileId { get; set; }
        public DateTime? PublishStart { get; set; }
        public DateTime? PublishEnd { get; set; }

        public int ApplicationCompletedCorrespondence { get; set; }
        public int ApplicantResumeRequiredId { get; set; }

        public bool IsPublished { get; set; }

        public bool IsClosed { get; set; }



        //Details
        public string PostingTypeDescription { get; set; }
        public string JobType { get; set; }
        public string Category { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
       // public string JobProfile { get; set; }
 
        public int? ApplicantsHeaderId { get; set; }
        public bool ApplicantHasApplied { get; set; }
        public bool ApplicantCompletedApplication { get; set; }

        public JobProfileDto JobProfile { get; set; }

    }
}