import { UserType } from '@ds/core/shared';


export interface UserPerformanceDashboardEmployee {
    userId: number;
    clientId: number;
    clientName: string;
    username: string;
    lastLoginDate: Date;
    isActive: boolean;
    firstName: string;
    lastName: string;
    userTypeId: UserType;
    supervisorId: number;
    supervisor: string;
    department: string;
    hireDate: Date;
    employeeStatus: string;
    employeeId: number;
    tempEnableFromDate: Date;
    tempEnableToDate: Date;
    viewEmployeePayTypes: string;
    viewEmployeeRateTypes: string;
    assignedEmployee: string;
    emailAddress: string;
    certifyI9: boolean;
    addEmployee: boolean;
    resetPassword: boolean;
    approveTimeCards: boolean;
    isSecurityEnabled: boolean;
    isPasswordEnabled: boolean;
    clientDepartmentName: string;
    clientDepartmentId: number;
}

export interface UserPerformanceDashboard {
    userId: number;
    userTypeId: UserType;
    firstName: string;
    lastName: string;
    emailAddress: string;
    employeeId: number;
    lastLoginDate: Date;
    viewEmployeePayTypes: string;
    viewEmployeeRateTypes: string;
    isSecurityEnabled: boolean;
    isPasswordEnabled: boolean;
    isEmployeeSelfServiceViewOnly: boolean;
    tempEnableFromDate: Date;
    tempEnableToDate: Date;
    isUserDisabled: boolean;
    department: string;
    supervisorId: number;
    supervisor: string;
    hireDate: Date;
    employeeStatus: string;
    assignedEmployee: string;
    certifyI9: boolean;
    addEmployee: boolean;
    resetPassword: boolean;
    approveTimecards: boolean;
    isActive: boolean;
    clientId: number;
    userName: string;
}

export interface UserPerformanceDashboardResult {
    eesWithUserCount: number;
    eesWoUserCount: number;
    lockedOutUsersCount: number;
    activeUsers: UserPerformanceDashboardEmployee[];
    eesWithUser: UserPerformanceDashboardEmployee[];
    eesWoUser: UserPerformanceDashboardEmployee[];
    isInFlight: boolean;
}

export interface UserPerformanceDashboardDialogData {
    users?: UserPerformanceDashboard[];
    employees?: UserPerformanceDashboardEmployee[];
    title: string;
}
