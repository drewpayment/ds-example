using System;

using Dominion.Domain.Entities.Base;
using Dominion.Domain.Interfaces.Entities;

namespace Dominion.Domain.Entities.Security
{
    /// <summary>
    /// Entity for dbo.UserSupervisorSecuritySettings table.
    /// </summary>
    public partial class UserSupervisorSecuritySetting : Entity<UserSupervisorSecuritySetting>, IHasModifiedData
    {
        public virtual int      UserSupervisorSecuritySettingsId { get; set; } 
        public virtual int      UserId                           { get; set; } 
        public virtual bool     IsAllowAddPunches                { get; set; } 
        public virtual bool     IsAllowEditPunches               { get; set; } 
        public virtual bool     IsAllowEditComments              { get; set; } 
        public virtual bool     IsAllowApproveHours              { get; set; } 
        public virtual DateTime Modified                         { get; set; } 
        public virtual int      ModifiedBy                       { get; set; } 
        public virtual bool     IsAllowEditCompanySchedules      { get; set; } 
        public virtual bool     IsAllowEditEmployeeSetup         { get; set; } 
        public virtual bool     IsAllowEditManualSchedules       { get; set; } 
        public virtual bool     IsManagerLinks                   { get; set; } 
        public virtual byte     AttachmentSecurity               { get; set; } 
        public virtual byte     FolderSecurity                   { get; set; } 
        public virtual bool     IsEmailLeaveMgmtRequests         { get; set; } 
        public virtual int      IsBlockDeductionPage             { get; set; }
        public virtual bool     IsRequestNobscotExitInterviews   { get; set; }
        public virtual bool     IsViewOsha                       { get; set; } 
        public virtual bool     IsLimitCostCenters               { get; set; } 
        public virtual bool     IsAllowEditGroupPlanner          { get; set; }
        public virtual bool     CertifyI9                        { get; set; }
        public virtual bool     AddEmployee                      { get; set; }
		public virtual bool     IsAllowAssignCompetencyModel     { get; set; }
        public virtual bool     AllowMarkEmployeePayrollReady    { get; set; }
        public virtual bool     CanSendPasswords                 { get; set; }

        public virtual User.User User { get; set; }
    }
}
