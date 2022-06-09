using System.Collections.Generic;
using System;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.ApplicantTracking
{
    /// <summary>
    /// EF entity representing a [dbo].[ApplicantCompanyApplication] record.
    /// </summary>
    public partial class ApplicantCompanyApplication : Entity<ApplicantCompanyApplication>, IHasModifiedOptionalData
    {
        public virtual int CompanyApplicationId { get; set; }
        public virtual string Description { get; set; }
        public virtual int ClientId { get; set; }
        public virtual bool IsEnabled { get; set; }
        public virtual int Education { get; set; }
        public virtual int ReferenceNo { get; set; }
        public virtual bool IsCurrentEmpApp { get; set; }
        public virtual bool IsExcludeHistory { get; set; }
        public virtual bool IsExcludeReferences { get; set; }
        public virtual bool IsFlagVolResign { get; set; }
        public virtual bool IsFlagNoEmail { get; set; }
        public virtual bool IsFlagReferenceCheck { get; set; }
        public virtual int YearsOfEmployment { get; set; }
        public virtual Client Client { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual int? ModifiedBy { get; set; }
        public virtual bool? IsExperience { get; set; }
        public virtual ICollection<ApplicantQuestionSet> ApplicantQuestionSets { get; set; }

        public ApplicantCompanyApplication()
        {
        }
    }
}