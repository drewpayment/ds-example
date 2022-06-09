using System;
using System.Collections.Generic;
using Dominion.Core.Dto.Core;
using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Benefit;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Entities.Employee;
using Dominion.Domain.Entities.Forms;
using Dominion.Domain.Interfaces.Entities;
using Dominion.Domain.Entities.Onboarding;
using Dominion.Core.Dto.Client;

namespace Dominion.Domain.Entities.Core
{
    /// <summary>
    /// Generic resource object used to manage file and URL information.
    /// </summary>
    public class Resource : Entity<Resource>, IHasModifiedData
    {
        public int                ResourceId   { get; set; } 
        public int?               ClientId     { get; set; } 
        public int?               EmployeeId   { get; set; }
        public int?               UserId       { get; set; }
        public string             Name         { get; set; } 
        public ResourceSourceType SourceType   { get; set; } 
        public string             Source       { get; set; } 
        public DateTime           AddedDate    { get; set; } 
        public int?               AddedById    { get; set; }
        public DateTime           Modified     { get; set; } 
        public bool               IsDeleted    { get; set; }
        public int                ModifiedBy   { get; set; }
        public int?               AzureAccount { get; set; }

        //FOREIGN KEYS
        public virtual Client                    Client             { get; set; } 
        public virtual Employee.Employee         Employee           { get; set; }
        public virtual User.User                 User               { get; set; }
        public virtual User.User                 ModifiedUser       { get; set; } 
        public virtual User.User                 AddedByUser        { get; set; }
        public virtual EmployeeAttachment        EmployeeAttachment { get; set; }
        public virtual Form                      Form               { get; set; }
        public virtual CompanyResource           CompanyResource    { get; set; }
        public virtual ImageResource             ImageResource      { get; set; }
        public virtual AzureResource             AzureResource      { get; set; }

        public virtual ICollection<OnboardingWorkflowResources> OnboardingWorkflowResources { get; set; }
    }
}
