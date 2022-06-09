using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using System;
using Dominion.Core.Dto.Labor;
using Dominion.Core.Dto.Labor.Enum;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    public partial class ApplicantEducationHistory : Entity<ApplicantEducationHistory>
    {
        public virtual int ApplicantEducationId { get; set; }
        public virtual int ApplicantId { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime? DateStarted { get; set; }
        public virtual DateTime? DateEnded { get; set; }
        public virtual HasDegreeType HasDegree { get; set; }
        public virtual int? DegreeId { get; set; }
        public virtual string Studied { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual int? YearsCompleted { get; set; }
        public virtual int? ApplicantSchoolTypeId { get; set; }
        public string ExternalDegree { get; set; }

        //FOREIGN KEYS
        public virtual Applicant Applicant { get; set; }
        public virtual ApplicantDegreeList ApplicantDegreeList { get; set; }
        public virtual ApplicantSchoolType ApplicantSchoolType { get; set; }

        public ApplicantEducationHistory()
        {
        }
    }
}