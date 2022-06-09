export interface OpenTimeCards {
    approvedTimeCards: TimeCards[];
    unassignedEmployeeStatuses: UnassignedEmployeeStatuses[];
}

export interface TimeCards {
    approved: string;
    employeeApprovals: EmployeeApprovals[];
    employeeId: number;
    employeePayTypeAccess: number;
    emulatedSupervisorIds: EmulatedSupervisorIds[];
    firstName: string;
    isApproved: boolean;
    lastName: string;
    middleInitial: string;
    name: string;
    userId: number;
}

export interface EmployeeApprovals {
    approvalDates: Date;
    clientCostCenterId: number;
    clientDepartmentCode: string;
    clientDepartmentId: number;
    employeeId: number;
    firstName: string;
    hasSupervisor: boolean;
    isApproved: boolean;
    lastName: string;
    middleInitial: string;
    payFrequency: number;
    payType: number;
    periodEnd: Date;
    periodStart: Date;
}

export interface EmulatedSupervisorIds {

}

export interface UnassignedEmployeeStatuses {
    approvalDates: Date;
    clientCostCenterId: number;
    clientDepartmentCode: string;
    clientDepartmentId: number;
    employeeId: number;
    firstName: string;
    hasSupervisor: boolean;
    isApproved: boolean;
    lastName: string;
    middleInitial: string;
    payFrequency: number;
    payType: number;
    periodEnd: Date;
    periodStart: Date;
}