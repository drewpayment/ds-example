export interface IAdvanceEnrollmentReportDialogData {
    clientId: number;
}

export interface IEnrollmentTypeData {
    enrollmentTypeId: number;
    description: string;
}

export interface IEmployeeData {
    employeeId: number;
    firstName?: string;
    lastName?: string;
}

export interface IPlanData {
    planId: number;
    planName: string;
}

export interface IProviderData {
    planProviderId: number;
    name: string;
}

export interface IAdvanceEnrollmentReportConfigData {
    enrollmentTypeId?: number;
    employeeId?: number;
    planId?: number;
    providerId?: number;
    startDate?: Date;
    endDate?: Date;
    clientId: number;
}
