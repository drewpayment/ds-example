using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;
using Dominion.Domain.Entities.Labor;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Domain.Entities.Employee;
using Dominion.Core.Dto.Employee;
using Dominion.LaborManagement.Dto.ApplicantTracking;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantPosting : Entity<ApplicantPosting>
    {
        public virtual int PostingId { get; set; }
        public virtual string Description { get; set; }
        public virtual PostingType PostingTypeId { get; set; }
        public virtual int PostingNumber { get; set; }
        public virtual int ApplicationId { get; set; }
        public virtual int? ClientDivisionId { get; set; }
        public virtual int? ClientDepartmentId { get; set; }
        public virtual int PostingCategoryId { get; set; }
        public virtual EmployeeStatusType JobTypeId { get; set; }
        public virtual string JobRequirements { get; set; }
        public virtual string Salary { get; set; }
        public virtual string HoursPerWeek { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? FilledDate { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual int ClientId { get; set; }
        public virtual DateTime PublishedDate { get; set; }
        public virtual int? JobProfileId { get; set; }
        public virtual DateTime? PublishStart { get; set; }
        public virtual DateTime? PublishEnd { get; set; }
        public virtual string StaffHired { get; set; }
        public virtual int RejectionCorrespondence { get; set; }
        public virtual int ApplicationCompletedCorrespondence { get; set; }
        public virtual int ApplicantResumeRequiredId { get; set; }
        public virtual int? MinSchooling { get; set; }
        public virtual int ApplicantOnBoardingProcessId { get; set; }
        public virtual bool IsPublished { get; set; }
        public virtual int? ModifiedBy { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual DateTime? DisabledDate { get; set; }
        public virtual bool IsForceMinSchoolingMatch { get; set; }
        public virtual bool IsClosed { get; set; }
        //public virtual int? PostingOwnerId { get; set; }
        public virtual bool OwnerNotifications { get; set; }
        public virtual int? NumOfPositions { get; set; }
        public virtual bool IsGeneralApplication { get; set; }
        public virtual bool IsCoverLetterRequired { get; set; }
        public virtual int? ReceivedTextCorrespondence { get; set; }
        //FOREIGN KEYS
        public virtual ApplicantPostingCategory ApplicantPostingCategory { get; set; }
        public virtual Client Client { get; set; }
        //public virtual ApplicantJobType ApplicantJobType { get; set; }
        public virtual ClientDepartment ClientDepartment { get; set; }
        public virtual ClientDivision ClientDivision { get; set; }
        public virtual ApplicantCompanyApplication ApplicantCompanyApplication { get; set; }
        public virtual JobProfile JobProfile { get; set; }
        public virtual ApplicantSchoolType ApplicantSchoolType { get; set; }
        public virtual ApplicantResumeRequired ApplicantResumeRequired { get; set; }
        //public virtual Dominion.Domain.Entities.User.User PostingOwner { get; set; } 
        public virtual ApplicantOnBoardingProcess ApplicantOnBoardingProcess { get; set; }
        public virtual EmployeeStatus EmployeeStatus { get; set; }

        public virtual ICollection<ApplicantApplicationHeader> ApplicantApplicationHeaders { get; set; }
        public virtual ICollection<ApplicantPostingOwner> ApplicantPostingOwners { get; set; }

        public ApplicantPosting()
        {
        }
    }
}