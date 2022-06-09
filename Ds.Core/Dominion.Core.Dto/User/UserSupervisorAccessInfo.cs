namespace Dominion.Core.Dto.User
{
    public class UserSupervisorAccessInfo
    {
        public int  UserSupervisorSecuritySettingsId { get; set; } 
        public int  UserId                           { get; set; } 
        public bool IsAllowAddPunches                { get; set; } 
        public bool IsAllowEditPunches               { get; set; } 
        public bool IsAllowEditComments              { get; set; } 
        public bool IsAllowApproveHours              { get; set; } 
        public bool IsAllowEditCompanySchedules      { get; set; } 
        public bool IsAllowEditEmployeeSetup         { get; set; } 
        public bool IsAllowEditManualSchedules       { get; set; } 
        public bool IsManagerLinks                   { get; set; } 
        public byte AttachmentSecurity               { get; set; } 
        public byte FolderSecurity                   { get; set; } 
        public bool IsEmailLeaveMgmtRequests         { get; set; } 
        public int IsBlockDeductionPage              { get; set; } 
        public bool IsViewOsha                       { get; set; } 
        public bool IsLimitCostCenters               { get; set; } 
        public bool IsAllowEditGroupPlanner          { get; set; }
        public bool CertifyI9                        { get; set; }
        public bool IsAllowAssignCompetencyModel     { get; set; }
        public bool CanSendPasswords { get; set; }
        public bool AddEmployee  { get; set; }
    }
}