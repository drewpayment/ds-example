using System;
using System.Collections.Generic;
using Dominion.LaborManagement.Dto.ApplicantTracking;

namespace Dominion.Core.Dto.Labor
{
    [Serializable]
    public partial class ApplicantPostingDetailDto
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
        public DateTime? FilledDate { get; set; }
        public bool IsEnabled { get; set; }
        public int ClientId { get; set; }
        public DateTime PublishedDate { get; set; }
        public int? JobProfileId { get; set; }
        public DateTime? PublishStart { get; set; }
        public DateTime? PublishEnd { get; set; }
        public string StaffHired { get; set; }
        public int RejectionCorrespondence { get; set; }
        public int ApplicationCompletedCorrespondence { get; set; }
        public int? ApplicationReceivedTextCorrespondence { get; set; }
        public int ApplicantResumeRequiredId { get; set; }
        public int? MinSchooling { get; set; }
        public int ApplicantOnBoardingProcessId { get; set; }
        public bool IsPublished { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
        public DateTime? DisabledDate { get; set; }
        public bool IsForceMinSchoolingMatch { get; set; }
        public bool IsClosed { get; set; }
        //public int? PostingOwnerId { get; set; }
        public List<ApplicantPostingOwnerDto> PostingOwners { get; set; }
        public bool OwnerNotifications { get; set; }
        public bool IsGeneralApplication { get; set; }
        public string CompanyName { get; set; }
        public int? NumOfPositions { get; set; }
        public bool IsCoverLetterRequired { get; set; }

        //Details
        public string PostingTypeDescription { get; set; }
        public string JobType { get; set; }
        public string Category { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string Application { get; set; }
        public string JobProfile { get; set; }
        public string ResumeType { get; set; }
        public string SchoolDescription { get; set; }
        public string RejectionCorrespondenceDescription { get; set; }
        public string ApplicationCorrespondenceDescription { get; set; }
     
    }
}