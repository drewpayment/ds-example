using System;
using System.Collections.Generic;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Entities.Clients;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Core
{
    /// <summary>
    /// Entity representation of a [dbo].[EmployeeFolder].
    /// </summary>
    public partial class EmployeeAttachmentFolder : Entity<EmployeeAttachmentFolder>, IHasModifiedOptionalData
    {
        public virtual int       EmployeeFolderId { get; set; } 
        public virtual int?      EmployeeId       { get; set; } 
        public virtual int?      ClientId         { get; set; } 
        public virtual string    Description      { get; set; } 
        public virtual bool      IsEmployeeView   { get; set; } 
        public virtual int?      ModifiedBy       { get; set; } 
        public virtual DateTime? Modified         { get; set; }
        public virtual bool      IsAdminViewOnly  { get; set; } 
        public virtual bool?     IsDefaultOnboardingFolder { get; set; }
        public virtual bool?     DefaultATFolder   { get; set; }
        public virtual bool?     IsDefaultPerformanceFolder { get; set; }
		
        public virtual Employee.Employee Employee { get; set; }
        public virtual Client            Client   { get; set; }
        
        public virtual ICollection<EmployeeAttachment> EmployeeAttachments { get; set; }
        public virtual ICollection<EmployeeFolderGroup> GroupAccess { get; set; }
    }
}