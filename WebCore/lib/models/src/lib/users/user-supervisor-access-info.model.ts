
export interface UserSupervisorAccessInfo {
    userSupervisorSecuritySettingsId: number;
    userId: number;
    canSendPasswords: boolean;
    isAllowAddPunches: boolean;
    isAllowEditPunches: boolean;
    isAllowEditComments: boolean;
    isAllowApproveHours: boolean;
    isAllowEditCompanySchedules: boolean;
    isAllowEditEmployeeSetup: boolean;
    isAllowEditManualSchedules: boolean;
    isManagerLinks: boolean;
    attachmentSecurity: any;
    folderSecurity: any;
    isEmailLeaveMgmtRequests: boolean;
    isBlockDeductionPage: boolean;
    isViewOsha: boolean;
    isLimitCostCenters: boolean;
    isAllowEditGroupPlanner: boolean;
    certifyI9: boolean;
    isAllowAssignCompetencyModel: boolean;
}
